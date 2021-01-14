using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day15
{
    public static class RambunctiousRecitation
    {
        public static void Day15()
        {
            var path = $"{SD.Path}15{SD.Ext}";
            var puzzleInput = File.ReadLines(path).Select(int.Parse).ToArray();

            Console.WriteLine("Day 15");
            Console.WriteLine($"The 2020th number spoken is {LastNumberSpoken(puzzleInput, 2020)}");
            Console.WriteLine($"The 30000000th number spoken is {LastNumberSpoken(puzzleInput, 30000000)}");
        }

        private static int LastNumberSpoken(IReadOnlyList<int> input, int number)
        {
            var spoken = new int[number];
            Array.Fill(spoken, -1);

            var turn = 1;
            for (; turn < input.Count + 1; turn++)
            {
                spoken[input[turn - 1]] = turn;
            }

            var currentNumber = 0;
            for (; turn < number; turn++)
            {
                var prevTime = spoken[currentNumber];
                spoken[currentNumber] = turn;
                currentNumber = prevTime != -1 ? turn - prevTime : 0;
            }

            return currentNumber;
        }
    }
}
