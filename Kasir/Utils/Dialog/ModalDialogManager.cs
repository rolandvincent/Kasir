using Kasir.Utils.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kasir.Utils.Dialog
{
    public class ModalDialogManager
    {
        public Grid GridContainer { get; }

        private List<MessageToast> _messageToasts = new List<MessageToast>();
        private MessageToast? currentMessageToast = null;

        public ModalDialogManager(Grid GridContainer)
        {
            this.GridContainer = GridContainer;
        }

        public PopupModal CreateNewPopupModal(string Title, object Content)
        {
            PopupModal modal = new PopupModal();
            modal.Visibility = System.Windows.Visibility.Collapsed;
            modal.Header = Title;
            modal.Content = Content;
            GridContainer.Children.Add( modal);

            modal.PopupCloseAnimationFinished += Modal_PopupCloseAnimationFinished;
            return modal;
        }

        public void MessageEqueue(object message, bool promote = false, Action<MessageToast> OnClick = null, bool isCanHit = true, bool canClose = false)
        {
            MessageEqueue(message, TimeSpan.FromSeconds(3), promote, OnClick, isCanHit, canClose);
        }

        public void MessageQueueClear()
        {
            _messageToasts.Clear();
        }

        public void MessageEqueue(MessageToast messageToast, bool promote = false, Action<MessageToast> OnClick = null)
        {
            messageToast.ToastCloseAnimationFinished += MessageToast_ToastCloseAnimationFinished;
            if (OnClick != null)
            {
                messageToast.Cursor = Cursors.Hand;
                messageToast.ToastClick += delegate { OnClick(messageToast); };
            }
            if (promote)
                _messageToasts.Insert(0, messageToast);
            else
                _messageToasts.Add(messageToast);
            MessageDequeueRequest();
        } 

        public void MessageQueueDropUntil(int total)
        {
            if (_messageToasts.Count() > total)
                _messageToasts.RemoveRange(total - 1, _messageToasts.Count() - total);
        }

        public void MessageEqueue(object message, TimeSpan? duration, bool promote = false, Action<MessageToast> OnClick = null, bool isCanHit = true, bool canClose = false)
        {
            MessageToast messageToast = new MessageToast() {
                Content = message,
                Duration = duration ?? TimeSpan.FromDays(1),
                CloseButtonVisibility = canClose? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed
            };
            messageToast.ToastCloseAnimationFinished += MessageToast_ToastCloseAnimationFinished;
            messageToast.IsHitTestVisible = isCanHit;
            if (OnClick != null)
            {
                messageToast.Cursor = Cursors.Hand;
                messageToast.ToastClick += delegate { OnClick(messageToast); };
            }
            if (promote)
                _messageToasts.Insert(0, messageToast);
            else
                _messageToasts.Add(messageToast);
            MessageDequeueRequest();
        }

        private void MessageDequeueRequest()
        {
            if (currentMessageToast == null && _messageToasts.Count > 0)
            {
                currentMessageToast = _messageToasts.First();
                _messageToasts.RemoveAt(0);
                Grid.SetZIndex(currentMessageToast, 99);
                GridContainer.Children.Add(currentMessageToast);
            }
        }

        private void MessageToast_ToastCloseAnimationFinished(object? sender, EventArgs e)
        {
            if (currentMessageToast != null)
            {
                GridContainer.Children.Remove(currentMessageToast);
                currentMessageToast.Dispose();
                currentMessageToast = null;
            }
            MessageDequeueRequest();
        }

        private void Modal_PopupCloseAnimationFinished(object? sender, EventArgs e)
        {
            PopupModal modal = sender as PopupModal;
            GridContainer.Children.Remove(modal);
            modal.Dispose();
        }
    }
}
