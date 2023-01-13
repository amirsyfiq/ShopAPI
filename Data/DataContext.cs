﻿namespace ShopAPI.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=shopapidb;Trusted_Connection=true;");
        //}

        public DbSet<Customer> Customers { get; set; }
    }
}