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
            RoundRobinSchedulerSample.RunSample();
            SingleEliminationSchedulerSample.RunSample();

            Console.ReadLine();
        }
    }
}