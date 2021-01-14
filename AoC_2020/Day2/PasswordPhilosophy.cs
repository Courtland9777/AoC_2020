using AoC_2020.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2020.Day2
{
    public static class PasswordPhilosophy
    {
        public static void Day2()
        {
            var path = $"{SD.Path}2{SD.Ext}";
            var passwordList = File.ReadLines(path);

            Console.WriteLine("Day 2");
            Console.WriteLine($"Number of Valid Passwords = {GetDay2Part1(passwordList)}");
            Console.WriteLine($"Number of Valid Passwords = {GetDay2Part2(passwordList)}");

        }
        private static bool ParsePasswordInfo(string password)
        {
            var pwSplit = password.Split(" ");
            var minMax = pwSplit[0].Split("-");
            return IsValidPw(int.Parse(minMax[0]), int.Parse(minMax[1]), pwSplit[1][0], pwSplit[2]);
        }

        private static bool ParsePasswordInfoPart2(string password)
        {
            var pwSplit = password.Split(" ");
            var minMax = pwSplit[0].Split("-");
            return IsValidPwPart2(int.Parse(minMax[0]), int.Parse(minMax[1]), pwSplit[1][0], pwSplit[2]);
        }

        private static int GetDay2Part2(IEnumerable<string> passwordList) => 
            passwordList.Count(ParsePasswordInfoPart2);
      

        private static int GetDay2Part1(IEnumerable<string> passwordList) =>
           passwordList.Count(ParsePasswordInfo);

        private static bool IsValidPw(int floor,int ceiling, char checkChar, string pwString)
        {
            var count = pwString.Count(checkChar.Equals);
            return count >= floor && count <= ceiling;
        }

        private static bool IsValidPwPart2(int floor, int ceiling, char checkChar, string pwString) =>
            (pwString[floor - 1] == checkChar) != (pwString[ceiling - 1] == checkChar);
    }
}

