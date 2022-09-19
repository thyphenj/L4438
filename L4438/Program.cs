using System;
using System.Collections.Generic;
using System.Linq;

namespace Listener
{
    class Program
    {
        static void Main()
        {
            var clues = new Dictionary<char, Clue>();

            //var A = new Clue();            
            //var T = new Clue();

            clues.Add('B', new Clue(facs: 12, len: 2, dirn: Direction.Across, xPos: 0, yPos: 1));
            clues.Add('C', new Clue(2, 6));
            clues.Add('D', new Clue(6, 5));
            clues.Add('E', new Clue(12, 4));
            clues.Add('F', new Clue(2, 3, Direction.Across, 1, 3));
            clues.Add('G', new Clue(3, 4, Direction.Across, 4, 3));
            clues.Add('H', new Clue(4, 2, Direction.Across, 0, 4));
            clues.Add('J', new Clue(12, 3, Direction.Across, 3, 4));
            clues.Add('K', new Clue(7, 2));
            clues.Add('M', new Clue(20, 4));
            clues.Add('N', new Clue(18, 3));
            clues.Add('P', new Clue(8, 4, Direction.Across, 0, 6));
            clues.Add('Q', new Clue(2, 5));
            clues.Add('R', new Clue(24, 6));
            clues.Add('S', new Clue(5, 2));


            //var a = new Clue();
            clues.Add('b', new Clue(8, 2, Direction.Down));
            clues.Add('c', new Clue(16, 3, Direction.Down));
            clues.Add('d', new Clue(24, 4, Direction.Down));
            //var e = new Clue();
            clues.Add('f', new Clue(2, 3, Direction.Down));
            clues.Add('g', new Clue(4, 3, Direction.Down));
            clues.Add('h', new Clue(8, 4, Direction.Down));
            clues.Add('j', new Clue(9, 4, Direction.Down));
            clues.Add('k', new Clue(8, 3, Direction.Down));
            clues.Add('m', new Clue(21, 3, Direction.Down));
            clues.Add('n', new Clue(20, 4, Direction.Down));
            clues.Add('p', new Clue(35, 4, Direction.Down));
            clues.Add('q', new Clue(10, 3, Direction.Down));
            clues.Add('r', new Clue(8, 4, Direction.Down));
            clues.Add('s', new Clue(2, 3, Direction.Down));
            clues.Add('t', new Clue(2, 3, Direction.Down));
            clues.Add('u', new Clue(14, 3, Direction.Down));
            clues.Add('v', new Clue(4, 2, Direction.Down));

            // *** Some of these are obvious POWERS ...
            foreach (int ind in Calcs.primes)
            {
                clues['K'].TryWithOnePower(ind);
                clues['S'].TryWithOnePower(ind);

                foreach (int jnd in Calcs.primes)
                {
                    if (ind != jnd)
                    {
                        clues['m'].TryWithTwoPower(ind, jnd);
                        clues['p'].TryWithTwoPower(ind, jnd);
                    }
                }
            }

            // ------------------------------------------------------------------------------------------------
            // -- At this point we have p, m and K determined, so let's look at J, G and j

            // -- We already have two digits of J so ...
            {
                int firstTwoDigits = clues['p'].GetDigit(0) * 100 + clues['m'].GetDigit(1) * 10;

                for (int guess = 1; guess <= 9; guess++)
                {
                    clues['J'].TryWith(firstTwoDigits + guess);
                }
            }

            // -- First digit of m will fix G
            {
                int firstDigit = clues['m'].GetDigit(0) * 1000;

                for (int guess = 123; guess <= 987; guess++)
                {
                    clues['G'].TryWith(guess + firstDigit);
                }
            }

            // -- Last two digits of j are now fixed
            {
                int lastTwoDigits = clues['G'].GetDigit(3) * 10 + clues['K'].GetDigit(0);

                for (int guess = 12; guess <= 98; guess++)
                {
                    clues['j'].TryWith(guess * 100 + lastTwoDigits);
                }
            }

            // -- Last 2 digits of M are fixed and this actually fixes N!
            {
                int lastTwoDigits = clues['p'].GetDigit(1) * 10 + clues['m'].GetDigit(2);

                clues['N'].TryCountingExcluding(lastTwoDigits);
            }

            // -- m is fixed - look at c,u
            {
                int mVal = clues['m'].GetNumber();

                clues['c'].TryCountingExcluding(mVal);
                clues['u'].TryCountingExcluding(mVal);

                CrossMatch(clues['c'], clues['u']);
            }

            // -- Given that we have p m and N, try for M
            {
                int lastTwoDigits = clues['p'].GetDigit(1) * 10 + clues['m'].GetDigit(2);
                for (int guess = 12; guess <= 98; guess++)
                {
                    clues['M'].TryWith(guess * 100 + lastTwoDigits);
                }
                CrossMatch(clues['M'], clues['N']);
            }

            // -- we have p, so try for g
            {
                clues['g'].TryCounting();

                CrossMatch(clues['p'], clues['g']);
            }

            // -- Try for B 
            {
                int fromc = clues['c'].GetDigit(1) * 10 + clues['j'].GetDigit(0);

                clues['B'].TryCountingExcluding(fromc);
            }

            // ------------------------------------------------------------------------------------------------
            {
                int lastTwoDigits = clues['G'].GetDigit(1) * 10 + clues['J'].GetDigit(2);

                for (int guess = 12; guess <= 98; guess++)
                {
                    clues['h'].TryWith(guess * 100 + lastTwoDigits);
                }
                clues['h'].ExcludeDigitsAtPosition(clues['B'].GetNumber(), 0);
                clues['h'].ExcludeDigitsAtPosition(clues['c'].GetDigit(1), 0);
                clues['h'].ExcludeDigitsAtPosition(clues['j'].GetDigit(0), 0);
                clues['h'].ExcludeDigitsAtPosition(clues['j'].GetDigit(1), 1);
            }

            clues['C'].TryCountingExcluding(clues['B'].GetNumber());
            clues['C'].IncludeDigitAtPosition(clues['c'].GetDigit(1), 2);
            clues['C'].IncludeDigitAtPosition(clues['j'].GetDigit(0), 5);
            clues['C'].ExcludeDigitsAtPosition(clues['p'].GetNumber(), 1);
            clues['C'].ExcludeDigitsAtPosition(clues['G'].GetDigit(1), 3);
            clues['C'].ExcludeDigitsAtPosition(clues['J'].GetDigit(2), 3);
            clues['C'].ExcludeDigitsAtPosition(clues['N'].GetDigit(0), 3);
            clues['C'].ExcludeDigitsAtPosition(clues['G'].GetDigit(2), 4);
            clues['C'].ExcludeDigitsAtPosition(clues['N'].GetDigit(1), 4);

            clues['D'].TryCountingExcluding(clues['j'].GetDigit(1));
            clues['D'].IncludeDigitAtPosition(clues['c'].GetDigit(2), 4);
            clues['D'].ExcludeDigitsAtPosition(clues['B'].GetDigit(0), 0);

            clues['E'].TryCountingExcluding(clues['c'].GetDigit(2));
            clues['E'].IncludeDigitAtPosition(clues['j'].GetDigit(1),2);
            CrossMatch(clues['D'], clues['E']);
            
            clues['F'].TryCountingExcluding(clues['G'].GetNumber());
            clues['F'].ExcludeDigitsAtPosition(clues['B'].GetDigit(1), 0);
            clues['F'].ExcludeDigitsAtPosition(clues['M'].GetDigit(0), 0);
            clues['F'].ExcludeDigitsAtPosition(clues['M'].GetDigit(1), 1);
            clues['F'].ExcludeDigitsAtPosition(clues['J'].GetNumber(), 2);
            CrossMatch(clues['F'], clues['G']);

            clues['H'].TryCountingExcluding(clues['J'].GetNumber(), clues['K'].GetNumber());

            clues['P'].TryCountingExcluding(clues['u'].GetDigit(0));
            clues['P'].IncludeDigitAtPosition(clues['p'].GetDigit(2), 3);
            clues['P'].ExcludeDigitsAtPosition(clues['B'].GetDigit(0), 0);
            clues['P'].ExcludeDigitsAtPosition(clues['B'].GetDigit(1), 1);
            clues['P'].ExcludeDigitsAtPosition(clues['M'].GetDigit(0), 1);
            clues['P'].ExcludeDigitsAtPosition(clues['M'].GetDigit(1), 2);
            clues['Q'].TryCountingExcluding(clues['p'].GetDigit(2));
            clues['Q'].IncludeDigitAtPosition(clues['u'].GetDigit(0), 0);
            clues['Q'].ExcludeDigitsAtPosition(clues['G'].GetDigit(1), 1);
            clues['Q'].ExcludeDigitsAtPosition(clues['J'].GetDigit(2), 1);
            clues['Q'].ExcludeDigitsAtPosition(clues['N'].GetDigit(0), 1);
            clues['Q'].ExcludeDigitsAtPosition(clues['G'].GetDigit(2), 2);
            clues['Q'].ExcludeDigitsAtPosition(clues['N'].GetDigit(1), 2);
            clues['Q'].ExcludeDigitsAtPosition(clues['N'].GetDigit(2), 3);
            CrossMatch(clues['P'], clues['Q']);

            clues['R'].TryCounting();
            clues['R'].IncludeDigitAtPosition(clues['p'].GetDigit(3), 2);
            clues['R'].IncludeDigitAtPosition(clues['u'].GetDigit(1), 3);
            CrossMatch(clues['R'], clues['S']);

            clues['b'].TryCountingExcluding(clues['M'].GetDigit(1));

            clues['d'].TryCountingExcluding(clues['N'].GetDigit(1));
            clues['d'].IncludeDigitAtPosition(clues['G'].GetDigit(2), 3);

            clues['f'].TryCountingExcluding(clues['M'].GetDigit(0));
            clues['f'].IncludeDigitAtPosition(clues['B'].GetDigit(1), 0);
            clues['f'].ExcludeDigitsAtPosition(clues['c'].GetDigit(2), 1);
            clues['f'].ExcludeDigitsAtPosition(clues['j'].GetDigit(1), 1);


            clues['k'].TryCountingExcluding(clues['M'].GetDigit(1));
            clues['n'].TryCountingExcluding(clues['B'].GetDigit(1));

            clues['q'].TryCountingExcluding(clues['G'].GetDigit(2));
            clues['q'].IncludeDigitAtPosition(clues['N'].GetDigit(1), 1);

            clues['r'].TryCounting();
            clues['s'].TryCountingExcluding(clues['G'].GetDigit(1), clues['J'].GetDigit(2));
            clues['t'].TryCountingExcluding(clues['j'].GetNumber());
            clues['v'].TryCounting();

            foreach (var clue in clues)
            {
                if (clue.Value.Results.Count == 1)
                    Console.WriteLine(clue);
            }
            Console.WriteLine();
            foreach (var clue in clues)
            {
                if (clue.Value.Results.Count != 1)
                    Console.WriteLine(clue);
            }

            Console.WriteLine();
        }

        private static void CrossMatch(Clue clue1, Clue clue2)
        {
            Match(clue1, clue2);
            Match(clue2, clue1);
        }

        private static void Match(Clue clue1, Clue clue2)
        {
            var toBeRemoved = new List<Result>();

            foreach (var cVal in clue1.Results)
            {
                bool possibleFound = false;
                foreach (var uVal in clue2.Results)
                {
                    possibleFound |= NoDigitsInCommon(cVal.Number, uVal.Number);
                }
                if (!possibleFound)
                    toBeRemoved.Add(cVal);
            }
            foreach (var val in toBeRemoved)
            {
                clue1.RemoveResult(val);
            }

        }
        private static bool NoDigitsInCommon(int number1, int number2)
        {
            foreach (char a in number1.ToString())
                foreach (char b in number2.ToString())
                    if (a == b)
                        return false;
            return true;
        }
    }
}
