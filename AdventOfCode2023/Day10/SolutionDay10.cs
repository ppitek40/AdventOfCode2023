using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Position = (int, int);

namespace AdventOfCode2023.Day10
{
    public class SolutionDay10
    {
        private static Dictionary<char, char> L7InsideMap = new Dictionary<char, char>
        {
            {'L', 'D'},
            {'D', 'L'},
            {'R', 'T'},
            {'T', 'R'},
        };
        private static Dictionary<char, char> JFInsideMap = new Dictionary<char, char>
        {
            {'T', 'L'},
            {'L', 'T'},
            {'R', 'D'},
            {'D', 'R'},
        };
        private static Dictionary<char, char> Directions = new Dictionary<char, char>
        {
            {'N', 'S'},
            {'S', 'N'},
            {'E', 'W'},
            {'W', 'E'},
        };

        private static Dictionary<char, string> Pipes = new Dictionary<char, string>
        {
            {'7', "SW"},
            {'J', "NW"},
            {'L', "NE"},
            {'F', "SE"},
            {'|', "SN"},
            {'-', "EW"},
            {'.', ""},
        };

        public static long Solve()
        {
            var lines = File.ReadAllLines("Day10/input.txt");

            List<List<(char, int)>> matrix = new List<List<(char, int)>>();

            Position start = InitializeMatrix(lines, matrix);
            var direction = FindStartingDirection(matrix, start);

            Position current = start;
            long steps = 0;
            while (true)
            {
                current = Move(current, direction);
                var from = Directions[direction];
                var pipe = matrix[current.Item1][current.Item2];
                if (pipe.Item1 == 'S') break;
                direction = Pipes[pipe.Item1].First(c => c != from);
                steps++;
            }

            return (steps + 1)/2;
        }

        public static long SolveGold()
        {
            var lines = File.ReadAllLines("Day10/input.txt");

            List<List<(char, int)>> matrix = new List<List<(char, int)>>();

            Position start = InitializeMatrix(lines, matrix);
            var startingDirection = FindStartingDirection(matrix, start);
            var direction = startingDirection;

            Position current = start;
            long steps = 0;
            while (true)
            {
                current = Move(current, direction);
                var from = Directions[direction];
                var pipe = matrix[current.Item1][current.Item2];
                matrix[current.Item1][current.Item2] = (matrix[current.Item1][current.Item2].Item1, 1);
                if (pipe.Item1 == 'S')
                {
                    var key = Pipes.FirstOrDefault(p => p.Value == string.Join("", from, startingDirection) || p.Value == string.Join("", startingDirection, from)).Key;
                    matrix[current.Item1][current.Item2] = (key, 1);
                    break;
                }
                direction = Pipes[pipe.Item1].First(c => c != from);
                steps++;
            }

            FloodFill(matrix, 0, 0, false);

            var firstPipe = FindFirstPipe(matrix);
            var inside = 'R';
            current = firstPipe;
            direction = 'S';
            while (true)
            {
                current = Move(current, direction);
                var from = Directions[direction];
                if (current == firstPipe) break;
                var pipe = matrix[current.Item1][current.Item2];
                MarkInside(matrix, current, inside);
                inside = ConvertInside(pipe.Item1, inside);
                MarkInside(matrix, current, inside);
                direction = Pipes[pipe.Item1].First(c => c != from);
                steps++;
            }

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    if (matrix[i][j].Item2 == 2)
                    {
                        FloodFill(matrix, i, j, true, true);
                    }
                }
            }

            var count = matrix.Sum(row => row.Count(c => c.Item2 == 2));

            File.WriteAllLines("Day10/output.txt", matrix.Select(line => string.Join("", line.Select(l => l.Item2.ToString()))));

            return count;
        }

        private static void MarkInside(List<List<(char, int)>> matrix, Position pos, char inside)
        {
            var (row, col) = pos;

            int checkRow = 0, checkCol = 0;
            switch (inside)
            {
                case 'L':
                    checkRow = row;
                    checkCol = col-1;
                    break;
                case 'R':
                    checkRow = row;
                    checkCol = col+1;
                    break;
                case 'T':
                    checkRow = row-1;
                    checkCol = col;
                    break;
                case 'D':
                    checkRow = row+1;
                    checkCol = col;
                    break;
            }

            if (checkRow >= 0 && checkRow < matrix.Count && checkCol >= 0 && checkCol < matrix[checkRow].Count && matrix[checkRow][checkCol].Item2 == 0)
                matrix[checkRow][checkCol] = (matrix[checkRow][checkCol].Item1, 2);
        }

        private static Position FindFirstPipe(List<List<(char, int)>> matrix)
        {
            for (int row = 0; row < matrix.Count; row++)
            {
                for (int col = 0; col < matrix[row].Count; col++)
                {
                    if (matrix[row][col].Item2 == 1)
                    {
                        return (row, col);
                    }
                }
            }

            return (0, 0);
        }

        private static char ConvertInside(char pipe, char inside)
        {
            if (pipe == '|' || pipe == '-')
                return inside;

            if (pipe == 'L' || pipe == '7')
            {
                return L7InsideMap[inside];
            }
            if (pipe == 'J' || pipe == 'F')
            {
                return JFInsideMap[inside];
            }
            return inside;
        }

        private static void FloodFill(List<List<(char, int)>> matrix, int row, int col, bool inside, bool firstIgnore = false)
        {
            // Check if the current cell is inside the loop
            if (row >= 0 && row < matrix.Count && col >= 0 && col < matrix[row].Count && (firstIgnore || matrix[row][col].Item2 == 0))
            {
                matrix[row][col] = (matrix[row][col].Item1, inside ? 2 : 3);

                // Perform flood-fill in all four directions
                FloodFill(matrix, row - 1, col, inside); // Up
                FloodFill(matrix, row + 1, col, inside); // Down
                FloodFill(matrix, row, col - 1, inside); // Left
                FloodFill(matrix, row, col + 1, inside); // Right
            }
        }

        private static Position Move(Position current, char direction)
        {
            var (row, col) = current;

            switch (direction)
            {
                case 'N':
                    return (row - 1, col);
                case 'S':
                    return (row + 1, col);
                case 'E':
                    return (row, col + 1);
                case 'W':
                    return (row, col - 1);
                default:
                    throw new Exception("Invalid direction");
            }
        }

        private static char FindStartingDirection(List<List<(char, int)>> matrix, Position start)
        {
            var (row, col) = start;

            if (row > 0 && Pipes[matrix[row - 1][col].Item1].Contains(Directions['N']))
            {
                return 'N';
            }
            if (row < matrix.Count - 1 && Pipes[matrix[row + 1][col].Item1].Contains(Directions['S']))
            {
                return 'S';
            }
            if (col > 0 && Pipes[matrix[row][col - 1].Item1].Contains(Directions['W']))
            {
                return 'W';
            }
            if (col < matrix[row].Count - 1 && Pipes[matrix[row][col + 1].Item1].Contains(Directions['E']))
            {
                return 'E';
            }

            return ' ';
        }

        private static (int, int) InitializeMatrix(string[] lines, List<List<(char, int)>> matrix)
        {
            var start = (0, 0);

            foreach (var line in lines)
            {
                if (line.Contains('S'))
                {
                    start = (matrix.Count, line.IndexOf('S'));
                }
                matrix.Add(line.Select(c => (c, 0)).ToList());
            }

            return start;
        }
    }
}
