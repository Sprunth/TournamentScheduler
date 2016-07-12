using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprunth.TournamentScheduler.Schedulers;

namespace TournamentSchedulerSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var competitors = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                competitors.Add(i);
            }

            var scheduler = new RoundRobinScheduler<int>();
            scheduler.LoadCompetitors(competitors);
            var matchups = scheduler.CalculateMatchups();

            var matchupsBrokenDown = new Dictionary<int, List<int>>();
            competitors.ForEach(i => matchupsBrokenDown.Add(i, new List<int>()));

            foreach (var matchup in matchups)
            {
                matchupsBrokenDown[matchup.Item1].Add(matchup.Item2);
                matchupsBrokenDown[matchup.Item2].Add(matchup.Item1);
            }

            foreach (var keyValuePair in matchupsBrokenDown)
            {
                Console.WriteLine(keyValuePair.Key + ": ");
                keyValuePair.Value.ForEach(i => Console.Write($"{i} "));
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}