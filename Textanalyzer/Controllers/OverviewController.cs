using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Textanalyzer.Data.Data;
using Textanalyzer.Data.Entities;
using Textanalyzer.Data.Util;

namespace Textanalyzer.Web.Controllers
{
    [Authorize]
    public class OverviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OverviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Overview
        public async Task<IActionResult> Index()
        {
            TextHandler th = new TextHandler(_context, HttpContext.User.Identity.Name);

            IList<Text> texts = th.GetCurrentUserTexts();
            return View(texts);
        }

        // GET: Overview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var text = await _context.Texts.SingleOrDefaultAsync(m => m.TextID == id);
            if (text == null)
            {
                return NotFound();
            }

            return View(text);
        }

        // GET: Overview/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Overview/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Value,UserName")] Text text)
        {
            if (ModelState.IsValid)
            {
                text.UserName = HttpContext.User.Identity.Name;
                _context.Add(text);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(text);
        }

        // GET: Overview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var text = await _context.Texts.SingleOrDefaultAsync(m => m.TextID == id);

            if (text.UserName != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }

            if (text == null)
            {
                return NotFound();
            }
            return View(text);
        }

        // POST: Overview/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Value")] Text text)
        {
            if (text.UserName != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }

            if (id != text.TextID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(text);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TextExists(text.TextID))
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
            return View(text);
        }

        // GET: Overview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var text = await _context.Texts.SingleOrDefaultAsync(m => m.TextID == id);

            if (text.UserName != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }

            if (text == null)
            {
                return NotFound();
            }

            return View(text);
        }

        // POST: Overview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var text = await _context.Texts.SingleOrDefaultAsync(m => m.TextID == id);

            if (text.UserName != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }

            _context.Texts.Remove(text);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TextExists(int id)
        {
            return _context.Texts.Any(e => e.TextID == id);
        }
    }
}
