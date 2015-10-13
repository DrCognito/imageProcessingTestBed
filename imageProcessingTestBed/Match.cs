using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageProcessingTestBed
{
    class Match
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public int RoundsToWin { get; set; }

        public int ScoreTeam1 { get; set; }
        public int ScoreTeam2 { get; set; }

        public Match WinPathMatch { get; set; }
        public Match LosePathMatch { get; set; }

        public Match()
        {
            RoundsToWin = 3;
            Team1 = null;
            Team2 = null;
            ScoreTeam1 = ScoreTeam2 = 0;
        }

        public Match(Team t1, Team t2, int nRounds = 3)
        {
            RoundsToWin = nRounds;
            Team1 = t1;
            Team2 = t2;
            ScoreTeam1 = ScoreTeam2 = 0;
        }
    }
}
