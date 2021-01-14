using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day8
{
    public static class HandheldHalting
    {
        public static void Day8()
        {
            var instructionsDictionary = BuildDictionary();
            var jmpList = new List<int>();
            
            Console.WriteLine("Day 8");
            Console.WriteLine($"Accumulator value = {GetDay8Part1(instructionsDictionary, ref jmpList)}");
            Console.WriteLine($"Accumulator after program terminates = {GetDay8Part2(instructionsDictionary, jmpList)}");
        }

        private static int GetDay8Part2(ImmutableDictionary<int, Tuple<string, int>> instructionsDictionary, IEnumerable<int> jmpList)
        {
            var answer = 0;
            foreach (var jump in jmpList.OrderByDescending(x => x))
            {
                var usedInstructionNumbers = new List<int>();
                var accumulator = 0;
                var instructionNumber = 0;
                do
                {
                    var type = jump==instructionNumber ? "nop" : instructionsDictionary[instructionNumber].Item1;
                    var movement = instructionsDictionary[instructionNumber].Item2;
                    usedInstructionNumbers.Add(instructionNumber);
                    switch (type)
                    {
                        case "acc":
                            instructionNumber++;
                            if (usedInstructionNumbers.Exists(e => instructionNumber == e))
                            {
                                answer = accumulator;
                                break;
                            }

                            accumulator += movement;
                            break;
                        case "nop":
                            instructionNumber++;
                            break;
                        case "jmp":
                            instructionNumber += movement;
                            break;
                    }
                } while (instructionNumber < instructionsDictionary.Count && answer == 0);

                if (instructionNumber < instructionsDictionary.Count) continue;
                answer = accumulator;
                break;
            }

            return answer;
        }

        private static int GetDay8Part1(ImmutableDictionary<int, Tuple<string, int>> instructionsDictionary,
            ref List<int> jmpList)
        {
            var accumulator = 0;
            var instructionNumber = 0;
            var answer = 0;
            var usedInstructionNumbers = new List<int>();
            do
            {
                var type = instructionsDictionary[instructionNumber].Item1;
                var movement = instructionsDictionary[instructionNumber].Item2;
                usedInstructionNumbers.Add(instructionNumber);
                switch (type)
                {
                    case "acc":
                        instructionNumber++;
                        if (usedInstructionNumbers.Exists(e => instructionNumber == e))
                        {
                            answer = accumulator;
                            break;
                        }

                        accumulator += movement;
                        break;
                    case "nop":
                        instructionNumber++;
                        break;
                    case "jmp":
                        instructionNumber += movement;
                        jmpList.Add(instructionNumber);
                        break;
                }
            } while (answer == 0);
            return answer;
        }
        
        private static ImmutableDictionary<int,Tuple<string,int>> BuildDictionary()
        {
            var counter = 0;
            var path = $"{SD.Path}8{SD.Ext}";
            return File.ReadLines(path).ToImmutableDictionary(
                instruction => counter++,
                instruction => new Tuple<string, int>(
                    instruction.Substring(0, 3),
                    int.Parse(instruction[4..])));
        }
    }
}
