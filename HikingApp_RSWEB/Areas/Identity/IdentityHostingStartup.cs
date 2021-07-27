using System;
using HikingApp_RSWEB.Areas.Identity.Data;
using HikingApp_RSWEB.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(HikingApp_RSWEB.Areas.Identity.IdentityHostingStartup))]
namespace HikingApp_RSWEB.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<HikingApp_RSWEBContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("HikingApp_RSWEBContext")));

             
            });
        }
    }
}