using System;
using System.Collections.Generic;
using System.Text;

namespace Textanalyzer.Data.Entities
{
    public class Text
    {
        public int TextID { get; set; }
        public string Value { get; set; }
        public string UserName { get; set; }
        public ICollection<Sentence> Sentences { get; set; }
    }
}
