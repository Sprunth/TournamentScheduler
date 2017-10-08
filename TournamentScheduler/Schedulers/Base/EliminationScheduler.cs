using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprunth.TournamentScheduler.Schedulers.Base
{
    public abstract class EliminationScheduler<T> : Scheduler<T> where T : IComparable
    {
        int CurrentIteration { get; set; }
        public abstract List<Matchup<T>> GetCurrentMatchups();
    }
}
