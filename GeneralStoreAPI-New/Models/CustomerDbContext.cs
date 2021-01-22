using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI_New.Models
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}