using Kasir.DbContexts;
using Kasir.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;

namespace Kasir
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            Unosquare.FFME.Library.FFmpegDirectory = @"C:\ffmpeg";

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
