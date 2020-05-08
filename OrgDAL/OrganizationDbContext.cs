using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrgDAL
{
    public class OrganizationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=vmadmin4323\SQLEXPRESS;Database=OrgAPIDb;Trusted_Connection=True;");
        }

        public DbSet<Department> Departments { get; set; }
    }
}
