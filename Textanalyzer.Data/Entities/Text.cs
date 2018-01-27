using System;
using System.Collections.Generic;
using System.Text;

namespace Textanalyzer.Data.Entities
{
    public class Text
    {
        public int TextId { get; set; }
        public List<Sentence> Value { get; set; }
    }
}
