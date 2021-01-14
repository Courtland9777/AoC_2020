using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day11
{
    public static class SeatingSystem
    {
        public static void Day11()
        {
            var path = $"{SD.Path}11{SD.Ext}";
            var initialSeatingChart = File.ReadLines(path).ToArray();

            Console.WriteLine($"Final seat count = {GetDay11Part1(initialSeatingChart)}");
            Console.WriteLine($"Final seat count Part 2 = {GetDay11Part2(initialSeatingChart)}");
        }

        private static int GetDay11Part1(IEnumerable<string> initialSeatingChart)
        {
            var paddedChart = BuildPaddedList(initialSeatingChart);
            string[] newPaddedChart = null;
            var sb = new StringBuilder();
            var updatedSeatingChart = new List<string>();

            do
            {
                if (newPaddedChart != null)
                {
                    Array.Clear(paddedChart, 0, paddedChart.Length);
                    paddedChart = CopyArray(newPaddedChart);
                    Array.Clear(newPaddedChart, 0, newPaddedChart.Length);
                }
                for (var row = 1; row < paddedChart.Length - 1; row++)
                {
                    for (var colIndex = 1; colIndex < paddedChart[0].Length - 1; colIndex++)
                    {
                        var checkString = $"{paddedChart[row - 1][colIndex - 1]}" +
                                          $"{paddedChart[row - 1][colIndex]}" +
                                          $"{paddedChart[row - 1][colIndex + 1]}" +
                                          $"{paddedChart[row][colIndex - 1]}" +
                                          $"{paddedChart[row][colIndex + 1]}" +
                                          $"{paddedChart[row + 1][colIndex - 1]}" +
                                          $"{paddedChart[row + 1][colIndex]}" +
                                          $"{paddedChart[row + 1][colIndex + 1]}";
                        var seatsOccupiedCount = checkString.Count(x => x == '#');

                        switch (paddedChart[row][colIndex])
                        {
                            case 'L':
                                sb.Append(seatsOccupiedCount < 1 ? '#' : 'L');
                                break;
                            case '#':
                                sb.Append(seatsOccupiedCount > 3 ? 'L' : '#');
                                break;
                            case '.':
                                sb.Append('.');
                                break;
                        }
                    }

                    updatedSeatingChart.Add(sb.ToString());
                    sb.Clear();
                }

                newPaddedChart = BuildPaddedList(updatedSeatingChart);
                updatedSeatingChart.Clear();
            } while (!paddedChart.SequenceEqual(newPaddedChart));

            return string.Concat(paddedChart).Count(x => x.Equals('#'));
        }
        
        private static int GetDay11Part2(IEnumerable<string> initialSeatingChart)
        {
            var paddedChart = BuildPaddedList(initialSeatingChart);
            string[] newPaddedChart = null;
            var sb = new StringBuilder();
            var updatedSeatingChart = new List<string>();

            do
            {
                if (newPaddedChart != null)
                {
                    Array.Clear(paddedChart, 0, paddedChart.Length);
                    paddedChart = CopyArray(newPaddedChart);
                    Array.Clear(newPaddedChart, 0, newPaddedChart.Length);
                }
                for (var row = 1; row < paddedChart.Length - 1; row++)
                {
                    for (var colIndex = 1; colIndex < paddedChart[0].Length - 1; colIndex++)
                    {
                        var checkString = $"{CheckDirection(paddedChart,row,-1,colIndex,-1)}" +
                                          $"{CheckDirection(paddedChart, row, -1, colIndex, 0)}" +
                                          $"{CheckDirection(paddedChart, row, -1, colIndex, 1)}" +
                                          $"{CheckDirection(paddedChart, row, 0, colIndex, -1)}" +
                                          $"{CheckDirection(paddedChart, row, 0, colIndex, 1)}" +
                                          $"{CheckDirection(paddedChart, row, 1, colIndex, -1)}" +
                                          $"{CheckDirection(paddedChart, row, 1, colIndex, 0)}" +
                                          $"{CheckDirection(paddedChart, row, 1, colIndex, 1)}";
                        var seatsOccupiedCount = checkString.Count(x => x == '#');

                        switch (paddedChart[row][colIndex])
                        {
                            case 'L':
                                sb.Append(seatsOccupiedCount < 1 ? '#' : 'L');
                                break;
                            case '#':
                                sb.Append(seatsOccupiedCount > 4 ? 'L' : '#');
                                break;
                            case '.':
                                sb.Append('.');
                                break;
                        }
                    }

                    updatedSeatingChart.Add(sb.ToString());
                    sb.Clear();
                }

                newPaddedChart = BuildPaddedList(updatedSeatingChart);
                updatedSeatingChart.Clear();
            } while (!paddedChart.SequenceEqual(newPaddedChart));

            return string.Concat(paddedChart).Count(x => x.Equals('#'));
        }

        private static char CheckDirection(IReadOnlyList<string> arr, int row,int rowIncrement, int column, int colIncrement)
        {
            char arrChar;
            do
            {
                row += rowIncrement;
                column += colIncrement;
                arrChar = arr[row][column];
            } while (arrChar == '.' && row > 0 && row < 99 && column > 0 && column < 99);

            return arrChar;
        }

        private static string[] CopyArray(IEnumerable<string> arr) =>
            arr.ToArray();

        private static string[] BuildPaddedList(IEnumerable<string> initialSeatingChart)
        {
            var list = new List<string>();
            var topAndBottomPadding = BuildPaddingString();

            list.Add(topAndBottomPadding);
            list.AddRange(initialSeatingChart.Select(row => $".{row}."));
            list.Add(topAndBottomPadding);
            return list.ToArray();
        }

        private static string BuildPaddingString()
        {
            var sb = new StringBuilder(100);
            for (var i = 0; i < 100; i++)
            {
                sb.Append('.');
            }

            return sb.ToString();
        }
    }
}
