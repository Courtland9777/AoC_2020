using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoC_2020.Helpers;
using MoreLinq;

namespace AoC_2020.Day14
{
    public static class DockingData
    {
        public static void Day14()
        {
            var path = $"{SD.Path}14{SD.Ext}";
            Regex regex = new(@"^(mask = (?<mask>[01X]{36}))|(mem\[(?<address>\d+)\] = (?<value>\d+))$");

            Console.WriteLine("Day 14");
            Console.WriteLine($"Sum of all values in memory upon completion v1 {GetDay14Part1(path, regex)}");
            Console.WriteLine($"Sum of all values in memory upon completion v2 {GetDay14Part2(path, regex)}");
        }

        private static ulong GetDay14Part1(string path, Regex regex)
        {
            var matches = File.ReadLines(path)
                .Select(l => regex.Match(l))
                .ToArray();

            var memory = new Dictionary<int, ulong>();
            var mask = string.Empty;
            foreach (var match in matches)
            {
                if (match.Groups["mask"].Success)
                {
                    mask = match.Groups["mask"].Value;
                }
                else
                {
                    var value = Convert.ToString(Convert.ToInt32(match.Groups["value"].Value), 2)
                        .PadLeft(36, '0');
                    var result =
                        Convert.ToUInt64(new string(value.Select((c, i) => mask[i] == 'X' ? c : mask[i]).ToArray()), 2);
                    var address = int.Parse(match.Groups["address"].Value);
                    memory[address] = result;
                }
            }

            return memory.Values.Aggregate((a, c) => a + c);
        }

        private static ulong GetDay14Part2(string path, Regex regex)
        {
            var matches = File.ReadLines(path)
                .Select(l => regex.Match(l))
                .ToArray();

            var memory = new Dictionary<ulong, ulong>();
            var mask = string.Empty;
            foreach (var match in matches)
            {
                if (match.Groups["mask"].Success)
                {
                    mask = match.Groups["mask"].Value;
                }
                else
                {
                    var address = Convert.ToString(Convert.ToInt32(match.Groups["address"].Value), 2)
                        .PadLeft(36, '0');
                    var result =
                        new string(address.Select((c, i) => mask[i] == '0' ? c : mask[i]).ToArray());
                    var addresses = GetAddresses(result);
                    var value = ulong.Parse(match.Groups["value"].Value);
                    foreach (var a in addresses)
                    {
                        memory[Convert.ToUInt64(a, 2)] = value;
                    }
                }
            }
            return memory.Values.Aggregate((a, c) => a + c);
        }

        private static IEnumerable<string> GetAddresses(string address)
        {
            var index = address.IndexOf('X');

            if (index == -1)
            {
                return MoreEnumerable.Return(address);
            }

            var replacements = new List<string>
            {
                address.Remove(index, 1).Insert(index, "0"),
                address.Remove(index, 1).Insert(index, "1")
            };
            return replacements.SelectMany(GetAddresses);
        }
    }

}

