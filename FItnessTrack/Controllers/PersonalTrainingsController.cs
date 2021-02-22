using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FItnessTrack.Data;
using FItnessTrack.Models;

namespace FItnessTrack.Controllers
{
    public class PersonalTrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonalTrainingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PersonalTrainings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Trainings.Include(p => p.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PersonalTrainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalTraining = await _context.Trainings
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.TrainingId == id);
            if (personalTraining == null)
            {
                return NotFound();
            }

            return View(personalTraining);
        }

        // GET: PersonalTrainings/Create
        public IActionResult Create()
        {
            ViewData["TrainingId"] = new SelectList(_context.Services, "ServiceId", "ServiceName");
            return View();
        }

        // POST: PersonalTrainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingId,Desciption,Charge,ServiceId")] PersonalTraining personalTraining)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personalTraining);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", personalTraining.TrainingId);
            return View(personalTraining);
        }

        // GET: PersonalTrainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalTraining = await _context.Trainings.FindAsync(id);
            if (personalTraining == null)
            {
                return NotFound();
            }
            ViewData["TrainingId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", personalTraining.TrainingId);
            return View(personalTraining);
        }

        // POST: PersonalTrainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingId,Desciption,Charge,ServiceId")] PersonalTraining personalTraining)
        {
            if (id != personalTraining.TrainingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalTraining);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalTrainingExists(personalTraining.TrainingId))
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
            ViewData["TrainingId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", personalTraining.TrainingId);
            return View(personalTraining);
        }

        // GET: PersonalTrainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalTraining = await _context.Trainings
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.TrainingId == id);
            if (personalTraining == null)
            {
                return NotFound();
            }

            return View(personalTraining);
        }

        // POST: PersonalTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personalTraining = await _context.Trainings.FindAsync(id);
            _context.Trainings.Remove(personalTraining);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalTrainingExists(int id)
        {
            return _context.Trainings.Any(e => e.TrainingId == id);
        }
    }
}
