using System;
using System.Collections.Generic;

namespace Sprunth.TournamentScheduler.Schedulers
{
    public abstract class Scheduler<T>
    {
        public abstract void LoadCompetitors(IEnumerable<T> competitors);
        public abstract IEnumerable<Tuple<T, T>> CalculateMatchups();
    }
}