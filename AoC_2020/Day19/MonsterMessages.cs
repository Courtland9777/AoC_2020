using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using AoC_2020.Extensions;
using AoC_2020.Helpers;
using MoreLinq;

namespace AoC_2020.Day19
{
    public static class MonsterMessages
    {
        public static void Day19()
        {
            var path = $"{SD.Path}19{SD.Ext}";
            var input = File.ReadAllText(path);

            Console.WriteLine("Day 19");
            Console.WriteLine($"Messages that completely match rule 0 = {GetDay19Part1(input)}");
            Console.WriteLine($"Messages that completely match rule 0 after updating rules 8 and 11 = {GetDay19Part2(input)}");
        }

        private static int GetDay19Part1(string input)
        {
            var rules = ParseRules(input);
            var messages = input.GetLines<string>()
                .SkipWhile(line => !string.IsNullOrWhiteSpace(line))
                .Skip(1);
            return messages.Count(message => RuleMatch(rules, message, 0).FirstOrDefault() == message.Length);
        }

        private static int GetDay19Part2(string input)
        {
            var rules = ParseRules(input);
            rules[8] = new[]
            {
                new[]
                {
                    new PointerRule(42)
                },
                new[]
                {
                    new PointerRule(42),
                    new PointerRule(8)
                }
            };
            rules[11] = new[]
            {
                new[]
                {
                    new PointerRule(42),
                    new PointerRule(31)
                },
                new[]
                {
                    new PointerRule(42),
                    new PointerRule(11),
                    new PointerRule(31)
                }
            };
            var messages = input.GetLines<string>()
                .SkipWhile(line => !string.IsNullOrWhiteSpace(line))
                .Skip(1);
            return messages.Count(message => RuleMatch(rules, message, 0).FirstOrDefault() == message.Length);
        }

        private static Dictionary<int, IEnumerable<IEnumerable<Rule>>> ParseRules(string input)
        {
            return input.GetLines<string>()
                .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var (id, rule, _) = line.Split(": ");
                    var combinations = rule.Split(" | ");
                    var entry = new KeyValuePair<int, IEnumerable<IEnumerable<Rule>>>(int.Parse(id),
                        combinations.Select(combination => combination.Split(" ")
                            .Select<string, Rule>(part =>
                                part.StartsWith(@"""") ? new CharRule(part[1]) : new PointerRule(int.Parse(part))
                            )
                        ));
                    return entry;
                })
                .ToDictionary();
        }

        private static IEnumerable<int> RuleMatch(IReadOnlyDictionary<int, IEnumerable<IEnumerable<Rule>>> rules, string message,
            int ruleId, int index = 0)
        {
            return rules[ruleId].SelectMany(rulesList =>
            {
                var positions = MoreEnumerable.Return(index);
                rulesList.ForEach(rule =>
                {
                    positions = positions.SelectMany(i =>
                    {
                        return i switch
                        {
                            _ when rule is CharRule charRule && message.TryGetCharAt(i) == charRule.Character => MoreEnumerable
                                .Return(i + 1),
                            _ when rule is PointerRule pointerRule => RuleMatch(rules, message, pointerRule.Id, i),
                            _ => Enumerable.Empty<int>()
                        };
                    });
                });
                return positions;
            });
        }
    }
    
    public abstract record Rule
    {
    }

    public sealed record CharRule(char Character) : Rule
    {
    }

    public sealed record PointerRule(int Id) : Rule
    {
    }
}

