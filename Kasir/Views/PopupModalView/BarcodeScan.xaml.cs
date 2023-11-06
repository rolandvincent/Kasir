using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;
using ZXing;
using System.Drawing.Imaging;

namespace Kasir.Views.PopupModalView
{

    public class BarcodeDetectedEventArgs : EventArgs
    {
        public BarcodeDetectedEventArgs(string BarcodeData)
        {
            this.BarcodeData = BarcodeData;
        }
        public string BarcodeData { get; set; }
    }

    /// <summary>
    /// Interaction logic for BarcodeScan.xaml
    /// </summary>
    public partial class BarcodeScan : UserControl, IDisposable
    {
        private FilterInfoCollection videoDevices; // List of available video devices
        private VideoCaptureDevice videoSource; // Video capture device
        private bool isCameraRunning = false;

        public delegate void BarcodeEventHandler(object sender, BarcodeDetectedEventArgs e);
        public event BarcodeEventHandler BarcodeDetected;

        public BarcodeScan()
        {
            InitializeComponent();

            Loaded += BarcodeScan_Loaded;
        }

        private void BarcodeScan_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            barcodeReaderGeneric = new BarcodeReaderGeneric();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach(FilterInfo info in videoDevices)
            {
                DeviceSelector.Items.Add(info.Name);
            }

            DeviceSelector.SelectedIndex = 0;
            DeviceSelector.SelectionChanged += DeviceSelector_SelectionChanged;

            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[DeviceSelector.SelectedIndex].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
                isCameraRunning = true;
            }
            else
            {
                MessageBox.Show("No camera devices found.");
            }

            BarcodeDetected += BarcodeScan_BarcodeDetected;
        }

        private void DeviceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }

            videoSource = new VideoCaptureDevice(videoDevices[DeviceSelector.SelectedIndex].MonikerString);
            videoSource.NewFrame -= VideoSource_NewFrame;
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();
        }

        private void BarcodeScan_BarcodeDetected(object sender, BarcodeDetectedEventArgs e)
        {
            Dispose();
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            IntPtr hBitmap = bitmap.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        public byte[] ConvertBitmapToRGBBytes(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }

            int width = bitmap.Width;
            int height = bitmap.Height;

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            IntPtr scan0 = bitmapData.Scan0;
            int stride = bitmapData.Stride;
            int bytesPerPixel = 3; // 3 bytes for RGB (24 bits)
            int bufferSize = stride * height;

            byte[] rgbBytes = new byte[bufferSize];

            Marshal.Copy(scan0, rgbBytes, 0, bufferSize);

            bitmap.UnlockBits(bitmapData);
            return rgbBytes;
        }

        private BarcodeReaderGeneric barcodeReaderGeneric;

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (isCameraRunning)
            {
                // Display the video frame in the MediaElement
                VideoOutput.Dispatcher.Invoke(() =>
                {
                    var frame = eventArgs.Frame;
                    VideoOutput.Source = ConvertBitmapToImageSource(frame);

                    
                    var result = barcodeReaderGeneric.Decode(new RGBLuminanceSource(ConvertBitmapToRGBBytes(frame), frame.Size.Width, frame.Height));
                    if (result != null)
                    {
                        BarcodeDetected?.Invoke(this, new BarcodeDetectedEventArgs(result.ToString()));
                    }
                });
            }
        }

        public void Dispose()
        {
            videoSource.NewFrame -= VideoSource_NewFrame;
            if (isCameraRunning)
            {
                videoSource.SignalToStop();
                isCameraRunning = false;
            }
        }
    }


}
