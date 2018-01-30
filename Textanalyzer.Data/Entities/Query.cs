using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Textanalyzer.Data.Entities
{
    public class Query
    {
        public int QueryID { get; set; }
        [ForeignKey("Text")]
        public int? TextID { get; set; }
        public Text Text { get; set; }
        public int Score { get; set; }
        public string Value { get; set; }

    }
}
