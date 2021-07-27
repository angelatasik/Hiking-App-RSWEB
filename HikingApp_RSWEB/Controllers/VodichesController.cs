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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace HikingApp_RSWEB.Controllers
{
    public class VodichesController : Controller
    {
        private readonly HikingApp_RSWEBContext _context;
        private readonly IHostingEnvironment webHostingEnvironment;

        public VodichesController(HikingApp_RSWEBContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            webHostingEnvironment = hostingEnvironment;
        }

        // GET: Vodiches

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string SearchPozicija)
        {
            //return View(await _context.Vodich.ToListAsync());
            IQueryable<Vodich> vodichi = _context.Vodich.AsQueryable();
            IQueryable<string> poziciiQuery = _context.Vodich.OrderBy(m => m.Pozicija).Select(m => m.Pozicija).Distinct();
            if (!string.IsNullOrEmpty(SearchPozicija))
            {

                vodichi = vodichi.Where(x => x.Pozicija == SearchPozicija);
            }

            var VM = new FiltrirajVodichi
            {
                Vodichi = await vodichi.ToListAsync(),
                Pozicii = new SelectList(await poziciiQuery.ToListAsync())
            };

            return View(VM);
        }

        // GET: Vodiches/Details/5

        [Authorize(Roles = "Admin,Vodich,Planinar")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vodich = await _context.Vodich
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vodich == null)
            {
                return NotFound();
            }

            return View(vodich);
        }

        // GET: Vodiches/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vodiches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VodichViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Vodich vodich = new Vodich
                {
                    Id = model.Id,
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    Vozrast = model.Vozrast,
                    ProfilePicture = uniqueFileName
                };
                _context.Add(vodich);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        private string UploadedFile(VodichViewModel model)
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


        // GET: Vodiches/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vodich = await _context.Vodich.FindAsync(id);
            if (vodich == null)
            {
                return NotFound();
            }
            return View(vodich);
        }

        // POST: Vodiches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, IFormFile imageUrl, [Bind("Id,Ime,Prezime,Pozicija,Vozrast,ProfilePicture")] Vodich vodich)
        {
            if (id != vodich.Id)
            {
                return NotFound();
            }
            VodichesController uploadImage = new VodichesController(_context, webHostingEnvironment);
            vodich.ProfilePicture = uploadImage.UploadedFile(imageUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vodich);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VodichExists(vodich.Id))
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
            return View(vodich);
        }

        // GET: Vodiches/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vodich = await _context.Vodich
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vodich == null)
            {
                return NotFound();
            }

            return View(vodich);
        }

        // POST: Vodiches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vodich = await _context.Vodich.FindAsync(id);
            _context.Vodich.Remove(vodich);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        private bool VodichExists(int id)
        {
            return _context.Vodich.Any(e => e.Id == id);
        }
    }
}
