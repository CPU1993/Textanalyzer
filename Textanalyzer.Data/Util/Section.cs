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

        int SentenceID { get; set; }
        int Score { get; set; }
        List<string> Summary { get; set; }
    }
}
