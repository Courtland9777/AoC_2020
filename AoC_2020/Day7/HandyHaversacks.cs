using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoC_2020.Helpers;
using static System.Text.RegularExpressions.RegexOptions;
using Bags = System.Collections.Immutable.ImmutableDictionary<string, int>;

namespace AoC_2020.Day7
{
    public static class HandyHaversacks
    {
        public static void Day7()
        {
            Console.WriteLine("Day 7");
            const string myBag = "shiny gold";
            var path = $"{SD.Path}7{SD.Ext}";
            var rules = File.ReadLines(path)
                .ToImmutableDictionary(
                    line => Regex.Match(line, @"^(\w+ \w+)", Compiled).Groups[1].Value,
                    line => line.Contains("no other bags.")
                        ? Bags.Empty
                        : Regex.Matches(line, @"(\d+) (\w+ \w+) bags?[,.]\s?", Compiled)
                            .ToImmutableDictionary(
                                match => match.Groups[2].Value,
                                match => int.Parse(match.Groups[1].Value)));
            Console.WriteLine("Day 7");
            Console.WriteLine($"Container Bag Count {GetDay7Part1(myBag, rules)}");
            Console.WriteLine($"Contained Bag Count {GetDay7Part2(myBag, rules)}");
        }
        private static int GetDay7Part1(string bag, ImmutableDictionary<string, Bags> rules)
        {
            bool IsBagContainedIn(Bags bags) =>
                bags.ContainsKey(bag) ||
                bags.Keys.Any(b => IsBagContainedIn(rules[b]));

            return rules.Values.Count(IsBagContainedIn);
        }

        private static int GetDay7Part2(string bag, ImmutableDictionary<string, Bags> rules) =>
            rules[bag].Sum(b => b.Value + (b.Value * GetDay7Part2(b.Key, rules)));
    }
}
