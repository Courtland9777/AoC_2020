using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020.Helpers
{
    public enum MovementDirection
    {
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3,
    }
    
    public class GridPoint
    {
        public int X { get; private init; }
        public int Y { get; private init; }
        public static readonly GridPoint Origin = new GridPoint(0, 0);

        private GridPoint()
        {
            X = 0;
            Y = 0;
        }

        public GridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPoint Move(MovementDirection direction, int d)
        {
            return direction switch
            {
                MovementDirection.Down => MoveDown(d),
                MovementDirection.Left => MoveLeft(d),
                MovementDirection.Right => MoveRight(d),
                MovementDirection.Up => MoveUp(d),
                _ => throw new Exception($"Invalid movement direction {direction}"),
            };
        }

        private GridPoint MoveRight(int d)
        {
            return new GridPoint
            {
                X = X + d,
                Y = Y
            };
        }
        private GridPoint MoveLeft(int d)
        {
            return new GridPoint
            {
                X = X - d,
                Y = Y
            };
        }
        private GridPoint MoveUp(int d)
        {
            return new GridPoint
            {
                X = X,
                Y = Y + d
            };
        }
        private GridPoint MoveDown(int d)
        {
            return new GridPoint
            {
                X = X,
                Y = Y - d
            };
        }

        public static MovementDirection GetMovementDirectionForAdjacentPoints(
            GridPoint startPoint,
            GridPoint endPoint,
            bool invertY = false)
        {
            if (!GetAreAdjacent(startPoint, endPoint))
                throw new Exception("Points are not adjacent");
            if (endPoint.X > startPoint.X)
                return MovementDirection.Right;
            if (endPoint.X < startPoint.X)
                return MovementDirection.Left;
            if (endPoint.Y > startPoint.Y)
                return invertY ? MovementDirection.Down : MovementDirection.Up;
            if (endPoint.Y < startPoint.Y)
                return invertY ? MovementDirection.Up : MovementDirection.Down;
            throw new Exception("Invalid state");
        }

        private static bool GetAreAdjacent(GridPoint p1, GridPoint p2)
        {
            return GetManhattanDistance(p1, p2) == 1;
        }

        /// <summary>
        /// Computes the manhattan distance between two points on the grid.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static int GetManhattanDistance(GridPoint p1, GridPoint p2)
        {
            return Math.Abs(p1.X - p2.X)
                + Math.Abs(p1.Y - p2.Y);
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var p = (GridPoint)obj;
            return (X == p.X) && (Y == p.Y);
        }

        public override int GetHashCode() =>
            Tuple.Create(X, Y).GetHashCode();

        public override string ToString() =>
            $"GridPoint({X}, {Y})";
    }
}
