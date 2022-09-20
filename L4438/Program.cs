using System;
using System.Collections.Generic;
using System.Linq;

namespace Listener
{
    class Program
    {
        static Dictionary<char, Clue> clues = new Dictionary<char, Clue>();
        static void Main()
        {

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

            // ------------------------------------------------------------------------------------------------
            // -- Some of these are obvious POWERS ...

            foreach (int primeInd in Calcs.primes)
            {
                clues['G'].TryWithOnePower(primeInd);
                clues['K'].TryWithOnePower(primeInd);
                clues['S'].TryWithOnePower(primeInd);

                foreach (int primeJnd in Calcs.primes)
                {
                    if (primeInd != primeJnd)
                    {
                        clues['j'].TryWithTwoPower(primeInd, primeJnd);
                        clues['m'].TryWithTwoPower(primeInd, primeJnd);
                        clues['p'].TryWithTwoPower(primeInd, primeJnd);
                        clues['q'].TryWithTwoPower(primeInd, primeJnd);
                    }
                }
            }
            DropOut('K');
            DropOut('m');
            DropOut('p');

            // -- First digit of m will fix G

            clues['G'].IncludeDigitAtPosition(clues['m'].GetDigit(0), 0);
            DropOut('G');

            // -- We already have two digits of J so ...

            clues['J'].TryCounting();
            clues['J'].IncludeDigitAtPosition(clues['p'].GetDigit(0), 0);
            clues['J'].IncludeDigitAtPosition(clues['m'].GetDigit(1), 1);
            DropOut('J');

            // -- Last two digits of j are now fixed

            clues['j'].IncludeDigitAtPosition(clues['G'].GetDigit(3), 2);
            clues['j'].IncludeDigitAtPosition(clues['K'].GetDigit(0), 3);
            DropOut('j');

            // -- Last 2 digits of M are fixed and this actually fixes N!

            clues['N'].TryCounting();
            clues['N'].ExcludeDigits(clues['p'].GetDigit(1));
            clues['N'].ExcludeDigits(clues['m'].GetDigit(2));
            DropOut('N');

            // -- m is fixed - look at c,u

            int mVal = clues['m'].GetNumber();

            clues['c'].TryCountingExcluding(mVal);
            clues['u'].TryCountingExcluding(mVal);
            CrossMatch(clues['c'], clues['u']);
            DropOut('c');
            DropOut('u');

            // -- Given that we have p m and N, try for M

            clues['M'].TryCounting();
            clues['M'].IncludeDigitAtPosition(clues['p'].GetDigit(1), 2);
            clues['M'].IncludeDigitAtPosition(clues['m'].GetDigit(2), 3);
            CrossMatch(clues['M'], clues['N']);
            DropOut('M');

            // -- Try for B 

            clues['B'].TryCounting();
            clues['B'].ExcludeDigits(clues['c'].GetDigit(1));
            clues['B'].ExcludeDigits(clues['j'].GetDigit(0));
            DropOut('B');

            clues['C'].TryCountingExcluding(clues['B'].GetNumber());
            clues['C'].IncludeDigitAtPosition(clues['c'].GetDigit(1), 2);
            clues['C'].IncludeDigitAtPosition(clues['j'].GetDigit(0), 5);
            clues['C'].ExcludeDigitsAtPosition(clues['M'].GetDigit(1), 0);
            clues['C'].ExcludeDigitsAtPosition(clues['p'].GetNumber(), 1);
            clues['C'].ExcludeDigitsAtPosition(clues['G'].GetDigit(1), 3);
            clues['C'].ExcludeDigitsAtPosition(clues['J'].GetDigit(2), 3);
            clues['C'].ExcludeDigitsAtPosition(clues['N'].GetDigit(0), 3);
            clues['C'].ExcludeDigitsAtPosition(clues['G'].GetDigit(2), 4);
            clues['C'].ExcludeDigitsAtPosition(clues['N'].GetDigit(1), 4);
            CrossMatch(clues['B'], clues['C']);

            clues['D'].TryCountingExcluding(clues['j'].GetDigit(1));
            clues['D'].IncludeDigitAtPosition(clues['c'].GetDigit(2), 4);
            clues['D'].ExcludeDigitsAtPosition(clues['B'].GetDigit(0), 0);

            clues['E'].TryCountingExcluding(clues['c'].GetDigit(2));
            clues['E'].IncludeDigitAtPosition(clues['j'].GetDigit(1), 2);
            CrossMatch(clues['D'], clues['E']);

            clues['F'].TryCountingExcluding(clues['G'].GetNumber());
            clues['F'].ExcludeDigitsAtPosition(clues['B'].GetDigit(1), 0);
            clues['F'].ExcludeDigitsAtPosition(clues['M'].GetDigit(0), 0);
            clues['F'].ExcludeDigitsAtPosition(clues['M'].GetDigit(1), 1);
            clues['F'].ExcludeDigitsAtPosition(clues['p'].GetNumber(), 2);
            CrossMatch(clues['F'], clues['G']);
            DropOut('F');

            clues['H'].TryCountingExcluding(clues['J'].GetNumber(), clues['K'].GetNumber());

            clues['P'].TryCountingExcluding(clues['u'].GetDigit(0));
            clues['P'].IncludeDigitAtPosition(clues['p'].GetDigit(2), 3);
            clues['P'].ExcludeDigitsAtPosition(clues['B'].GetDigit(0), 0);
            clues['P'].ExcludeDigitsAtPosition(clues['B'].GetDigit(1), 1);
            clues['P'].ExcludeDigitsAtPosition(clues['F'].GetDigit(1), 2);
            clues['P'].ExcludeDigitsAtPosition(clues['M'].GetDigit(1), 2);

            clues['Q'].TryCountingExcluding(clues['p'].GetDigit(2));
            clues['Q'].IncludeDigitAtPosition(clues['u'].GetDigit(0), 0);
            clues['Q'].ExcludeDigitsAtPosition(clues['G'].GetDigit(1), 1);
            clues['Q'].ExcludeDigitsAtPosition(clues['J'].GetDigit(2), 1);
            clues['Q'].ExcludeDigitsAtPosition(clues['N'].GetDigit(0), 1);
            clues['Q'].ExcludeDigitsAtPosition(clues['G'].GetDigit(2), 2);
            clues['Q'].ExcludeDigitsAtPosition(clues['N'].GetDigit(1), 2);
            clues['Q'].ExcludeDigitsAtPosition(clues['j'].GetNumber(), 3);
            clues['Q'].ExcludeDigitsAtPosition(clues['N'].GetDigit(2), 3);
            CrossMatch(clues['P'], clues['Q']);

            clues['R'].TryCounting();
            clues['R'].IncludeDigitAtPosition(clues['p'].GetDigit(3), 2);
            clues['R'].IncludeDigitAtPosition(clues['u'].GetDigit(1), 3);
            clues['R'].ExcludeDigitsAtPosition(clues['B'].GetDigit(1), 0);
            clues['R'].ExcludeDigitsAtPosition(clues['F'].GetDigit(0), 0);
            clues['R'].ExcludeDigitsAtPosition(clues['M'].GetDigit(0), 0);
            clues['R'].ExcludeDigitsAtPosition(clues['F'].GetDigit(1), 1);
            clues['R'].ExcludeDigitsAtPosition(clues['M'].GetDigit(1), 1);
            clues['R'].ExcludeDigitsAtPosition(clues['G'].GetDigit(1), 4);
            clues['R'].ExcludeDigitsAtPosition(clues['J'].GetDigit(2), 4);
            clues['R'].ExcludeDigitsAtPosition(clues['N'].GetDigit(0), 4);
            clues['R'].ExcludeDigitsAtPosition(clues['G'].GetDigit(2), 5);
            clues['R'].ExcludeDigitsAtPosition(clues['N'].GetDigit(1), 5);
            CrossMatch(clues['R'], clues['S']);

            // ------------------------------------------------------------------------------------------------

            clues['f'].TryCountingExcluding(clues['M'].GetDigit(0));
            clues['f'].IncludeDigitAtPosition(clues['B'].GetDigit(1), 0);
            clues['f'].IncludeDigitAtPosition(clues['F'].GetDigit(0), 2);
            clues['f'].ExcludeDigitsAtPosition(clues['c'].GetDigit(2), 1);
            clues['f'].ExcludeDigitsAtPosition(clues['j'].GetDigit(1), 1);

            clues['n'].TryCountingExcluding(clues['B'].GetDigit(1));
            clues['n'].IncludeDigitAtPosition(clues['M'].GetDigit(0), 1);
            CrossMatch(clues['f'], clues['n']);

            clues['b'].TryCountingExcluding(clues['M'].GetDigit(1));

            clues['k'].TryCountingExcluding(clues['M'].GetDigit(1));
            clues['k'].IncludeDigitAtPosition(clues['F'].GetDigit(1), 1);
            clues['k'].ExcludeDigitsAtPosition(clues['J'].GetNumber(), 2);
            clues['k'].ExcludeDigitsAtPosition(clues['K'].GetNumber(), 2);

            clues['r'].TryCounting();
            clues['r'].IncludeDigitAtPosition(clues['M'].GetDigit(1), 0);
            clues['r'].ExcludeDigits(clues['k'].GetNumber());
            clues['r'].ExcludeDigitsAtPosition(clues['u'].GetDigit(1),2);
            CrossMatch(clues['b'], clues['k']);
            CrossMatch(clues['r'], clues['k']);

            clues['g'].TryCountingExcluding(clues['p'].GetNumber());
            clues['g'].IncludeDigitAtPosition(clues['F'].GetDigit(2), 2);
            CrossMatch(clues['g'], clues['p']);

            clues['h'].TryCounting();
            clues['h'].IncludeDigitAtPosition(clues['G'].GetDigit(1), 2);
            clues['h'].IncludeDigitAtPosition(clues['J'].GetDigit(2), 3);

            clues['h'].ExcludeDigits(clues['N'].GetDigit(0));

            clues['s'].TryCountingExcluding(clues['G'].GetDigit(1), clues['J'].GetDigit(2));
            clues['s'].ExcludeDigitsAtPosition(clues['r'].GetDigit(1), 1);
            clues['s'].ExcludeDigitsAtPosition(clues['p'].GetDigit(2), 1);
            clues['s'].ExcludeDigitsAtPosition(clues['u'].GetDigit(0), 1);
            CrossMatch(clues['h'], clues['s']);

            clues['d'].TryCountingExcluding(clues['N'].GetDigit(1));
            clues['d'].IncludeDigitAtPosition(clues['G'].GetDigit(2), 3);

            clues['q'].TryCountingExcluding(clues['G'].GetDigit(2));
            clues['q'].IncludeDigitAtPosition(clues['N'].GetDigit(1), 1);

            clues['v'].TryCounting();
            CrossMatch(clues['d'], clues['q']);
            CrossMatch(clues['d'], clues['v']);
            CrossMatch(clues['q'], clues['v']);

            clues['t'].TryCountingExcluding(clues['j'].GetNumber());
            CrossMatch(clues['j'], clues['t']);

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

        private static void DropOut(char key)
        {
            Console.WriteLine($"{key} {clues[key],6}");
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
