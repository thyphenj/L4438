using System;
using System.Collections.Generic;
using System.Linq;

namespace Listener
{
    class Clue
    {
        public int NumberOfFactors { get; set; }
        public int Len { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        private Direction Dirn { get; }


        public List<Result> Results { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="facs">number of factors</param>
        /// <param name="len">length of answer</param>
        /// <param name="dirn">across or down</param>
        /// <param name="xPos">grid X position</param>
        /// <param name="yPos">grid Y position</param>

        public Clue(int facs, int len, Direction dirn, int xPos, int yPos)
        {
            Len = len;
            NumberOfFactors = facs;
            XPos = xPos;
            YPos = yPos;
            Dirn = dirn;

            Results = new List<Result>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="facs">number of factors</param>
        /// <param name="len">length of answer</param>
        /// <param name="dirn">across or down</param>

        public Clue(int facs, int len, Direction dirn = Direction.Across)
        {
            Len = len;
            NumberOfFactors = facs;

            Results = new List<Result>();
        }

        public bool AddResult(int attempt)
        {
            Results.Add(new Result(attempt));

            return false;
        }
        public bool AddResult(Result result)
        {
            Results.Add(result);

            return false;
        }
        public bool TryWith(int attempt)
        {
            if (UniqueAndNonZero(attempt))
            {
                if (attempt.ToString().Length == Len)
                {
                    Result Attempt = new Result(attempt);
                    if (Attempt.FactorCount == NumberOfFactors)
                        AddResult(Attempt);
                }
            }
            return true;
        }
        public bool TryCounting()
        {
            int from = 0;
            int to = 0;

            switch (Len)
            {
                case 2:
                    from = 12;
                    to = 98;
                    break;
                case 3:
                    from = 123;
                    to = 987;
                    break;
                case 4:
                    from = 1234;
                    to = 8765;
                    break;
                case 5:
                    from = 12345;
                    to = 98765;
                    break;
                case 6:
                    from = 123456;
                    to = 987654;
                    break;
            }

            for (int guess = from; guess <= to; guess++)
            {
                TryWith(guess);
            }
            return true;
        }
        public bool TryWithOnePower(int ind)
        {
            List<int> powersRequired = Calcs.factors(NumberOfFactors);

            for (int p = 0; p < powersRequired.Count(); p++)
                powersRequired[p]--;

            TryWith(Calcs.power(ind, powersRequired[0]));

            return true;
        }

        public bool TryWithTwoPower(int ind, int jnd)
        {
            List<int> powersRequired = Calcs.factors(NumberOfFactors);

            for (int p = 0; p < powersRequired.Count(); p++)
                powersRequired[p]--;

            TryWith(Calcs.power(ind, powersRequired[0]) * Calcs.power(jnd, powersRequired[1]));

            return true;
        }

        public override string ToString()
        {
            if (Results.Count == 1)
                return $"-----------------> {Results[0].Number,6}";
            else if (Results.Count == 0)
                return $"({Len})";
            else
                return $"({Len}), count={Results.Count,2}";
        }

        private bool UniqueAndNonZero(int attempt)
        {
            int xx = attempt.ToString().Distinct().Count();
            int yy = attempt.ToString().IndexOf('0');

            return (xx == Len && yy < 0);
        }
    }
}
