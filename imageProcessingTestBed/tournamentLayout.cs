using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;


namespace imageProcessingTestBed
{
    class tournamentLayout
    {
        public List<Match[]> UpperBracketLayout { get; }
        public List<Match[]> LowerBracketLayout { get; }


        public int Rounds { get; }
        public bool IncludeLosersBracket { get; }
        public List<int> MapsToWin { get; set; }

        tournamentLayout(int nRounds, bool losersBracket = false)
        {
            Rounds = nRounds;
            IncludeLosersBracket = losersBracket;
            UpperBracket = InitRounds(nRounds);
            

            //Earlier rounds to Bo3
            for( int iMapsToWin = 0; iMapsToWin < nRounds - 1; iMapsToWin++)
            {
                MapsToWin.Add(2);
            }
            //Set the final round to Bo5
            MapsToWin.Add(3);
        }

        private List<Match[]> InitRounds(int nRounds)
        {

            List<Match[]> tempLayout = new List<Match[]>(nRounds);


            //Generate a right to left bracket so finals are on the left.
            int nMatches = 1; //For keeping track of the 2 powers without math.pow
            for(int iRound = 0; iRound < nRounds; iRound++)
            {
                for(int iMatch = 0; iMatch < nMatches; iMatch++)
                {
                    tempLayout[iRound][iMatch] = new Match();

                    //Link to previous round!
                    if(iRound > 0)
                    {
                        //Integer division should ensure they link up correctly.
                        tempLayout[iRound][iMatch].WinPathMatch = tempLayout[iRound-1][iMatch/2];
                    }
                }
                nMatches *= 2;
            }

            return tempLayout;
        }

        private List<Match[]> AddLosersBracket()
        {
            if(Rounds <= 1)
            {
                throw new System.IndexOutOfRangeException();
            }
            List<Match[]> tempLayout = InitRounds(Rounds - 1);

            int nMatches = 2; //For keeping track of the 2 powers without math.pow
            for (int iRound = 1; iRound < Rounds - 1; iRound++)
            {
                for (int iMatch = 0; iMatch < nMatches; iMatch++)
                {
                    //Create links to upper bracket
                    if (iRound > 0)
                    {
                        //Integer division should ensure they link up correctly.
                        UpperBracketLayout[iRound][iMatch].LosePathMatch = tempLayout[iRound - 1][iMatch / 2];
                    }
                }
                nMatches *= 2;
            }

            return tempLayout;
    }
}
