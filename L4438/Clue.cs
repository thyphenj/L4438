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

        private int From;
        private int To;

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

            GetExtents();
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

            GetExtents();
        }

        // -- Return some values

        internal int GetNumber()
        {
            if (Results.Count != 1)
                throw new ArgumentOutOfRangeException();

            return Results[0].Number;
        }

        internal int GetDigit(int v)
        {
            if (Results.Count != 1)
                throw new ArgumentOutOfRangeException();

            return Results[0].Digits[v];
        }

        // -- Add results

        internal bool AddResult(int attempt)
        {
            Results.Add(new Result(attempt));

            return false;
        }

        internal bool AddResult(Result result)
        {
            Results.Add(result);

            return false;
        }

        internal bool RemoveResult(Result result)
        {
            Results.Remove(result);

            return false;
        }

        // -- Exclude certain digits at certain position

        internal bool ExcludeDigitsAtPosition(int digits, int pos)
        {
            string digStr = digits.ToString();

            var removeThese = new List<Result>();

            foreach (var res in Results)
                foreach (var dig in digStr)
                {
                    if (res.Digits[pos].ToString() == dig.ToString())
                        removeThese.Add(res);
                }

            foreach (var rem in removeThese)
                Results.Remove(rem);

            return true;
        }

        // -- Only INCLUDE specified digit at position

        internal bool IncludeDigitAtPosition(int digit, int pos)
        {
            var removeThese = new List<Result>();

            foreach (var res in Results)
            {
                if (res.Digits[pos] != digit)
                    removeThese.Add(res);
            }

            foreach (var rem in removeThese)
                Results.Remove(rem);

            return true;
        }
        // -- Attempt at single value

        internal bool TryWith(int attempt)
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

        // -- Attempt with multiple values

        internal bool TryCounting()
        {
            for (int guess = From; guess <= To; guess++)
            {
                TryWith(guess);
            }
            return true;
        }

        internal bool TryCountingExcluding(int exclude1, int exclude2)
        {
            for (int guess = From; guess <= To; guess++)
            {
                if (DoesNotContain(guess, exclude1))
                    if (DoesNotContain(guess, exclude2))
                        TryWith(guess);
            }
            return true;
        }

        internal bool TryCountingExcluding(int exclude1)
        {
            for (int guess = From; guess <= To; guess++)
            {
                if (DoesNotContain(guess, exclude1))
                    TryWith(guess);
            }
            return true;
        }

        // -- Obviously POWERS

        internal bool TryWithOnePower(int ind)
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

        // --

        public override string ToString()
        {
            if (Results.Count == 0)
                return "";

            else if (Results.Count == 1)
                return $"-----------------> {Results[0].Number,6}";

            else if (Results.Count > 10)
                return $"count={Results.Count,2}";
            else
            {
                string str = "";
                foreach (var s in Results)
                    str += $"{s.Number,6}  ";
                return str;
            }
        }

        // -- local funcs

        private bool UniqueAndNonZero(int attempt)
        {
            int xx = attempt.ToString().Distinct().Count();
            int yy = attempt.ToString().IndexOf('0');

            return (xx == Len && yy < 0);
        }

        private void GetExtents()
        {
            switch (Len)
            {
                case 2:
                    From = 12;
                    To = 98;
                    break;
                case 3:
                    From = 123;
                    To = 987;
                    break;
                case 4:
                    From = 1234;
                    To = 8765;
                    break;
                case 5:
                    From = 12345;
                    To = 98765;
                    break;
                case 6:
                    From = 123456;
                    To = 987654;
                    break;
            }
        }

        private bool DoesNotContain(int text, int pattern)
        {
            string gStr = pattern.ToString();

            foreach (var guessChar in text.ToString())
                if (gStr.Contains(guessChar))
                    return false;

            return true;
        }

    }
}
