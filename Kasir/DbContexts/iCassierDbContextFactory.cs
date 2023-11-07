using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Configuration;
using System.Windows;

namespace Kasir.DbContexts
{
    public class iCassierDbContextFactory : IDesignTimeDbContextFactory<iCassierDbContext>
    {
        public iCassierDbContext CreateDbContext(string[] args)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionStrings"] ?? "server=localhost;user=root;password=;database=iCassierDB";
            string DB_Type = ConfigurationManager.AppSettings["databaseType"] ?? "MySQL";
            iCassierDbContext? dbContext = null;
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<iCassierDbContext>();
                if (DB_Type.ToLower() == "mysql")
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                else if(DB_Type.ToLower() == "sqlserver")
                    optionsBuilder.UseSqlServer(connectionString);
                else if (DB_Type.ToLower() == "sqlite")
                    optionsBuilder.UseSqlite(connectionString);
                else { 
                    MessageBox.Show($"Connection Type '{DB_Type}' not supported.", "iCassier SQL Connection", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(1);
                }

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
