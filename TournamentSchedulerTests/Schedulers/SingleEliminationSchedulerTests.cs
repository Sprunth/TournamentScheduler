using Sprunth.TournamentScheduler.Schedulers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Sprunth.TournamentScheduler.Schedulers.Tests
{
    [TestFixture()]
    public class SingleEliminationSchedulerTests
    {
        private SingleEliminationScheduler<int> scheduler;
        private List<int> testCompetitors;

        [SetUp()]
        public void Setup()
        {
            testCompetitors = new List<int>();
            for (var i = 0; i < 16; i++)
            {
                testCompetitors.Add(i);
            }
            scheduler = new SingleEliminationScheduler<int>();
        }

        [Test()]
        public void LoadCompetitorsTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            Assert.IsTrue(scheduler.competitors.Count == testCompetitors.Count);
            Assert.IsTrue(scheduler.competitors.All(i => testCompetitors.Contains(i)));
        }

        [Test()]
        public void LoadCompetitorsNotPowerOfTwoTest()
        {
            testCompetitors = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                testCompetitors.Add(i);
            }
            Assert.Throws<Exception>(() => scheduler.LoadCompetitors(testCompetitors));
        }

        [Test()]
        public void CalculateNextMatchupsTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            scheduler.CalculateNextMatchups();
            // submit all results so we can calculate again
            scheduler.currentMatchups.ForEach(matchup => matchup.Winner = matchup.Challenger1);
            scheduler.CalculateNextMatchups();
        }

        [Test()]
        public void CalculateNextMatchupsFailTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            scheduler.CalculateNextMatchups();
            // Second call will fail as not all results are submitted yet
            Assert.Throws<Exception>(() => scheduler.CalculateNextMatchups());
        }

        [Test()]
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

        [Test()]
        public void GetCurrentMatchupsFailTest()
        {
            scheduler.LoadCompetitors(testCompetitors);
            // if we call GetCurrentMatchups without calculating once, it should raise an exception
            Assert.Throws<Exception>(() => scheduler.GetCurrentMatchups());
        }
    }
}