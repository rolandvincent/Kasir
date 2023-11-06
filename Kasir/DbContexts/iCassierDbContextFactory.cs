using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kasir.DbContexts
{
    public class iCassierDbContextFactory : IDesignTimeDbContextFactory<iCassierDbContext>
    {
        private readonly string _connectionString = "server=localhost;user=root;password=;database=iCassierDB";
        public iCassierDbContext CreateDbContext(string[] args)
        {
            iCassierDbContext? dbContext = null;
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<iCassierDbContext>();
                optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));

                dbContext = new iCassierDbContext(optionsBuilder.Options);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "iCassier MySQL", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
            return dbContext;
        }
    }
}
