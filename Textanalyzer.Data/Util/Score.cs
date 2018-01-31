using System;
using System.Collections.Generic;
using System.Text;

namespace Textanalyzer.Data.Util
{
    public class Score
    {
        public Score(int textID, int totalScore, List<Section> sections)
        {
            this.TextID = textID;
            this.TotalScore = totalScore;
            this.Sections = sections;
        }
        public int TextID { get; set; }
        public int TotalScore { get; set; }
        public List<Section> Sections { get; set; }
    }
}
