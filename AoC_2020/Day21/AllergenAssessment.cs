using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoC_2020.Helpers;

namespace AoC_2020.Day21
{
    public static class AllergenAssessment
    {
        public static void Day21()
        {
            var path = $"{SD.Path}21{SD.Ext}";
            var input = File.ReadAllText(path);

            var foods = GetFoods(input);
            var poisons = GetPoisons(foods);

            Console.WriteLine("Day 21");
            Console.WriteLine($"Number of times ingredients appear {GetDay21Part1(foods, poisons)}");
            Console.WriteLine($"My canonical dangerous ingredient list {string.Join(",", GetDay21Part2(poisons))}");
        }

        private static IEnumerable<string> GetDay21Part2(IEnumerable<(string Allergen, string Ingredient)> poisons)
        {
            var part2 = poisons
                .OrderBy(it => it.Allergen)
                .Select(it => it.Ingredient);
            return part2;
        }

        private static int GetDay21Part1(IEnumerable<(string[] Allergens, string[] Ingredients)> foods, IEnumerable<(string Allergen, string Ingredient)> poisons)
        {
            var part1 = foods
                .SelectMany(it => it.Ingredients)
                .Count(it => !poisons.Select(it => it.Ingredient).Contains(it));
            return part1;
        }

        private static IEnumerable<(string[] Allergens, string[] Ingredients)> GetFoods(string input)
        {
            var foods = Regex.Replace(input, @"\(|\)|\r", "")
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(" contains "))
                .Select(groups =>
                    (
                        Allergens: groups[1].Split(", "),
                        Ingredients: groups[0].Split(" ")
                    )
                );
            return foods;
        }

        private static IEnumerable<(string Allergen, string Ingredient)> GetPoisons(IEnumerable<(string[] Allergens, string[] Ingredients)> foods)
        {
            var poisons = foods
                .SelectMany(it => it.Allergens.Select(Allergen => (Allergen, it.Ingredients)))
                .GroupBy(
                    pair => pair.Allergen,
                    pair => pair.Ingredients.Select(it => it),
                    (Allergen, collection) =>
                        (Allergen, Ingredients: collection.Aggregate((acc, it) => acc.Intersect(it)))
                )
                .OrderBy(pair => pair.Ingredients.Count())
                .Aggregate(
                    Enumerable.Empty<(string Allergen, string Ingredient)>(),
                    (poisons, pair) =>
                        poisons.Concat(new[]
                        {
                            (
                                allergen: pair.Allergen,
                                ingredient:
                                pair.Ingredients
                                    .Except(poisons.Select(it => it.Ingredient))
                                    .First()
                            )
                        })
                );
            return poisons;
        }
    }
}