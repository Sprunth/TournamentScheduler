using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprunth.TournamentScheduler.Schedulers;

namespace TournamentSchedulerSamples
{
    static class RoundRobinSchedulerSample
    {
        public static void RunSample()
        {
            var competitors = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                competitors.Add(i);
            }

            var scheduler = new RoundRobinScheduler<int>();
            scheduler.LoadCompetitors(competitors);
            scheduler.CalculateNextMatchups();
            var matchups = scheduler.Matchups;

            var matchupsBrokenDown = new Dictionary<int, List<int>>();
            competitors.ForEach(i => matchupsBrokenDown.Add(i, new List<int>()));

            foreach (var matchup in matchups)
            {
                matchupsBrokenDown[matchup.Challenger1].Add(matchup.Challenger2);
                matchupsBrokenDown[matchup.Challenger2].Add(matchup.Challenger1);
            }

            Console.WriteLine("Matchups by Competitor, in order");
            foreach (var keyValuePair in matchupsBrokenDown)
            {
                Console.WriteLine(keyValuePair.Key + ": ");
                keyValuePair.Value.ForEach(i => Console.Write($"{i} "));
                Console.WriteLine();
            }
        }
    }
}
