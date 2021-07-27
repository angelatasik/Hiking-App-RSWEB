using HikingApp_RSWEB.Areas.Identity.Data;
using HikingApp_RSWEB.Data;
using HikingApp_RSWEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HikingApp_RSWEBContext _context;
        private readonly UserManager<HikingApp_RSWEBUser> userManager;

        public HomeController(ILogger<HomeController> logger, HikingApp_RSWEBContext context, UserManager<HikingApp_RSWEBUser> usrMgr)
        {
            _logger = logger;
            _context = context;
            userManager = usrMgr;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Turas");
            }
            else if (User.IsInRole("Vodich"))
            {
                var userID = userManager.GetUserId(User);
                HikingApp_RSWEBUser user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("TuriPoVodich", "Turas", new { id = user.VodichId });
            }
            else if (User.IsInRole("Planinar"))
            {
                var userID = userManager.GetUserId(User);
                HikingApp_RSWEBUser user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("PlaninarViewModel", "Rezervaciis", new { id = user.PlaninarId });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
