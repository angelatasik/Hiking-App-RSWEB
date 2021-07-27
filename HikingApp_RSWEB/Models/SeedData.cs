using HikingApp_RSWEB.Areas.Identity.Data;
using HikingApp_RSWEB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<HikingApp_RSWEBUser>>();
            IdentityResult roleResult;

            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            //Add Admin User
            HikingApp_RSWEBUser user = await UserManager.FindByEmailAsync("admin1@hiking.com");
            if (user == null)
            {
                var User = new HikingApp_RSWEBUser();
                User.Email = "admin1@hikinga.com";
                User.UserName = "admin1@hiking.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            
            //Add Planinar Role
            var roleCheck2 = await RoleManager.RoleExistsAsync("Planinar");
            if (!roleCheck2) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Planinar")); }
            //Add Planinar User
            HikingApp_RSWEBUser user2 = await UserManager.FindByEmailAsync("planinar1@hiking.com");
            if (user2 == null)
            {
                var User = new HikingApp_RSWEBUser();
                User.Email = "planinar1@hiking.com";
                User.UserName = "planinar1@hiking.com";
                User.PlaninarId = 1;
                string userPWD = "Planinar123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Planinar
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Planinar"); }
            }

            //Add Vodich Role
            var roleCheck3 = await RoleManager.RoleExistsAsync("Vodich");
            if (!roleCheck3) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Vodich")); }

            //Add Vodich User
            HikingApp_RSWEBUser user3 = await UserManager.FindByEmailAsync("vodich1@hiking.com");
            if (user3 == null)
            {
                var User = new HikingApp_RSWEBUser();
                User.Email = "vodich1@hiking.com";
                User.UserName = "vodich1@hiking.com";
                User.VodichId = 1;
                string userPWD = "Vodich123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Vodich
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Vodich"); }
            }

            HikingApp_RSWEBUser user4 = await UserManager.FindByEmailAsync("vodich2@hiking.com");
            if (user4 == null)
            {
                var User = new HikingApp_RSWEBUser();
                User.Email = "vodich2@hiking.com";
                User.UserName = "vodich2@hiking.com";
                User.VodichId = 2;
                string userPWD = "Vodich123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Vodich
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Vodich"); }
            }

            var roleCheck6 = await RoleManager.RoleExistsAsync("Planinar");
            if (!roleCheck6) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Planinar")); }
            //Add Planinar User
            HikingApp_RSWEBUser user6 = await UserManager.FindByEmailAsync("planinar2@hiking.com");
            if (user6 == null)
            {
                var User = new HikingApp_RSWEBUser();
                User.Email = "planinar2@hiking.com";
                User.UserName = "planinar2@hiking.com";
                User.PlaninarId = 2;
                string userPWD = "Planinar123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Planinar
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Planinar"); }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HikingApp_RSWEBContext(
                serviceProvider.GetRequiredService<DbContextOptions<HikingApp_RSWEBContext>>()))
            {

                CreateUserRoles(serviceProvider).Wait();

                if (context.Tura.Any() || context.Vodich.Any() || context.Planinar.Any())
                {
                    return;
                }

                context.Vodich.AddRange(
                    new Vodich { Ime = "Стефан", Prezime = "Петров", Pozicija = "Помошник водич", Vozrast = 30},
                    new Vodich { Ime = "Горан", Prezime = "Стојков", Pozicija = "Главен водич", Vozrast = 28},
                    new Vodich { Ime = "Иван", Prezime = "Зафиров", Pozicija = "Водич со лиценца", Vozrast = 35},
                    new Vodich { Ime = "Милан", Prezime = "Павлов", Pozicija = "Главен водич", Vozrast = 27},
                    new Vodich { Ime = "Трајче", Prezime = "Дамчев", Pozicija = "Водич со лиценца", Vozrast = 26}


                );
                context.SaveChanges();

                context.Planinar.AddRange(
                    new Planinar { Ime = "Ангела", Prezime = "Тасиќ", Vozrast = 22 },
                    new Planinar { Ime = "Сара", Prezime = "Попова", Vozrast = 18 },
                    new Planinar { Ime = "Горан", Prezime = "Илиев", Vozrast = 25 },
                    new Planinar { Ime = "Раде", Prezime = "Радевски", Vozrast = 28 },
                    new Planinar { Ime = "Мила", Prezime = "Павловска", Vozrast = 32 }
                    );
                context.SaveChanges();

                context.Tura.AddRange(
                    new Tura
                    {
                        Mesto = "Кањон Плачковица",
                        DatumPocetok = DateTime.Parse("2021-05-05"),
                        DatumKraj= DateTime.Parse("2021-05-05"),
                        Tezina = "Лесна",
                        Vremetraenje = "3 часа",
                        FirstVodichId = context.Vodich.Single(d => d.Ime == "Стефан" && d.Prezime == "Петров").Id,
                        SecoundVodichId = context.Vodich.Single(d => d.Ime == "Горан" && d.Prezime == "Стојков").Id
                    },
                    new Tura
                    {
                        Mesto = "Шар Планина",
                        DatumPocetok = DateTime.Parse("2018-06-06"),
                        DatumKraj = DateTime.Parse("2018-06-07"),
                        Tezina = "Тешка",
                        Vremetraenje = "7 часа",
                        FirstVodichId = context.Vodich.Single(d => d.Ime == "Стефан" && d.Prezime == "Петров").Id,
                        SecoundVodichId = context.Vodich.Single(d => d.Ime == "Милан" && d.Prezime == "Павлов").Id
                    },
                    new Tura
                    {
                        Mesto = "Кампинг Црно Езеро",
                        DatumPocetok = DateTime.Parse("2018-08-08"),
                        DatumKraj = DateTime.Parse("2018-08-10"),
                        Tezina = "Средно Лесна",
                        Vremetraenje = "12 часа",
                        FirstVodichId = context.Vodich.Single(d => d.Ime == "Трајче" && d.Prezime == "Дамчев").Id,
                        SecoundVodichId = context.Vodich.Single(d => d.Ime == "Горан" && d.Prezime == "Стојков").Id
                    },
                    new Tura
                    {
                        Mesto = "Крчин",
                        DatumPocetok = DateTime.Parse("2018-09-09"),
                        DatumKraj = DateTime.Parse("2018-09-09"),
                        Tezina = "Тешка",
                        Vremetraenje = "6 часа",
                        FirstVodichId = context.Vodich.Single(d => d.Ime == "Иван" && d.Prezime == "Зафиров").Id,
                        SecoundVodichId = context.Vodich.Single(d => d.Ime == "Горан" && d.Prezime == "Стојков").Id
                    }



                );
                context.SaveChanges();

                context.Rezervacii.AddRange(
                    new Rezervacii { PlaninarId = 1, TuraId = 1 },
                    new Rezervacii { PlaninarId= 1, TuraId = 2 },
                    new Rezervacii { PlaninarId = 1, TuraId = 4 },
                    new Rezervacii { PlaninarId = 2, TuraId = 1 },
                    new Rezervacii { PlaninarId = 2, TuraId = 3 },
                    new Rezervacii { PlaninarId = 2, TuraId = 5 }
                   
                );

                context.SaveChanges();
            }
        }
    }

}
