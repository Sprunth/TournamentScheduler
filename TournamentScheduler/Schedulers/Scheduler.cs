using System;
using System.Collections.Generic;

namespace Sprunth.TournamentScheduler.Schedulers
{
    public abstract class Scheduler<T>
    {
        public IEnumerable<Tuple<T, T>> Matchups { get; protected set; }
        public IList<IList<Tuple<T, T>>> MatchupsByDay { get; protected set; }

        public abstract void LoadCompetitors(IEnumerable<T> competitors);
        public abstract void CalculateMatchups();
    }
}