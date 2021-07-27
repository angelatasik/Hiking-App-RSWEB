using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HikingApp_RSWEB.Data;
using HikingApp_RSWEB.Models;
using Microsoft.AspNetCore.Authorization;
using HikingApp_RSWEB.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using HikingApp_RSWEB.ViewModel;

namespace HikingApp_RSWEB.Controllers
{
    public class TurasController : Controller
    {
        private readonly HikingApp_RSWEBContext _context;

        public TurasController(HikingApp_RSWEBContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Vodich")]
        public async Task<IActionResult> TuriPoVodich(int? id)
        {

            var vodich = await _context.Vodich.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = vodich;

            if (id == null)
            {
                return NotFound();
            }

            if (vodich == null)
            {
                return NotFound();
            }
            var HikingAppContext = _context.Tura.Where(m => (m.FirstVodichId == id) || (m.SecoundVodichId == id)).Include(c => c.FirstVodich).Include(c => c.SecoundVodich);
            return View(await HikingAppContext.ToListAsync());
        }

        // GET: Turas

        [Authorize(Roles = "Admin, Vodich,Planinar")]
        public async Task<IActionResult> Index(string tezina)
        {
            IQueryable<Tura> turi = _context.Tura.AsQueryable();
            IQueryable<Vodich> vodichi = _context.Vodich.AsQueryable();
            IQueryable<string> tezinaQuery = _context.Tura.OrderBy(m => m.Tezina).Select(m => m.Tezina).Distinct();
            //IQueryable<string>  vodichiQuery = _context.Vodich.OrderBy(m => m.FullName).Select(m => m.FullName).Distinct();
            //IQueryable<DateTime>  datumQuery = _context.Tura.OrderBy(m => m.DatumPocetok).Select(m => m.DatumPocetok).Distinct();
            /*
            if (!string.IsNullOrEmpty(vodichii))
            {
                //var fullname = vodichi.Where(s => s.FullName.Contains(vodichii));
               // turi = turi.Where(x => x.FirstVodich.FullName == vodichii);
               turi = turi.Where(x => x.FirstVodich.FullName.Contains(vodichii));
            }*/
            /*if (!string.IsNullOrEmpty(searchStringName))
            {
                vodichi = vodichi.Where(s => s.FullName.Contains(searchStringName) || s.Ime.Contains(searchStringName) || s.Prezime.Contains(searchStringName));
                //turi = turi.Where(s => s.FirstVodich == vodichi || s.SecoundVodich == vodichi);
                //turi = turi.Where(s => s.FirstVodich.Ime.Contains(searchStringName));
            }*/
            if (!string.IsNullOrEmpty(tezina))
            {
                turi = turi.Where(x => x.Tezina == tezina);
            }
            /*
            if (datum!=null)
            {
                turi = turi.Where(x => x.DatumPocetok == datum);
            }
            */
            //turi = turi.Include(c => c.FirstVodich).Include(c => c.SecoundVodich).Include(m => m.Planinari).ThenInclude(m => m.Planinar);
            
            var viewmodel = new FiltrirajTuri
            {
                //Datumi = new SelectList(await datumQuery.ToListAsync()),
                Tezini = new SelectList(await tezinaQuery.ToListAsync()),
                //Vodicii = new SelectList(await vodichiQuery.ToListAsync()),
                Vodichi = await vodichi.ToListAsync(),
                Turi = await turi.ToListAsync()
            };
            return View(viewmodel);

            //var hikingApp_RSWEBContext = _context.Tura.Include(t => t.FirstVodich).Include(t => t.SecoundVodich);
            //return View(await hikingApp_RSWEBContext.ToListAsync());
        }

        // GET: Turas/Details/5

        [Authorize(Roles = "Admin,Planinar,Vodich")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tura = await _context.Tura
                .Include(t => t.FirstVodich)
                .Include(t => t.SecoundVodich)
                .Include(m => m.Planinari).ThenInclude(m => m.Planinar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tura == null)
            {
                return NotFound();
            }

            return View(tura);
        }

        // GET: Turas/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["FirstVodichId"] = new SelectList(_context.Vodich, "Id", "FullName");
            ViewData["SecoundVodichId"] = new SelectList(_context.Vodich, "Id", "FullName");
            return View();
        }

        // POST: Turas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mesto,DatumPocetok,DatumKraj,Tezina,Vremetraenje,FirstVodichId,SecoundVodichId")] Tura tura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstVodichId"] = new SelectList(_context.Vodich, "Id", "FullName", tura.FirstVodichId);
            ViewData["SecoundVodichId"] = new SelectList(_context.Vodich, "Id", "FullName", tura.SecoundVodichId);
            return View(tura);
        }

        // GET: Turas/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tura = _context.Tura.Where(m => m.Id == id).Include(m => m.Planinari).First();

            if (tura == null)
            {
                return NotFound();
            }

            var planinari = _context.Planinar.AsEnumerable();
            planinari = planinari.OrderBy(s => s.FullName);
            EditTuraViewModel viewmodel = new EditTuraViewModel
            {
                Tura = tura,
                ListaPlaninari = new MultiSelectList(planinari, "Id", "FullName"),
                SelectedPlaninari = tura.Planinari.Select(sa => sa.PlaninarId)
            };

            ViewData["FirstVodichId"] = new SelectList(_context.Vodich, "Id", "FullName", tura.FirstVodichId);
            ViewData["SecoundVodichId"] = new SelectList(_context.Vodich, "Id", "FullName", tura.SecoundVodichId);
            return View(viewmodel);
        }

        // POST: Turas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, EditTuraViewModel viewModel)
        {
            if (id != viewModel.Tura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Tura);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> listaPlaninari = viewModel.SelectedPlaninari;
                    IQueryable<Rezervacii> toBeRemoved = _context.Rezervacii.Where(s => !listaPlaninari.Contains(s.PlaninarId) && s.TuraId == id);
                    _context.Rezervacii.RemoveRange(toBeRemoved);
                    IEnumerable<int> existStudents = _context.Rezervacii.Where(s => listaPlaninari.Contains(s.PlaninarId) && s.TuraId == id).Select(s => s.PlaninarId);
                    IEnumerable<int> newStudents = listaPlaninari.Where(s => !existStudents.Contains(s));
                    foreach (int studentId in newStudents)
                        _context.Rezervacii.Add(new Rezervacii { PlaninarId = studentId, TuraId = id });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TuraExists(viewModel.Tura.Id))
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
            ViewData["FirstVodichId"] = new SelectList(_context.Vodich, "Id", "FullName", viewModel.Tura.FirstVodichId);
            ViewData["SecoundVodichId"] = new SelectList(_context.Vodich, "Id", "FullName", viewModel.Tura.SecoundVodichId);
            return View(viewModel);
        }

        // GET: Turas/Delete/5

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tura = await _context.Tura
                .Include(t => t.FirstVodich)
                .Include(t => t.SecoundVodich)
                .Include(t => t.Planinari).ThenInclude(t => t.Planinar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tura == null)
            {
                return NotFound();
            }

            return View(tura);
        }


        // POST: Turas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tura = await _context.Tura.FindAsync(id);
            _context.Tura.Remove(tura);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool TuraExists(int id)
        {
            return _context.Tura.Any(e => e.Id == id);
        }

    }
}
