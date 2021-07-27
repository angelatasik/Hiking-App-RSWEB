using HikingApp_RSWEB.Areas.Identity.Data;
using HikingApp_RSWEB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.Data
{
    public class HikingApp_RSWEBContext : IdentityDbContext<HikingApp_RSWEBUser>
    {
        public HikingApp_RSWEBContext(DbContextOptions<HikingApp_RSWEBContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("AppDBContextConnection");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Tura>().HasOne(p => p.FirstVodich).WithMany(p => p.Tura1).HasForeignKey(p => p.FirstVodichId);
            builder.Entity<Tura>().HasOne(p => p.SecoundVodich).WithMany(p => p.Tura2).HasForeignKey(p => p.SecoundVodichId);

            base.OnModelCreating(builder);
        }

        public DbSet<HikingApp_RSWEB.Models.Vodich> Vodich { get; set; }

        public DbSet<HikingApp_RSWEB.Models.Tura> Tura { get; set; }

        public DbSet<HikingApp_RSWEB.Models.Rezervacii> Rezervacii { get; set; }

        public DbSet<HikingApp_RSWEB.Models.Planinar> Planinar { get; set; }

    }
}
