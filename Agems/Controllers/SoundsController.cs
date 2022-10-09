using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agems;
using Agems.Models;

namespace Agems.Controllers
{
    public class SoundsController : Controller
    {
        private readonly AgemsSoundsContext _context;
        string soundsFolder = "wwwroot/sounds/";

        public SoundsController(AgemsSoundsContext context)
        {
            _context = context;
        }

        // GET: Sounds
        public async Task<IActionResult> Index(int id)
        {
            var agemsSoundsContext = _context.Sounds.Where(x => x.CategoryId == id).Include(s => s.Category);
            ViewBag.CategoryId = id;
            ViewBag.CategoryName = _context.Categories.Where(x => x.Id == id).First().Name;

            return View(await agemsSoundsContext.ToListAsync());
        }

        // GET: Sounds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sounds == null)
            {
                return NotFound();
            }

            var sound = await _context.Sounds
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sound == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Comments", new { Id = id });
        }

        // GET: Sounds/Create
        public IActionResult Create(int Id)
        {
            ViewBag.CategoryId = Id;
            return View();
        }

        // POST: Sounds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Author,CategoryId,Name,SoundPath, SoundFile, About")] Sound sound)
        {
            if (ModelState.IsValid)
            {
                string FileName = Path.GetFileNameWithoutExtension(sound.SoundFile.FileName) + Path.GetExtension(sound.SoundFile.FileName);
                sound.SoundPath = FileName;
                writeFileAsync(sound.SoundFile, FileName);
                _context.Add(sound);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Id = sound.CategoryId} );
            }
            ViewBag.CategoryId = sound.CategoryId;
            return View(sound);
        }

        // GET: Sounds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sounds == null)
            {
                return NotFound();
            }

            var sound = await _context.Sounds.FindAsync(id);
            if (sound == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = sound.CategoryId;
            ViewBag.Categories = _context.Categories;
            return View(sound);
        }

        async void writeFileAsync(IFormFile soundFile, string name)
        {
                using (var stream = System.IO.File.Create(soundsFolder + name))
                {
                    await soundFile.CopyToAsync(stream);
                }
        }

        // POST: Sounds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Author, CategoryId, Name, SoundPath, SoundFile, About")] Sound sound)
        {
            if (id != sound.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (sound.SoundFile != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(sound.SoundFile.FileName) + Path.GetExtension(sound.SoundFile.FileName);
                        sound.SoundPath = FileName;
                        writeFileAsync(sound.SoundFile, FileName);
                    }
                    _context.Update(sound);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoundExists(sound.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return RedirectToAction(nameof(Index), new { Id = sound.CategoryId });
            }
            ViewBag.CategoryId = sound.CategoryId;
            return View(sound);
        }

        // GET: Sounds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sounds == null)
            {
                return NotFound();
            }

            var sound = await _context.Sounds
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sound == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = sound.CategoryId;
            return View(sound);
        }

        // POST: Sounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sounds == null)
            {
                return Problem("Entity set 'AgemsSoundsContext.Sounds'  is null.");
            }
            var sound = await _context.Sounds.FindAsync(id);
            if (sound != null)
            {
                _context.Sounds.Remove(sound);
                System.IO.File.Delete(soundsFolder + sound.SoundPath);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { Id = sound.CategoryId });
        }

        private bool SoundExists(int id)
        {
          return _context.Sounds.Any(e => e.Id == id);
        }
    }
}
