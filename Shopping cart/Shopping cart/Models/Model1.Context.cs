﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shopping_cart.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyConn : DbContext
    {
        public MyConn()
            : base("name=MyConn")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Delivery_Note> Delivery_Note { get; set; }
        public virtual DbSet<Detail_Order> Detail_Order { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
    }
}
