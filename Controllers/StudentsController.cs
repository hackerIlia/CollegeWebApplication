using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeWebApplication.Models;
using PagedList;

namespace CollegeWebApplication.Controllers
{
    public class StudentsController : Controller
    {
        private readonly collegeContext _context;

        public StudentsController(collegeContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string studentGroup,string searchString)
        {
            if(_context.Students == null)
            {
                return Problem("Entity set 'MvcStudentContext.Student is null");
            }

            IQueryable<string> groupsQuery = from s in _context.Students
                                             orderby s.IdGroupNavigation.NameGroup
                                             select s.IdGroupNavigation.NameGroup;
            //var groups = _context.GroupColleges.ToList();
            //var selectList = new SelectList(await groupsQuery.Distinct().ToListAsync(), "IdGroup", "NameGroup");

            var students = from s in _context.Students
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.SurnameStudent.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(studentGroup))
            {
                //Console.WriteLine("Searching for group: " + studentGroup);
                students = students.Where(s => s.IdGroupNavigation.NameGroup == studentGroup);
            }

            var studentGroupVM = new StudentGroupViewModel
            {
                Groups = new SelectList(await groupsQuery.Distinct().ToListAsync()),
                Students = await students
                        .Include(s => s.IdGroupNavigation)
                        .ToListAsync()
            };

            return View(studentGroupVM);
            //var collegeContext = _context.Students.Include(s => s.IdGroupNavigation);
            //return View(await collegeContext.ToListAsync());
        }

        [HttpPost]
        public string Index(string searchString,bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.IdGroupNavigation)
                .FirstOrDefaultAsync(m => m.IdStudent == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["IdGroup"] = new SelectList(_context.GroupColleges, "IdGroup", "NameGroup");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStudent,NameStudent,SurnameStudent,YearOfStudy,IdGroup")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGroup"] = new SelectList(_context.GroupColleges, "IdGroup", "NameGroup");
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["IdGroup"] = new SelectList(_context.GroupColleges, "IdGroup", "NameGroup");
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStudent,NameStudent,SurnameStudent,YearOfStudy,IdGroup")] Student student)
        {
            if (id != student.IdStudent)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.IdStudent))
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
            ViewData["IdGroup"] = new SelectList(_context.GroupColleges, "IdGroup", "NameGroup");
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.IdGroupNavigation)
                .FirstOrDefaultAsync(m => m.IdStudent == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.IdStudent == id);
        }
    }
}
