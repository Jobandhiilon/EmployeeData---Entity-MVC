using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EmployeeData.Models;

namespace EmployeeData.DAL
{
    public class mySQLdbContext : DbContext
    {
        public mySQLdbContext(DbContextOptions<mySQLdbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>().ToTable("employees");
            builder.Entity<Employee>().Property(t => t.empid).HasColumnName("empid");
            builder.Entity<Employee>().Property(t => t.empname).HasColumnName("empname");
            builder.Entity<Employee>().Property(t => t.empdesignation).HasColumnName("empdesignation");
            builder.Entity<Employee>().Property(t => t.empcontact).HasColumnName("empcontact");
        }

        public virtual DbSet<Employee> Employees { get; set; }
    }

}
