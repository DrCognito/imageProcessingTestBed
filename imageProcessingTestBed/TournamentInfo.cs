using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace imageProcessingTestBed
{
    class TournamentInfo
    {
        private List<string[]> matchUps = new List<string[]>(); //Who can play who, order is not important
        //Graph for the basic layout
        public tournamentLayout tournamentLayout { get; set; }

        //private var tournamentResults = new AdjacencyGraph<int, TaggedEdge<int, string>>(); //Graph for the results so far, including times?

        //Region for the information we want
        public GameInfo gameInfo { get; set; }

        public string fontInfo { get; set; } //Some sort of qualititative info for various fonts used by different people/overlays.
        public bool multiLineTeam { get; set; } //Multiple lines for teams? Apparently.       
    }
}
