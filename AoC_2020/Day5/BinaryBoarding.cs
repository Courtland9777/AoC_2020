using AoC_2020.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace AoC_2020.Day5
{
    public static class BinaryBoarding
    {
        
        

        public static void Day5()
        {
            var path = $"{SD.Path}5{SD.Ext}";
            var boardingPasses = File.ReadLines(path);
            var seatIdList = new List<int>();
            
            Console.WriteLine("Day 5");
            Console.WriteLine($"Highest seat id on a boarding pass = {GetDay5Part1(boardingPasses, ref seatIdList)}");
            Console.WriteLine($"My seat id = {GetDay5Part2(seatIdList)}");
        }

        private static int GetDay5Part2(List<int> seatIdList)
        {
            var result = 0;
            for (var i = 1; i < 127; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var seatId = (i * 8) + j;
                    if (!seatIdList.Exists(s => s == seatId - 1) ||
                        !seatIdList.Exists(s => s == seatId + 1) ||
                        seatIdList.Exists(s => s == seatId))
                    { continue; }
                    result = seatId;
                    break;
                }

                if (result != 0) break;
            }

            return result;
        }

        private static int GetDay5Part1(IEnumerable<string> boardingPasses,ref List<int> seatIdList)
        {
            var maxSeatId = 0;
            foreach (var boardingPass in boardingPasses)
            {
                var front = 0;
                var back = 127;
                var left = 0;
                var right = 7;
                foreach (var t in boardingPass)
                {
                    switch (t)
                    {
                        case 'F':
                            back = ((front + back+1) / 2) - 1;
                            break;
                        case 'B':
                            front = (front + back + 1) / 2;
                            break;
                        case 'L':
                            right = ((left + right + 1) / 2) - 1;
                            break;
                        case 'R':
                            left = (left + right + 1) / 2;
                            break;
                    }
                }
                var thisSeatId = (front * 8) + left;
                seatIdList.Add(thisSeatId);
                if (thisSeatId > maxSeatId) maxSeatId = thisSeatId;
            }
            return maxSeatId;
        }
    }
}
