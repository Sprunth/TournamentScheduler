using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprunth.TournamentScheduler.Schedulers;

namespace TournamentSchedulerSamples
{
    public static class SingleEliminationSchedulerSample
    {
        public static void RunSample()
        {
            var competitors = new List<int>();
            for (var i = 0; i < 16; i++)
            {
                competitors.Add(i);
            }

            var random = new Random();
            var scheduler = new SingleEliminationScheduler<int>();
            scheduler.LoadCompetitors(competitors);
            scheduler.CalculateNextMatchups();

            var currentMatchups = scheduler.GetCurrentMatchups();
            Console.WriteLine($"Starting with {currentMatchups.Count} Matchups");
            while (true)
            {
                Console.WriteLine($"-- Round {scheduler.CurrentIteration} --");
                for (var i =0; i<currentMatchups.Count; i++)
                {
                    var matchup = currentMatchups[i];
                    matchup.Winner = random.NextDouble() > 0.5 ? matchup.Challenger1 : matchup.Challenger2;
                    Console.WriteLine($"[{matchup.Winner} beat {matchup.Loser}]");
                }
                if (currentMatchups.Count == 1)
                    break;
                scheduler.CalculateNextMatchups();
                currentMatchups = scheduler.GetCurrentMatchups();
            }

            Console.WriteLine($"The winner is {currentMatchups[0].Winner}");
        }
    }
}
