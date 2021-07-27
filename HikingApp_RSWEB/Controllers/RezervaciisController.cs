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
using Microsoft.AspNetCore.Authorization;

namespace HikingApp_RSWEB.Controllers
{
    public class RezervaciisController : Controller
    {
        private readonly HikingApp_RSWEBContext _context;

        public RezervaciisController(HikingApp_RSWEBContext context)
        {
            _context = context;
        }

        // GET: Rezervaciis

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string tura)
        {
            IQueryable<Rezervacii> rezervacii = _context.Rezervacii.AsQueryable();
            IQueryable<string> turamesto = _context.Tura.OrderBy(m => m.Mesto).Select(m => m.Mesto).Distinct();
            

            if (!string.IsNullOrEmpty(tura))
            {
                rezervacii = rezervacii.Where(s => s.Tura.Mesto.Contains(tura));
            }
            /*

            if (!string.IsNullOrEmpty(tura))
            {

                rezervacii = rezervacii.Where(x => x.mesto == tura);
            }*/

            rezervacii = rezervacii.Include(r => r.Planinar).Include(r => r.Tura);
            var VM = new RezervaciiPoTura
            {
                Rezervacii = await rezervacii.ToListAsync(),
                TuraMesto = new SelectList(await turamesto.ToListAsync())
            };

            return View(VM);
        }


        // GET: Rezervaciis/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["PlaninarId"] = new SelectList(_context.Planinar, "Id", "Id");
            ViewData["TuraId"] = new SelectList(_context.Tura, "Id", "Id");
            return View();
        }

        // POST: Rezervaciis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,TuraId,PlaninarId")] Rezervacii rezervacii)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacii);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaninarId"] = new SelectList(_context.Planinar, "Id", "Id", rezervacii.PlaninarId);
            ViewData["TuraId"] = new SelectList(_context.Tura, "Id", "Id", rezervacii.TuraId);
            return View(rezervacii);
        }

        // GET: Rezervaciis/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacii = await _context.Rezervacii.FindAsync(id);
            if (rezervacii == null)
            {
                return NotFound();
            }
            ViewData["PlaninarId"] = new SelectList(_context.Planinar, "Id", "FullName", rezervacii.PlaninarId);
            ViewData["TuraId"] = new SelectList(_context.Tura, "Id", "Mesto", rezervacii.TuraId);
            return View(rezervacii);
        }

        // POST: Rezervaciis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TuraId,PlaninarId")] Rezervacii rezervacii)
        {
            if (id != rezervacii.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacii);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervaciiExists(rezervacii.Id))
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
            ViewData["PlaninarId"] = new SelectList(_context.Planinar, "Id", "FullName", rezervacii.PlaninarId);
            ViewData["TuraId"] = new SelectList(_context.Tura, "Id", "Mesto", rezervacii.TuraId);
            return View(rezervacii);
        }

        // GET: Rezervaciis/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacii = await _context.Rezervacii
                .Include(r => r.Planinar)
                .Include(r => r.Tura)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacii == null)
            {
                return NotFound();
            }

            return View(rezervacii);
        }

        // POST: Rezervaciis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacii = await _context.Rezervacii.FindAsync(id);
            _context.Rezervacii.Remove(rezervacii);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervaciiExists(int id)
        {
            return _context.Rezervacii.Any(e => e.Id == id);
        }

        public async Task<IActionResult> NewRezz()
        {
            IQueryable<Tura> turi = _context.Tura;
            IEnumerable<Planinar> planinari = _context.Planinar;

            NewRezzViewModel ViewModel = new NewRezzViewModel
            {
                Turi = new SelectList(await turi.ToListAsync(), "Id", "Mesto"),
                Planinari = new SelectList(planinari.OrderBy(s => s.FullName).ToList(), "Id", "FullName"),
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewRezz(NewRezzViewModel ViewModel)
        {
            var tura = await _context.Tura.FirstOrDefaultAsync(c => c.Id == ViewModel.TuraId);
            if (tura == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach (int planinarId in ViewModel.SelectedPlaninari)
                {
                    Rezervacii enroll = await _context.Rezervacii
                        .FirstOrDefaultAsync(c => c.TuraId == ViewModel.TuraId && c.PlaninarId == planinarId);
                    if (enroll == null)
                    {

                        _context.Add(new Rezervacii { PlaninarId = planinarId, TuraId = ViewModel.TuraId });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Planinar")]
        public async Task<IActionResult> PlaninarViewModel(int? id)
        {

            if (id == null)
            {
                //id = 1;
                return NotFound();
            }

            var planinar = await _context.Planinar.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = planinar;

            
           
            if (planinar == null)
            {
                return NotFound();
            }
            var HikingAppContext = _context.Rezervacii.Where(m => m.PlaninarId == id).Include(m => m.Planinar).Include(m => m.Tura);
            return View(await HikingAppContext.ToListAsync());
        }


        [Authorize(Roles = "Vodich")]
        public async Task<IActionResult> PlaninariNaTura(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Tura.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = course;
            if (course == null)
            {
                return NotFound();
            }
            var HikingAppContext = _context.Rezervacii.Where(m => m.TuraId == id).Include(m => m.Planinar).Include(m => m.Tura);
            return View(await HikingAppContext.ToListAsync());
        }


    }
}
