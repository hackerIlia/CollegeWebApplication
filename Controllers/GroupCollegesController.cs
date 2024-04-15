using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeWebApplication.Models;

namespace CollegeWebApplication.Controllers
{
    public class GroupCollegesController : Controller
    {
        private readonly collegeContext _context;

        public GroupCollegesController(collegeContext context)
        {
            _context = context;
        }

        // GET: GroupColleges
        public async Task<IActionResult> Index()
        {
            var collegeContext = _context.GroupColleges.Include(g => g.IdTeahcerNavigation);
            return View(await collegeContext.ToListAsync());
        }

        // GET: GroupColleges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupCollege = await _context.GroupColleges
                .Include(g => g.IdTeahcerNavigation)
                .FirstOrDefaultAsync(m => m.IdGroup == id);
            if (groupCollege == null)
            {
                return NotFound();
            }

            return View(groupCollege);
        }

        // GET: GroupColleges/Create
        public IActionResult Create()
        {
            ViewData["IdTeahcer"] = new SelectList(_context.Teachers.Select(t => new { t.IdTeacher, FullName = t.NameTeahcer + " " + t.SurnameTeacher }), "IdTeacher", "FullName");
            return View();
        }

        // POST: GroupColleges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGroup,NameGroup,IdTeahcer")] GroupCollege groupCollege)
        {
            if (ModelState.IsValid)
            {
                var teahcer = await _context.Teachers
                    .FirstOrDefaultAsync(t => t.IdTeacher == groupCollege.IdTeahcer && t.GroupColleges.Any());

                if(teahcer != null)
                {
                    TempData["ErrorMessage"] = $"Teacher {teahcer.SurnameTeacher} {teahcer.NameTeahcer} is already assigned to another group.";
                    ViewData["IdTeahcer"] = new SelectList(_context.Teachers.Select(t => new { t.IdTeacher, FullName = t.NameTeahcer + " " + t.SurnameTeacher }), "IdTeacher", "FullName");
                    return View(groupCollege);
                }

                _context.Add(groupCollege);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTeahcer"] = new SelectList(_context.Teachers.Select(t => new { t.IdTeacher, FullName = t.NameTeahcer + " " + t.SurnameTeacher }), "IdTeacher", "FullName");
            return View(groupCollege);
        }

        // GET: GroupColleges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupCollege = await _context.GroupColleges.FindAsync(id);
            if (groupCollege == null)
            {
                return NotFound();
            }
            ViewData["IdTeahcer"] = new SelectList(_context.Teachers.Select(t => new { t.IdTeacher, FullName = t.NameTeahcer + " " + t.SurnameTeacher }), "IdTeacher", "FullName");
            return View(groupCollege);
        }

        // POST: GroupColleges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGroup,NameGroup,IdTeahcer")] GroupCollege groupCollege)
        {
            if (id != groupCollege.IdGroup)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var teahcer = await _context.Teachers
                    .FirstOrDefaultAsync(t => t.IdTeacher == groupCollege.IdTeahcer && t.GroupColleges.Any(g => g.IdGroup != id));

                if (teahcer != null)
                {
                    TempData["ErrorMessage"] = $"Teacher {teahcer.SurnameTeacher} {teahcer.NameTeahcer} is already assigned to another group.";
                    ViewData["IdTeahcer"] = new SelectList(_context.Teachers.Select(t => new { t.IdTeacher, FullName = t.NameTeahcer + " " + t.SurnameTeacher }), "IdTeacher", "FullName");
                    return View(groupCollege);
                }

                try
                {
                    _context.Update(groupCollege);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupCollegeExists(groupCollege.IdGroup))
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
            ViewData["IdTeahcer"] = new SelectList(_context.Teachers.Select(t => new { t.IdTeacher, FullName = t.NameTeahcer + " " + t.SurnameTeacher }), "IdTeacher", "FullName");
            return View(groupCollege);
        }

        // GET: GroupColleges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupCollege = await _context.GroupColleges
                .Include(g => g.IdTeahcerNavigation)
                .FirstOrDefaultAsync(m => m.IdGroup == id);
            if (groupCollege == null)
            {
                return NotFound();
            }

            return View(groupCollege);
        }

        // POST: GroupColleges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupCollege = await _context.GroupColleges
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.IdGroup == id);

            if (groupCollege == null)
            {
                return NotFound();
            }

            if (groupCollege.Students.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete group because there is at least one student registred in it.";
                return RedirectToAction(nameof(Index));
            }

            _context.GroupColleges.Remove(groupCollege);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool GroupCollegeExists(int id)
        {
            return _context.GroupColleges.Any(e => e.IdGroup == id);
        }
    }
}
