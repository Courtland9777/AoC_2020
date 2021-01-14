using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day25
{
    public static class ComboBreaker
    {
        public static void Day25()
        {
            Console.WriteLine("Day 25");
            Console.WriteLine($"Encryption key the handshake is trying to establish = {GetDay25Part1()}");
        }

        private static BigInteger GetDay25Part1()
        {
            var (item1, item2) = GetDay25Input();
            return DoorHelper.GetEncryptionKey(item1, item2);
        }

        private static Tuple<BigInteger, BigInteger> GetDay25Input()
        {
            var path = $"{SD.Path}25{SD.Ext}";
            if (!File.Exists(path))
            {
                throw new Exception($"Cannot locate file {path}");
            }

            var inputLines = File.ReadAllLines(path);
            return new Tuple<BigInteger, BigInteger>(
                BigInteger.Parse(inputLines[0]),
                BigInteger.Parse(inputLines[1]));
        }
    }
}
