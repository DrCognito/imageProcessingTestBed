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

        private void InitRounds(int nRounds)
        {

            int nMatches = Convert.ToInt32((Math.Pow(2.0, nRounds) - 1.0));
            Match[] matchSet = new Match[nMatches];



            //Theres always a final round.
            tLayout.AddVertex(matchSet[0]);
            int offSet = 1;
            for (int iRound = 1; iRound <= nRounds; iRound++)
            {
                for (int iMatch = 0; iMatch < offSet; iMatch++)
                {
                    tLayout.AddVerticesAndEdge(new Edge<Match>(matchSet[iMatch + offSet - 1], matchSet[iMatch + 2 * offSet - 1]));
                    tLayout.AddVerticesAndEdge(new Edge<Match>(matchSet[iMatch + offSet - 1], matchSet[iMatch + 2 * offSet]));
                }

                offSet *= 2; //Proceed by powers of 2 more simply this way.
            }
        }
    }
}
