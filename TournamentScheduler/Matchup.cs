using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprunth.TournamentScheduler
{
    public struct Matchup<T> where T : IComparable
    {
        public T Challenger1;
        public T Challenger2;

        /// <summary>
        /// Until we change T to be nullable and comparable, we'll need this.
        /// </summary>
        public bool WinnerSet { get; private set; }

        public Matchup(T challenger1, T challenger2)
        {
            Challenger1 = challenger1;
            Challenger2 = challenger2;
            // Doesn't really matter what it's set to since we have WinnerSet
            _winner = default(T);
            WinnerSet = false;
        }

        private T _winner;

        public T Winner
        {
            get { return _winner; }
            set
            {
                if (value.CompareTo(Challenger1) != 0 && value.CompareTo(Challenger2) != 0)
                    throw new Exception("Winner has to be one of the matchup's challengers");
                _winner = value;
                WinnerSet = true;
            }
        }

        public T Loser
        {
            get
            {
                if (!WinnerSet)
                    throw new Exception("No winner set yet");
                return Challenger1.CompareTo(Winner) == 0 ? Challenger2 : Challenger1;
            }
        }
    }
}
