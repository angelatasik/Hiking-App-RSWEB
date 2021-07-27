using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HikingApp_RSWEB.Data;
using HikingApp_RSWEB.Models;
using HikingApp_RSWEB.ViewModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace HikingApp_RSWEB.Controllers
{
    public class PlaninarsController : Controller
    {
        private readonly HikingApp_RSWEBContext _context;

        private readonly IHostingEnvironment webHostingEnvironment;


        public PlaninarsController(HikingApp_RSWEBContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;

            webHostingEnvironment = hostingEnvironment;
        }

        // GET: Planinars

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchStringName, string searchStringSurname)
        {
            //return View(await _context.Planinar.ToListAsync());
            IQueryable<Planinar> planinari = _context.Planinar.AsQueryable();
            if (!string.IsNullOrEmpty(searchStringName))
            {
                planinari = planinari.Where(s => s.Ime.Contains(searchStringName));
            }
            if (!string.IsNullOrEmpty(searchStringSurname))
            {
                planinari = planinari.Where(s => s.Prezime.Contains(searchStringSurname));
            }
           
            var VM = new FiltrirajPlaninari
            {
                Planinari = await planinari.ToListAsync()
            };

            return View(VM);
        }

        // GET: Planinars/Details/5

        [Authorize(Roles = "Admin,Planinar,Vodich")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planinar = await _context.Planinar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planinar == null)
            {
                return NotFound();
            }

            return View(planinar);
        }

        // GET: Planinars/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Planinars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,Vozrast,ProfilePicture")] Planinar planinar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planinar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planinar);
        }*/

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PlaninarViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Planinar planinar = new Planinar
                {
                    Id = model.Id,
                    Ime = model.FirstName,
                    Prezime = model.LastName,
                    Vozrast = model.Age,
                    ProfilePicture = uniqueFileName
                };
                _context.Add(planinar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadedFile(PlaninarViewModel model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        //Oveloaded function UploadedFile for Edit
        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Planinars/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planinar = await _context.Planinar.FindAsync(id);
            if (planinar == null)
            {
                return NotFound();
            }
            return View(planinar);
        }

        // POST: Planinars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, IFormFile imageUrl,[Bind("Id,Ime,Prezime,Vozrast,ProfilePicture")] Planinar planinar)
        {
            if (id != planinar.Id)
            {
                return NotFound();
            }

            PlaninarsController uploadImage = new PlaninarsController(_context, webHostingEnvironment);
            planinar.ProfilePicture = uploadImage.UploadedFile(imageUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planinar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaninarExists(planinar.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(planinar);
        }

        // GET: Planinars/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planinar = await _context.Planinar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planinar == null)
            {
                return NotFound();
            }

            return View(planinar);
        }

        // POST: Planinars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planinar = await _context.Planinar.FindAsync(id);
            _context.Planinar.Remove(planinar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        private bool PlaninarExists(int id)
        {
            return _context.Planinar.Any(e => e.Id == id);
        }
    }
}
