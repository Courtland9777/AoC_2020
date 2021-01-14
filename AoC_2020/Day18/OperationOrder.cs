using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day18
{
    public static class OperationOrder
    {
        private const char Space = ' ';
        private const char Add = '+';
        private const char Multiply = '*';
        private const char OpenBracket = '(';
        private const char CloseBracket = ')';

        public static void Day18()
        {
            var path = $"{SD.Path}18{SD.Ext}";

            Console.WriteLine("Day 18");
            Console.WriteLine($"Sum of each line of homework = {GetDay18Part1(path)}");
            Console.WriteLine($"Sum of each line of homework w/ new rules = {GetDay18Part2(path)}");
        }

        private static string GetDay18Part1(string path)
        {
            return File.ReadLines(path)
                .Select(line => ReversePolishNotation(line, false))
                .Sum()
                .ToString(CultureInfo.InvariantCulture);
        }

        private static string GetDay18Part2(string path)
        {
            return File.ReadLines(path)
                .Select(line => ReversePolishNotation(line, true))
                .Sum()
                .ToString(CultureInfo.InvariantCulture);
        }

        private static double ReversePolishNotation(string line, bool part2)
        {
            var values = new Stack<double>();
            var ops = new Stack<char>();
            ops.Push(OpenBracket);

            foreach (var c in line)
            {
                switch (c)
                {
                    case Space:
                        break;
                    case Add:
                        (ops, values) = Calculate(ops, values, part2);
                        ops.Push(Add);
                        break;
                    case Multiply:
                        (ops, values) = Calculate(ops, values);
                        ops.Push(c);
                        break;
                    case OpenBracket:
                        ops.Push(c);
                        break;
                    case CloseBracket:
                        (ops, values) = Calculate(ops, values);
                        ops.Pop();
                        break;
                    default:
                        values.Push(char.GetNumericValue(c));
                        break;
                }
            }
            (_, values) = Calculate(ops, values);
            return values.Single();
        }

        private static (Stack<char> ops, Stack<double> values) Calculate(Stack<char> ops, Stack<double> values, bool part2 = false)
        {
            while (!(ops.Peek() == OpenBracket || (part2 && ops.Peek() == Multiply)))
            {
                switch (ops.Pop())
                {
                    case Add:
                        values.Push(values.Pop() + values.Pop());
                        break;
                    case Multiply:
                        values.Push(values.Pop() * values.Pop());
                        break;
                }
            }

            return (ops, values);
        }
    }
}
