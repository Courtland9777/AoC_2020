using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoC_2020.Helpers;
using MoreLinq;

namespace AoC_2020.Day16
{
    public enum State
    {
        RULES = 0,
        MY_TICKET = 1,
        OTHER_TICKETS = 2
    }
    
    public static class TicketTranslation
    {
        public static void Day16()
        {
            var path = $"{SD.Path}16{SD.Ext}";
            var input = File.ReadAllLines(path);
            var readState = State.RULES;
            var rules = new Dictionary<string, List<int>>();
            var myTicket = new List<int>();
            var validNearbyTicketsInts = new List<List<int>>();
            

            Console.WriteLine("Day 16");
            Console.WriteLine($"Part one solution:  {GetDay16Part1(input, readState, rules, ref myTicket, validNearbyTicketsInts)}");
            Console.WriteLine($"Part two solution:  {GetDay16Part2(rules, myTicket, validNearbyTicketsInts)}");
        }

        private static int GetDay16Part1(string[] input, State readState, Dictionary<string, List<int>> rules,
            ref List<int> myTicket, List<List<int>> validNearbyTicketsInts)
        {
            var count = 0;
            foreach (var line in input)
            {
                if (line?.Length == 0)
                {
                    var newState = (int) readState + 1;
                    readState = (State) newState;
                    continue;
                }

                switch (readState)
                {
                    case State.RULES:
                    {
                        const string regex = @"(.+?): (?<v1>\d+)-(?<v2>\d+) or (?<v3>\d+)-(?<v4>\d+)";
                        var match = Regex.Match(line, regex);
                        var ruleName = match.Groups[1].Value;
                        var v1 = int.Parse(match.Groups["v1"].Value);
                        var v2 = int.Parse(match.Groups["v2"].Value);
                        var range1 = Enumerable.Range(v1, v2 - v1 + 1).ToList();
                        var v3 = int.Parse(match.Groups["v3"].Value);
                        var v4 = int.Parse(match.Groups["v4"].Value);
                        var range2 = Enumerable.Range(v3, v4 - v3 + 1).ToList();
                        var fullRange = new List<int>();
                        fullRange.AddRange(range1);
                        fullRange.AddRange(range2);
                        rules.Add(ruleName, fullRange);
                        break;
                    }
                    case State.MY_TICKET when line.StartsWith("you"):
                        continue;
                    case State.MY_TICKET:
                        myTicket = Array.ConvertAll(line.Split(","), int.Parse).ToList();
                        break;
                    case State.OTHER_TICKETS when line.StartsWith("nearby"):
                        continue;
                    case State.OTHER_TICKETS:
                    {
                        var nearbyTickets = new List<int>(Array.ConvertAll(line.Split(","), s => Int32.Parse(s)));
                        if (GetInvalidField(nearbyTickets, rules))
                        {
                            validNearbyTicketsInts.Add(nearbyTickets);
                        }
                        else
                        {
                            count += GetInvalidFieldValue(nearbyTickets, rules);
                        }

                        break;
                    }
                }
            }

            return count;
        }

        private static long GetDay16Part2(Dictionary<string, List<int>> rules, List<int> myTicket, List<List<int>> validNearbyTicketsInts)
        {
            var indexes = new Dictionary<string, List<int>>();
            var founded = new List<int>();
            foreach (var (key, value) in rules)
            {
                founded = new List<int>();
                for (var i = 0; i < myTicket.Count; i++)
                {
                    var allValidTickets = validNearbyTicketsInts.Select(x => x.ElementAt(i)).ToList();
                    allValidTickets.Add(myTicket.ElementAt(i));
                    if (allValidTickets.All(x => value.Any(c => c == x)))
                    {
                        founded.Add(i);
                    }
                }

                indexes.Add(key, founded);
            }

            var final = new Dictionary<string, int>();
            var ordered = indexes.OrderBy(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
            var toRemove = new List<int>();
            foreach (var (key, value) in ordered)
            {
                if (value.Count == 1)
                {
                    toRemove.AddRange(value);
                    final.Add(key, value[0]);
                    continue;
                }

                var s = value.Except(toRemove).ToList();
                toRemove.Add(s[0]);
                final.Add(key, s[0]);
            }

            return final.Where(x => x.Key.Contains("departure")).Aggregate<KeyValuePair<string, int>, long>(1, (current, t) => current * myTicket.ElementAt(t.Value));
        }

        private static bool GetInvalidField(IEnumerable<int> nearbyTickets, Dictionary<string, List<int>> rules)
        {
            return nearbyTickets.All(field => rules.Values.Any(x => x.Any(c => c == field)));
        }

        private static int GetInvalidFieldValue(IEnumerable<int> nearbyTickets, Dictionary<string, List<int>> rules)
        {
            return nearbyTickets.FirstOrDefault(field => !rules.Values.Any(x => x.Any(c => c == field)));
        }

        
    }
}
