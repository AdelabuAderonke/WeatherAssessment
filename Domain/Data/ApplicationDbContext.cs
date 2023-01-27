using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new DbInitializer(builder).Seed();
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Weather> Weathers { get; set; }
    }

    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<Weather>().HasData(
                   new Weather() { Id = 1, Description = "Chilly", Date = DateTime.Now.AddDays(-2), TemperatureC = "23C" }, 
                   new Weather() { Id = 2, Description = "Hot", Date = DateTime.Now.AddDays(-1), TemperatureC = "50C" }
            ); 
        }
    }
}
