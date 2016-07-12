using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprunth.TournamentScheduler.Schedulers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprunth.TournamentScheduler.Schedulers.Tests
{
    [TestClass()]
    public class RoundRobinSchedulerTests
    {
        private readonly RoundRobinScheduler<int> scheduler = new RoundRobinScheduler<int>();
        private List<int> competitors;

        [TestInitialize()]
        public void Setup()
        {
            competitors = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                competitors.Add(i);
            }
        }

        [TestMethod()]
        public void LoadCompetitorsTest()
        {
            scheduler.LoadCompetitors(competitors);

            var firstNotSecond = competitors.Except(scheduler.competitors).ToList();
            var secondNotFirst = scheduler.competitors.Except(competitors).ToList();

            // There is one T in first and not second and that's the competitor not
            // rotated in the algorithm
            Assert.IsTrue((firstNotSecond.Count == 1) && (secondNotFirst.Count == 0));
        }

        [TestMethod()]
        public void CalculateMatchupsTest()
        {
            scheduler.LoadCompetitors(competitors);
            var matchups = scheduler.CalculateMatchups();

            var matchupsBrokenDown = new Dictionary<int, List<int>>();
            competitors.ForEach(i => matchupsBrokenDown.Add(i, new List<int>()));

            foreach (var matchup in matchups)
            {
                matchupsBrokenDown[matchup.Item1].Add(matchup.Item2);
                matchupsBrokenDown[matchup.Item2].Add(matchup.Item1);
            }

            var wrongMatchCount =
                matchupsBrokenDown.Where(keyValuePair => keyValuePair.Value.Count != competitors.Count - 1)
                    .ToList()
                    .Count;
            Assert.IsTrue(wrongMatchCount == 0);
        }
    }
}