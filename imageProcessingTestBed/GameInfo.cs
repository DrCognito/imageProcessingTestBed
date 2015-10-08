using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageProcessingTestBed
{
    class GameInfo
    {
        //ROI Info
        private RegionOfInterest team1;
        private RegionOfInterest team2;
        private RegionOfInterest score1;
        private RegionOfInterest score2;

        //Temporary time info for short/mid/long games, maybe this could be better?
        //Cummulative gaussian maybe?
        //Time in seconds
        private int shortGame = 0;
        private int midGame = 0;
        private int longGame = 0;

        //Character classes?
    }
}
