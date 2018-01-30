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
        [ForeignKey("Previous")]
        public int? PreviousID { get; set; }
        public Sentence Previous { get; set; }
        [ForeignKey("Next")]
        public int? NextID { get; set; }
        public Sentence Next { get; set; }
        public string Value { get; set; }
        [ForeignKey("Text")]
        public int TextID { get; set; }
        public Text Text { get; set; }
    }
}
