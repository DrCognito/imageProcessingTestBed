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
        public List<int> LosersMapsToWin { get; set; }

        tournamentLayout(int nRounds, bool losersBracket = false)
        {
            Rounds = nRounds;
            IncludeLosersBracket = losersBracket;
            UpperBracketLayout = InitRounds(nRounds);

            //Set the final round to Bo5
            MapsToWin.Add(3);
            //Earlier rounds to Bo3
            for (int iMapsToWin = 1; iMapsToWin < nRounds - 1; iMapsToWin++)
            {
                MapsToWin.Add(2);
            }

            if (IncludeLosersBracket)
            {
                InitLosersBracket();
                LosersMapsToWin.Add(3);
                for (int iMapsToWin = 1; iMapsToWin < nRounds - 2; iMapsToWin++)
                {
                    LosersMapsToWin.Add(2);
                }

            }


        }

        tournamentLayout(tournamentLayout inTourney)
        {
            this.IncludeLosersBracket = inTourney.IncludeLosersBracket;
            this.LosersMapsToWin = inTourney.LosersMapsToWin;
            this.LowerBracketLayout = inTourney.LowerBracketLayout;
            this.MapsToWin = inTourney.MapsToWin;
            this.Rounds = inTourney.Rounds;
            this.UpperBracketLayout = inTourney.UpperBracketLayout;
        }

        public tournamentLayout Clone()
        {
            tournamentLayout newTournament = new tournamentLayout(Rounds, IncludeLosersBracket);
            
            for (int iRound = 0; iRound < Rounds; iRound++)
            {
                //Upper bracket clone
                for (int iMatch = 0; iMatch < UpperBracketLayout.Count; iMatch++)
                {
                    newTournament.UpperBracketLayout[iRound][iMatch] = UpperBracketLayout[iRound][iMatch];
                }
                newTournament.MapsToWin[iRound] = MapsToWin[iRound];

                //Lower bracket clone
                if (IncludeLosersBracket && iRound < Rounds - 1)
                {
                    for (int iMatch = 0; iMatch < LowerBracketLayout.Count; iMatch++)
                    {
                        newTournament.LowerBracketLayout[iRound][iMatch] = LowerBracketLayout[iRound][iMatch];
                    }
                    newTournament.LosersMapsToWin[iRound] = LosersMapsToWin[iRound];
                }
            }

            return newTournament;
        }

        private List<Match[]> InitRounds(int nRounds)
        {

            List<Match[]> tempLayout = new List<Match[]>(nRounds);


            //Generate a right to left bracket so finals are on the left.
            int nMatches = 1; //For keeping track of the 2 powers without math.pow
            for (int iRound = 0; iRound < nRounds; iRound++)
            {
                for (int iMatch = 0; iMatch < nMatches; iMatch++)
                {
                    tempLayout[iRound][iMatch] = new Match();

                    //Link to previous round!
                    if (iRound > 0)
                    {
                        //Integer division should ensure they link up correctly.
                        tempLayout[iRound][iMatch].WinPathMatch = tempLayout[iRound - 1][iMatch / 2];
                    }
                }
                nMatches *= 2;
            }

            return tempLayout;
        }

        private List<Match[]> InitLosersBracket()
        {
            if (Rounds <= 1)
            {
                throw new IndexOutOfRangeException();
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
}
