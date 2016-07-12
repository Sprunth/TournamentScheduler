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
        private List<int> testCompetitors;

        [TestInitialize()]
        public void Setup()
        {
            testCompetitors = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                testCompetitors.Add(i);
            }
        }

        [TestMethod()]
        public void LoadCompetitorsTest()
        {
            scheduler.LoadCompetitors(testCompetitors);

            var firstNotSecond = testCompetitors.Except(scheduler.competitors).ToList();
            var secondNotFirst = scheduler.competitors.Except(testCompetitors).ToList();

            // There is one T in first and not second and that's the competitor not
            // rotated in the algorithm
            Assert.IsTrue((firstNotSecond.Count == 1) && (secondNotFirst.Count == 0));
        }

        [TestMethod()]
        public void LoadCompetitorsTest2()
        {
            scheduler.LoadCompetitors(testCompetitors);

            var firstNotSecond = testCompetitors.Except(scheduler.competitors).ToList();
            var secondNotFirst = scheduler.competitors.Except(testCompetitors).ToList();

            // There is one T in first and not second and that's the competitor not
            // rotated in the algorithm
            var lastOne = firstNotSecond[0];
            Assert.IsTrue(scheduler.firstCompetitor.Equals(lastOne));
        }

        [TestMethod()]
        public void CalculateMatchupsTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            scheduler.CalculateMatchups();
            var matchups = scheduler.Matchups;

            var matchupsBrokenDown = new Dictionary<int, List<int>>();
            testCompetitors.ForEach(i => matchupsBrokenDown.Add(i, new List<int>()));

            foreach (var matchup in matchups)
            {
                matchupsBrokenDown[matchup.Item1].Add(matchup.Item2);
                matchupsBrokenDown[matchup.Item2].Add(matchup.Item1);
            }

            var wrongMatchCount =
                matchupsBrokenDown.Where(keyValuePair => keyValuePair.Value.Count != testCompetitors.Count - 1)
                    .ToList()
                    .Count;
            Assert.IsTrue(wrongMatchCount == 0);
        }

        [TestMethod()]
        public void CalculateMatchupsByDayTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            scheduler.CalculateMatchups();
            var matchupsByDay = scheduler.MatchupsByDay;

            Assert.IsTrue(matchupsByDay.All(tuples => tuples.Count == (int) Math.Ceiling(testCompetitors.Count/2.0)));
        }

        [TestMethod()]
        public void CalculateMatchupsByDayTest2()
        {
            scheduler.LoadCompetitors(testCompetitors);
            scheduler.CalculateMatchups();
            var matchupsByDay = scheduler.MatchupsByDay;

            Assert.IsTrue(matchupsByDay.Count == testCompetitors.Count - 1);
        }
    }
}