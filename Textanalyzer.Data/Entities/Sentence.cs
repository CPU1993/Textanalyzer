using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Textanalyzer.Data.Entities
{
    public class Sentence
    {
        public int SentenceID { get; set; }
        public int? PreviousID { get; set; }
        public Sentence Previous { get; set; }
        public int? NextID { get; set; }
        public Sentence Next { get; set; }
        public string Value { get; set; }
        public int TextID { get; set; }
        public Text Text { get; set; }
        public ICollection<Word> Words { get; set; }
    }
}
