using System;
using System.Collections.Generic;

namespace Sprunth.TournamentScheduler.Schedulers.Base
{
    public abstract class Scheduler<T> where T : IComparable
    {
        IEnumerable<Matchup<T>> Matchups { get; set; }
        IList<IList<Matchup<T>>> MatchupsByIteration { get; set; }

        public abstract void LoadCompetitors(IEnumerable<T> competitors);
        public abstract void CalculateNextMatchups();
    }
}