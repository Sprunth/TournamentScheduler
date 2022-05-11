using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sprunth.TournamentScheduler.Schedulers.Base;

[assembly: InternalsVisibleTo("Sprunth.TournamentScheduler.Schedulers.Tests")]
namespace Sprunth.TournamentScheduler.Schedulers
{
    public class SingleEliminationScheduler<T> : EliminationScheduler<T> where T : IComparable
    {
        public HashSet<T> competitors = new HashSet<T>();
        public List<Matchup<T>> currentMatchups;

        public int CurrentIteration { get; set; }

        public override void LoadCompetitors(IEnumerable<T> competitors)
        {
            this.competitors = new HashSet<T>(competitors);

            if ((this.competitors.Count & (this.competitors.Count - 1)) != 0)
            {
                throw new Exception("Competitor count needs to be power of 2");
                // todo: is there a bye in single elimination?
            }
        }

        /// <summary>
        /// Calculate the next set of matchups. This will increment the CurrentIteration
        /// If not all results have been submitted, an exception will be thrown.
        /// If there was only 1 matchup left, an exception will be thrown (no more matchups).
        /// </summary>
        public override void CalculateNextMatchups()
        {
            if (!ReadyForNextIteration())
                throw new Exception("Not all matchup results submitted!");
            var stillCompeting = new List<T>();
            if (currentMatchups != null)
            {
                if (currentMatchups.Count == 1)
                    throw new Exception("Final matchup played already. No more matchups!");
                // In single elimination, every loser is out. We can update the competitors set with only the winners
                stillCompeting = new List<T>(currentMatchups.Select(matchup => matchup.Winner));
                CurrentIteration += 1;
            }
            else
            {
                stillCompeting = new List<T>(competitors);
            }

            // In Single Elimination, the winner of match 1 plays the winner of match 2. Same with matches 3 and 4, and so on.
            currentMatchups = new List<Matchup<T>>(); // clear
            for (var i = 0; i < stillCompeting.Count; i += 2)
            {
                currentMatchups.Add(new Matchup<T>(stillCompeting[i], stillCompeting[i + 1]));
            }
            competitors = new HashSet<T>(stillCompeting);
        }

        public override List<Matchup<T>> GetCurrentMatchups()
        {
            if (currentMatchups == null)
                throw new Exception("Need to calculate matchups once to get current matchups");
            return currentMatchups;
        }

        private bool ReadyForNextIteration()
        {
            return currentMatchups == null || currentMatchups.All(matchup => matchup.WinnerSet);
        }
    }
}
