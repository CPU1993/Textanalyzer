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

                CreateWorker(text);

                return RedirectToAction(nameof(Index));
            }
            return View(text);
        }

        private void CreateWorker(Text text)
        {
            text.UserName = HttpContext.User.Identity.Name;
            _context.Add(text);
            _context.SaveChanges();

            int textId = _context.Texts.FirstOrDefault(x => x.Value == text.Value).TextID;

            SentenceWorker(text.Value, textId);
        }

        private bool SentenceWorker(string text, int textId)
        {
            bool result = false;
            int i = 0;
            int id = 0;
            string[] sentences = text.Split('.');
            Sentence sentence = null;

            foreach (string s in sentences)
            {
                string trimmedS = s.Trim();
                if (trimmedS.Equals(""))
                {
                    continue;
                }

                sentence = new Sentence() { Value = trimmedS, TextID = textId };
                if (i == 0)
                {
                    _context.Sentences.Add(sentence);
                    _context.SaveChanges();
                    id = sentence.SentenceID;
                    WordWorker(trimmedS, id);
                    i++;
                    result = true;
                }
                else
                {
                    sentence.PreviousID = id;
                    _context.Sentences.Add(sentence);
                    _context.SaveChanges();
                    int currentId = sentence.SentenceID;

                    WordWorker(trimmedS, currentId);

                    Sentence previous = _context.Sentences.Find(id);
                    previous.NextID = currentId;
                    _context.Sentences.Update(previous);
                    _context.SaveChanges();

                    id = currentId;
                    i++;
                    result = true;
                }
            }

            return result;
        }

        private bool WordWorker(string sentence, int sentenceId)
        {
            bool result = false;
            string[] split = sentence.Split(' ');
            List<Word> words = new List<Word>();

            foreach (string s in split)
            {
                words.Add(new Word() { Value = s, SentenceID = sentenceId });
            }

            _context.Words.AddRange(words);
            _context.SaveChanges();
            result = true;

            return result;
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
            var oldText = await _context.Texts.SingleOrDefaultAsync(m => m.TextID == id);

            if (oldText.UserName != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }

            if (id != oldText.TextID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DeleteWorker(id);
                    CreateWorker(text);
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

            DeleteWorker(id);

            return RedirectToAction(nameof(Index));
        }

        private void DeleteWorker(int id)
        {
            var text = _context.Texts.SingleOrDefault(m => m.TextID == id);

            List<Sentence> sentences = _context.Sentences.Where(x => x.TextID == text.TextID).ToList();

            sentences.Reverse();

            foreach (Sentence s in sentences)
            {
                List<Word> words = _context.Words.Where(x => x.SentenceID == s.SentenceID).ToList();

                foreach (Word w in words)
                {
                    _context.Words.Remove(w);
                    _context.SaveChanges();
                }
                _context.Sentences.Remove(s);
                _context.SaveChanges();
            }

            _context.Texts.Remove(text);
            _context.SaveChanges();
        }

        private bool TextExists(int id)
        {
            return _context.Texts.Any(e => e.TextID == id);
        }
    }
}
