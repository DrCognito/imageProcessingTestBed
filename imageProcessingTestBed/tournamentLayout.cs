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
        private AdjacencyGraph<Match, Edge<Match>> tLayout = new AdjacencyGraph<Match, Edge<Match>>();

        private List<Match[]> tLayout =;


        public int Rounds { get; }
        public bool IncludeLosersBracket { get; }
        public List<int> MapsToWin { get; set; }

        tournamentLayout(int nRounds, bool losersBracket = false)
        {
            Rounds = nRounds;
            IncludeLosersBracket = losersBracket;
            InitRounds(nRounds);
            

            //Earlier rounds to Bo3
            for( int iMapsToWin = 0; iMapsToWin < nRounds - 1; iMapsToWin++)
            {
                MapsToWin.Add(2);
            }
            //Set the final round to Bo5
            MapsToWin.Add(3);
        }

        private void InitRounds(int nRounds)
        {

            int nMatches = Convert.ToInt32((Math.Pow(2.0, nRounds) - 1.0));
            Match[] matchSet = new Match[nMatches];

            
            //Theres always a final round.
            tLayout.AddVertex(matchSet[0]);
            int innerCursor = 1;
            int outerCursor = 2;//Will auto fail for 1 round case.
            while(outerCursor < nMatches)
            {
                int upperInner = 2 * innerCursor;
                while(innerCursor < upperInner)
                {
                    tLayout.AddVerticesAndEdge(new Edge<Match>(matchSet[innerCursor], matchSet[outerCursor]));
                    outerCursor++;
                    tLayout.AddVerticesAndEdge(new Edge<Match>(matchSet[innerCursor], matchSet[outerCursor]));
                    outerCursor++;
                    innerCursor++;
                }

            }
        }

        private void AddLosersBracket()
        {
            int nLosersMatches = Convert.ToInt32((Math.Pow(2.0, Rounds-1) - 1.0));
            Match[] matchSet = new Match[nLosersMatches];

            tLayout.
        }
    }
}
