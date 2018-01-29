using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Textanalyzer.Data.Entities
{
    public class Word
    {
        public int WordID { get; set; }
        [ForeignKey("Sentence")]
        public int SentenceID { get; set; }
        public Sentence Sentence { get; set; }
        public string Value { get; set; }
    }
}
