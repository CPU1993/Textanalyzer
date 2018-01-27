using System;
using System.Collections.Generic;
using System.Text;

namespace Textanalyzer.Data.Entities
{
    public class Sentence
    {
        public int SentenceId { get; set; }
        public Sentence Previous { get; set; }
        public Sentence Next { get; set; }
        public List<Word> Value { get; set; }
    }
}
