using System;
using System.Collections.Generic;

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
            // -- At this point we have p, m and K determined, so let's look at J, G

            // -- We already have two digits of J so ...
            {
                int firstTwoDigits = clues['p'].Results[0].Digits[0] * 100 + clues['m'].Results[0].Digits[1] * 10;
                for (int guess = 1; guess <= 9; guess++)
                {
                    clues['J'].TryWith(firstTwoDigits + guess);
                }
            }

            // -- First digit of m will fix G
            {
                int firstDigit = clues['m'].Results[0].Digits[0] * 1000;
                for (int guess = 123; guess <= 987; guess++)
                {
                    clues['G'].TryWith(guess + firstDigit);
                }
            }

            // -- Last two digits of j are now fixed
            {
                int lastTwoDigits = clues['G'].Results[0].Digits[3] *10 + clues['K'].Results[0].Digits[0] ;
                for (int guess = 12; guess <= 98; guess++)
                {
                    clues['j'].TryWith(guess*100 + lastTwoDigits);
                }
            }

            // ------------------------------------------------------------------------------------------------
            // -- Last 2 digits of M are fixed
            {
                int lastTwoDigits = clues['p'].Results[0].Digits[1] * 10 + clues['m'].Results[0].Digits[2];
                for ( int guess = 12; guess <= 98; guess++)
                {
                    clues['M'].TryWith(guess*100+lastTwoDigits);
                }
            }

            clues['B'].TryCounting();
            clues['C'].TryCounting();
            clues['D'].TryCounting();
            clues['E'].TryCounting();
            clues['F'].TryCounting();

            clues['H'].TryCounting();

            clues['N'].TryCounting();
            clues['P'].TryCounting();
            clues['Q'].TryCounting();
            clues['R'].TryCounting();

            clues['b'].TryCounting();
            clues['c'].TryCounting();
            clues['d'].TryCounting();
            clues['f'].TryCounting();
            clues['g'].TryCounting();
            clues['h'].TryCounting();
            clues['k'].TryCounting();
            clues['n'].TryCounting();
            clues['q'].TryCounting();
            clues['s'].TryCounting();
            clues['t'].TryCounting();
            clues['u'].TryCounting();
            clues['v'].TryCounting();


            Console.WriteLine();
        }
    }
}
