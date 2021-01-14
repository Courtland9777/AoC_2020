using AoC_2020.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2020.Day4
{
    public static class PassportProcessing
    {
        public static void Day4()
        {
            string[] requiredFields = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            var path = $"{SD.Path}4{SD.Ext}";
            var passportData = ConcatImportData.ConcatPassportData(File.ReadLines(path));

            Console.WriteLine("Day 4");
            Console.WriteLine($"Number of Valid Passports = {GetDay4Part1(passportData, requiredFields)}");
            Console.WriteLine($"Number of Passports with Valid Inputs = {GetDay4Part2(passportData, requiredFields)}");
        }
        
        private static int GetDay4Part1(IEnumerable<string> passportData, string[] requiredFields) =>
            CountValidPassports(passportData, requiredFields);

        private static int GetDay4Part2(IEnumerable<string> passportData, string[] requiredFields) =>
            CountPassportsWithValidInputValues(passportData, requiredFields);
        

        private static int CountPassportsWithValidInputValues(IEnumerable<string> passportData, string[] requiredFields) =>
            RemovePassportsMissingFields(passportData, requiredFields)
                .Count(passport => IsPassportInputValid(passport.Split(' ')
                .Select(field => field.Split(':'))
                .ToDictionary(parsedField => parsedField[0], parsedField => parsedField[1])));

        private static bool IsPassportInputValid(IReadOnlyDictionary<string, string> passport)
        {
            string[] possibleEyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            return
                IsBirthYearValid(int.Parse(passport["byr"])) &&
                IsIssueYearValid(int.Parse(passport["iyr"])) &&
                IsExpirationYearValid(int.Parse(passport["eyr"])) &&
                IsHeightValid(passport["hgt"]) &&
                passport["hcl"].Length == 7 &&
                passport["hcl"][0] == '#' &&
                passport["hcl"][1..].All(char.IsLetterOrDigit) &&
                possibleEyeColors.Any(c => passport["ecl"].Equals(c)) &&
                passport["pid"].Length == 9 && passport["pid"].All(char.IsDigit);
        }

        private static bool IsHeightValid(string hgt)
        {
            if (!hgt.Contains("in") && (!hgt.Contains("cm"))) return false;
            var heightAsInt = int.Parse(hgt[0..^2]);
            if (hgt.Contains("in"))
            {
                return heightAsInt >= 59 && heightAsInt <= 76;
            }
            return heightAsInt >= 150 && heightAsInt <= 193;
        }

        private static bool IsExpirationYearValid(int expirationYear) =>
            expirationYear >= 2020 && expirationYear <= 2030;
        private static bool IsIssueYearValid(int issueYear) =>
            issueYear >= 2010 && issueYear <= 2020;
        private static bool IsBirthYearValid(int birthYear) =>
            birthYear >= 1920 && birthYear <= 2002;

        private static IEnumerable<string> RemovePassportsMissingFields(IEnumerable<string> passportData, string[] requiredFields) =>
            passportData.Where(line => requiredFields.All(line.Contains));

        private static int CountValidPassports(IEnumerable<string> passportData, string[] requiredFields) =>
            passportData.Count(line => requiredFields.All(line.Contains));
    }
}
