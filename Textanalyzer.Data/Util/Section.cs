using System;
using System.Collections.Generic;
using System.Text;

namespace Textanalyzer.Data.Util
{
    public class Section
    {
        public Section(int sentenceID, int score, List<string> summary)
        {
            this.SentenceID = sentenceID;
            this.Score = score;
            this.Summary = summary;
        }

        public int SentenceID { get; set; }
        public int Score { get; set; }
        public List<string> Summary { get; set; }
    }
}
