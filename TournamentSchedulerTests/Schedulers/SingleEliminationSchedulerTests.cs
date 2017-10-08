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
    public class SingleEliminationSchedulerTests
    {
        private SingleEliminationScheduler<int> scheduler;
        private List<int> testCompetitors;

        [TestInitialize()]
        public void Setup()
        {
            testCompetitors = new List<int>();
            for (var i = 0; i < 16; i++)
            {
                testCompetitors.Add(i);
            }
            scheduler = new SingleEliminationScheduler<int>();
        }

        [TestMethod()]
        public void LoadCompetitorsTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            Assert.IsTrue(scheduler.competitors.Count == testCompetitors.Count);
            Assert.IsTrue(scheduler.competitors.All(i => testCompetitors.Contains(i)));
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void LoadCompetitorsNotPowerOfTwoTest()
        {
            testCompetitors = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                testCompetitors.Add(i);
            }
            scheduler.LoadCompetitors(testCompetitors); // should throw exception
        }

        [TestMethod()]
        public void CalculateNextMatchupsTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            scheduler.CalculateNextMatchups();
            // submit all results so we can calculate again
            scheduler.currentMatchups.ForEach(matchup => matchup.Winner = matchup.Challenger1);
            scheduler.CalculateNextMatchups();
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void CalculateNextMatchupsFailTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            scheduler.CalculateNextMatchups();
            // Second call will fail as not all results are submitted yet
            scheduler.CalculateNextMatchups();
        }

        [TestMethod()]
        public void GetCurrentMatchupsTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            scheduler.CalculateNextMatchups();
            // first iteration started
            var matchups = scheduler.GetCurrentMatchups();
            Assert.IsTrue(matchups.Count == testCompetitors.Count / 2);
            Assert.IsTrue(matchups.TrueForAll(matchup => scheduler.competitors.Contains(matchup.Challenger1)));
            Assert.IsTrue(matchups.TrueForAll(matchup => scheduler.competitors.Contains(matchup.Challenger2)));
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetCurrentMatchupsFailTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            // if we call GetCurrentMatchups without calculating once, it should raise an exception
            scheduler.GetCurrentMatchups();
        }
    }
}