using WebApi.Models;

namespace WebApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApi.Models.CustomerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApi.Models.CustomerContext context)
        {
            context.CustomerComplaints.AddOrUpdate(
                new Complaints() { CustomerId = "1", Description = "too expensive" },
                new Complaints() { CustomerId = "1", Description = "too bad" },
                new Complaints() { CustomerId = "1", Description = "too soon" },
                new Complaints() { CustomerId = "2", Description = "too late" },
                new Complaints() { CustomerId = "2", Description = "too too" },
                new Complaints() { CustomerId = "2", Description = "too wrong" },
                new Complaints() { CustomerId = "2", Description = "too sad" },
                new Complaints() { CustomerId = "3", Description = "wrong delivery" },
                new Complaints() { CustomerId = "3", Description = "wrong comodity" },
                new Complaints() { CustomerId = "3", Description = "wrong person" },
                new Complaints() { CustomerId = "3", Description = "wrong address" },
                new Complaints() { CustomerId = "3", Description = "wrong manner" }
                );
        }
    }
}
