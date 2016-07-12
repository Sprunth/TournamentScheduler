using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sprunth.TournamentScheduler.Schedulers
{
    /// <summary>
    /// A scheduler that Conducts a round-robin assignment
    /// Every competitor will be matched with every other competitor once.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RoundRobinScheduler<T> : Scheduler<T>
    {
        // Based on http://stackoverflow.com/a/1293174/495501

        internal T firstCompetitor;
        internal readonly List<T> competitors = new List<T>();
        private int competitionDays;
        private int halfSize;

        public override void LoadCompetitors(IEnumerable<T> competitors)
        {
            this.competitors.AddRange(competitors);

            if (this.competitors.Count%2 != 0)
            {
                throw new Exception("Need even competitor count");
                // todo: support "Bye" with odd competitor count
            }

            competitionDays = (this.competitors.Count - 1);
            halfSize = this.competitors.Count/2;


            // Since the first competitor doesn't rotate, we take it out and store it seperately
            // Thought: Can we take out the last one instead? More efficient
            firstCompetitor = this.competitors[0];
            this.competitors.RemoveAt(0);
        }

        public override IEnumerable<Tuple<T, T>> CalculateMatchups()
        {
            var matchups = new List<Tuple<T, T>>();

            var rotatingTeamSize = competitors.Count;

            for (var day = 0; day < competitionDays; day++)
            {
                var teamIdx = day%rotatingTeamSize;

                matchups.Add(new Tuple<T, T>(competitors[teamIdx], firstCompetitor));

                for (var idx = 1; idx < halfSize; idx++)
                {
                    var firstTeamIndex = (day + idx)%rotatingTeamSize;
                    var secondTeamIndex = (day + rotatingTeamSize - idx)%rotatingTeamSize;
                    matchups.Add(new Tuple<T, T>(competitors[firstTeamIndex], competitors[secondTeamIndex]));
                }
            }

            return matchups;
        }
    }
}