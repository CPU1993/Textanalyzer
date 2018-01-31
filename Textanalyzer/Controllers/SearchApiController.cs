using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimumEditDistance;
using Newtonsoft.Json;
using Textanalyzer.Data.Data;
using Textanalyzer.Data.Entities;
using Textanalyzer.Data.Util;

namespace Textanalyzer.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/SearchApi")]
    public class SearchApiController : Controller
    {
        private readonly ILogger<SearchApiController> _log;
        private readonly ApplicationDbContext _context;

        public SearchApiController(ApplicationDbContext context, ILogger<SearchApiController> log)
        {
            _context = context;
            _log = log;
        }

        // GET: api/SearchApi/5
        [HttpGet("{text}")]
        public async Task<IActionResult> GetText(string text)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string[] words = text.Split(' ');
            string[] maxWords = new string[3];

            if (words.Length > 3)
            {
                maxWords[0] = words[0];
                maxWords[1] = words[1];
                maxWords[2] = words[2];
            }
            else
            {
                for (int i = 0; i < words.Length; i++)
                {
                    maxWords[i] = words[i];
                }
            }

            List<Text> texts = new List<Text>();

            foreach (string s in maxWords)
            {
                if (s == null)
                {
                    continue;
                }
                texts.AddRange(FindTextsByWord(s));
            }

            if (texts.Count == 0)
            {
                return NotFound();
            }

            List<Score> scores = RateTexts(texts, maxWords);
            _log.LogInformation("Found scores sent!");
            return new ObjectResult(scores);
        }

        private List<Score> RateTexts(List<Text> texts, string[] words)
        {
            List<Score> scores = new List<Score>();
            foreach (Text t in texts)
            {
                Score score = null;
                List<Section> sections = new List<Section>();
                int totalScore = 0;

                List<Sentence> sentences = _context.Sentences.Where(x => x.TextID == t.TextID).ToList();
                foreach (Sentence s in sentences)
                {
                    int findingMain = 0;
                    int findingLevenshteinMain = 0;
                    int findingPrevious = 0;
                    int findingLevenshteinPrevious = 0;
                    int findingNext = 0;
                    int findingLevenshteinNext = 0;

                    foreach (string word in words)
                    {
                        if (word == null)
                        {
                            continue;
                        }

                        findingMain += Regex.Matches(s.Value.ToLower(), word.ToLower()).Count;
                        findingLevenshteinMain += FindLevenstheinOccurences(s, word);

                        if (s.PreviousID != null)
                        {
                            Sentence previous = _context.Sentences.Find(s.PreviousID);
                            findingPrevious += Regex.Matches(previous.Value.ToLower(), word.ToLower()).Count;
                            findingLevenshteinPrevious += FindLevenstheinOccurences(previous, word);
                        }

                        if (s.NextID != null)
                        {
                            Sentence next = _context.Sentences.Find(s.NextID);
                            findingNext += Regex.Matches(next.Value.ToLower(), word.ToLower()).Count;
                            findingLevenshteinNext += FindLevenstheinOccurences(next, word);
                        }
                    }

                    int mainScore = findingMain * 3;
                    int neighbourScore = findingPrevious + findingLevenshteinPrevious + findingNext + findingLevenshteinNext;
                    int levenshteinScore = findingLevenshteinMain * 2;

                    int sentenceScore = mainScore + neighbourScore + levenshteinScore;

                    List<string> summary = new List<string>();

                    summary.Add($"{mainScore} Points: Terms found {findingMain}x in main sentence.");
                    summary.Add($"{levenshteinScore} Points: Similar terms found {findingLevenshteinMain}x in main sentence.");
                    summary.Add($"{neighbourScore} Points: Terms found {neighbourScore}x in neighbouring sentences.");

                    sections.Add(new Section(s.SentenceID, sentenceScore, summary));

                    totalScore += sentenceScore;
                }

                score = new Score(t.TextID, totalScore, sections);
                scores.Add(score);

            }

            Score tempScore = null;

            for (int i = 0; i < scores.Count; i++)
            {
                for (int j = 0; j < scores.Count - 1; j++)
                {
                    if (scores[j].TotalScore < scores[j + 1].TotalScore)
                    {
                        tempScore = scores[j + 1];
                        scores[j + 1] = scores[j];
                        scores[j] = tempScore;
                    }
                }
            }

            if (scores.Count > 5)
            {
                scores.RemoveRange(4, scores.Count);
            }

            return scores;
        }

        private int FindLevenstheinOccurences(Sentence s, string word)
        {
            int result = 0;
            List<Word> words = _context.Words.Where(x => x.SentenceID == s.SentenceID).ToList();

            foreach (Word w in words)
            {
                if (Levenshtein.CalculateDistance(w.Value.ToLower(), word.ToLower(), 1) <= 1)
                {
                    result++;
                }
            }

            return result;
        }

        private List<Text> FindTextsByWord(string word)
        {
            List<Text> result = new List<Text>();

            List<Word> possibleWords = _context.Words.Where(x => x.Value.ToLower() == word.ToLower() || Levenshtein.CalculateDistance(x.Value, word, 1) <= 1).ToList();
            List<int> sentenceIDs = new List<int>();

            foreach (Word w in possibleWords)
            {
                if (!sentenceIDs.Contains(w.SentenceID))
                {
                    sentenceIDs.Add(w.SentenceID);
                }
            }

            List<Sentence> possibleSentences = _context.Sentences.Where(x => sentenceIDs.Contains(x.SentenceID)).ToList();
            List<int> textIDs = new List<int>();

            foreach (Sentence s in possibleSentences)
            {
                if (!textIDs.Contains(s.TextID))
                {
                    textIDs.Add(s.TextID);
                }
            }

            result = _context.Texts.Where(x => textIDs.Contains(x.TextID)).ToList();

            return result;
        }

        private bool TextExists(int id)
        {
            return _context.Texts.Any(e => e.TextID == id);
        }
    }
}