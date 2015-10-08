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


        public int Rounds { get; }
        public bool IncludeLosersBracket { get; }
        public List<int> MapsToWin { get; set; }

        tournamentLayout()
        {

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
    }
}
