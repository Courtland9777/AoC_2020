using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day12
{
    public static class RainRisk
    {
        public static void Day12()
        {
            var path = $"{SD.Path}12{SD.Ext}";

            Console.WriteLine("Day 12");
            Console.WriteLine($"Part 1 = {GetDay12Part1(path)}");
            Console.WriteLine($"Part 2 = {GetDay12Part2(path)}");
        }

        private static string GetDay12Part1(string path)
        {
            var lines = File.ReadLines(path);
            var ship = new Ship();
            foreach (var (op, val) in lines.Select(l => (l[0], int.Parse(l[1..]))))
            {
                switch (op)
                {
                    case 'F':
                        ship.MoveForward(val);
                        break;
                    case 'E':
                        ship.Move(0, val);
                        break;
                    case 'S':
                        ship.Move(1, val);
                        break;
                    case 'W':
                        ship.Move(2, val);
                        break;
                    case 'N':
                        ship.Move(3, val);
                        break;
                    case 'R':
                        ship.Rotate(val / 90);
                        break;
                    case 'L':
                        ship.Rotate(4 - val / 90);
                        break;
                }
            }

            return ship.ManhattanDistance();
        }

        private static string GetDay12Part2(string path)
        {
            var lines = File.ReadLines(path);
            var ship = new Ship();
            foreach (var (op, val) in lines.Select(l => (l[0], int.Parse(l[1..]))))
            {
                switch (op)
                {
                    case 'F':
                        ship.MoveForwardWaypoint(val);
                        break;
                    case 'E':
                        ship.MoveWaypoint(0, val);
                        break;
                    case 'S':
                        ship.MoveWaypoint(1, val);
                        break;
                    case 'W':
                        ship.MoveWaypoint(2, val);
                        break;
                    case 'N':
                        ship.MoveWaypoint(3, val);
                        break;
                    case 'R':
                        ship.RotateWaypoint(val / 90);
                        break;
                    case 'L':
                        ship.RotateWaypoint(4 - (val / 90));
                        break;
                }
            }

            return ship.ManhattanDistance();
        }
    }

    public class Ship
    {
        private int x { get; set; }
        private int y { get; set; }
        public int dir { get; set; }
        public int wayx { get; set; } = 10;
        public int wayy { get; set; } = 1;

        public void Move(int dir, int val)
        {
            (x, y) = dir switch
            {
                0 => (x + val, y),
                1 => (x, y - val),
                2 => (x - val, y),
                3 => (x, y + val),
                _ => (x, y)
            };
        }

        public void MoveWaypoint(int dir, int val)
        {
            (wayx, wayy) = dir switch
            {
                0 => (wayx + val, wayy),
                1 => (wayx, wayy - val),
                2 => (wayx - val, wayy),
                3 => (wayx, wayy + val),
                _ => (wayx, wayy)
            };
        }

        public void MoveForward(int val)
        {
            Move(dir, val);
        }

        public void MoveForwardWaypoint(int val)
        {
            x += wayx * val;
            y += wayy * val;
        }

        public void Rotate(int val)
        {
            dir = (dir + val) % 4;
        }

        public void RotateWaypoint(int val)
        {
            (wayx, wayy) = val switch
            {
                0 => (wayx, wayy),
                1 => (wayy, -wayx),
                2 => (-wayx, -wayy),
                3 => (-wayy, wayx),
                _ => (wayx, wayy)
            };
        }

        public string ManhattanDistance()
        {
            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }
    }
}
