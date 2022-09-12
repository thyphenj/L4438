using System;
using System.Collections.Generic;
using System.Linq;

namespace Listener
{
    class Result
    {
        public int Number { get; set; }
        public List<int> Digits { get; set; }
        private String Strng { get; set; }
        public List<int> Factors { get; set; }
        public int FactorCount { get; set; }
        public Result (int number)
        {
            Number = number;
            Strng = Number.ToString();

            Digits = new List<int>();
            foreach (char c in Strng)
                Digits.Add(c - '0');

            Factors = Calcs.factors(Number);

            FactorCount = 1;
            foreach (int c in Factors.Distinct().ToList())
            {
                FactorCount *= (Factors.Where(a => a == c).Count() + 1);
            }
        }
        public override string ToString()
        {
            string retval = Number.ToString() + " ( ";

            foreach (int ind in Factors)
                retval += ind.ToString()+" ";

            retval += ")";

            return retval;
        }
    }
}
