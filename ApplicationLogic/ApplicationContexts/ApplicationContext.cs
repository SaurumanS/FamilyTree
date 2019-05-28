using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using Account = ApplicationLogic.DataBaseTableInstances.Account;
using Admin = ApplicationLogic.DataBaseTableInstances.Admin;
using Family = ApplicationLogic.DataBaseTableInstances.Family;
using Person = ApplicationLogic.DataBaseTableInstances.Person;

namespace ApplicationLogic.ApplicationContexts
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = Environment.CurrentDirectory + @"\Data.db" }.ConnectionString
                }, true)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Family> Families { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> People { get; set; }
    }
}
