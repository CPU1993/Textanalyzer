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
        int TextID { get; set; }
        int TotalScore { get; set; }
        List<Section> Sections { get; set; }
    }
}
