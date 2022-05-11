using Sprunth.TournamentScheduler.Schedulers.Base;
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
    public class RoundRobinScheduler<T> : Scheduler<T> where T: IComparable
    {
        // Based on http://stackoverflow.com/a/1293174/495501
        

        public T firstCompetitor;
        public readonly List<T> competitors = new List<T>();
        private int competitionIterations;
        private int halfSize;

        public IEnumerable<Matchup<T>> Matchups { get; set; }
        public IList<IList<Matchup<T>>> MatchupsByIteration { get; set; }

        public override void LoadCompetitors(IEnumerable<T> competitors)
        {
            this.competitors.AddRange(competitors);

            if (this.competitors.Count % 2 != 0)
            {
                throw new Exception("Need even competitor count");
                // todo: support "Bye" with odd competitor count
            }

            competitionIterations = (this.competitors.Count - 1);
            halfSize = this.competitors.Count / 2;


            // Since the first competitor doesn't rotate, we take it out and store it seperately
            // Thought: Can we take out the last one instead? More efficient
            firstCompetitor = this.competitors[0];
            this.competitors.RemoveAt(0);
        }

        public override void CalculateNextMatchups()
        {
            var matchups = new List<Matchup<T>>();
            var matchupsByDay = new List<IList<Matchup<T>>>();

            var rotatingTeamSize = competitors.Count;

            for (var day = 0; day < competitionIterations; day++)
            {
                var teamIdx = day % rotatingTeamSize;

                var firstMatchup = new Matchup<T>(competitors[teamIdx], firstCompetitor);
                matchups.Add(firstMatchup);
                var dayMatchups = new List<Matchup<T>> { firstMatchup };

                for (var idx = 1; idx < halfSize; idx++)
                {
                    var firstTeamIndex = (day + idx) % rotatingTeamSize;
                    var secondTeamIndex = (day + rotatingTeamSize - idx) % rotatingTeamSize;

                    var tup = new Matchup<T>(competitors[firstTeamIndex], competitors[secondTeamIndex]);
                    matchups.Add(tup);
                    dayMatchups.Add(tup);
                }

                matchupsByDay.Add(dayMatchups);
            }

            Matchups = matchups;
            MatchupsByIteration = matchupsByDay;
        }
    }
}