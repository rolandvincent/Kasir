using Kasir.DbContexts;
using Kasir.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Kasir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            CultureInfo customCulture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = customCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            iCassierDbContextFactory iCassierDbContextFactory = new iCassierDbContextFactory();
            using (iCassierDbContext iCassierDbContext = iCassierDbContextFactory.CreateDbContext(Array.Empty<string>()))
            {
                await iCassierDbContext.Database.EnsureCreatedAsync();
                if (!iCassierDbContext.Users.Any())
                {
                    //await iCassierDbContext.Database.MigrateAsync();
                    await DbSeeder.StartSeederAsync(iCassierDbContext);
                }
            }

            MainWindow main = new MainWindow();
            main.DataContext = new NavigationVM(main);
            main.Show();
            base.OnStartup(e);
        }
    }
}
