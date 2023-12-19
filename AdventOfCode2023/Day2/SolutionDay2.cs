namespace AdventOfCode2023.Day2
{
    public static class SolutionDay2
    {
        public static (int, int) Solve()
        {
            var lines = File.ReadAllLines("Day2/input.txt");

            int sum = 0, powerSum = 0;

            foreach (var line in lines)
            {
                var groups = line.Split(':');

                var gameId = ReadGameId(groups[0]);

                bool result = true;

                var minimalValues = (0, 0, 0);

                foreach (var set in groups[1].Split(';'))
                {
                    var values = ReadSetCubes(set);

                    minimalValues = SetMinimalValues(minimalValues, values);

                    if (!ValidateSet(values)) result = false;
                }

                powerSum += minimalValues.Item1 * minimalValues.Item2 * minimalValues.Item3;

                if (result) sum += int.Parse(gameId);
            }

            return (sum, powerSum);
        }

        private static (int, int, int) SetMinimalValues((int, int, int) minimalValues, IEnumerable<(int, string)> set)
        {
            foreach (var value in set)
            {
                if (value.Item2 == "red") minimalValues.Item1 = Math.Max(minimalValues.Item1, value.Item1);
                if (value.Item2 == "blue") minimalValues.Item2 = Math.Max(minimalValues.Item2, value.Item1);
                if (value.Item2 == "green") minimalValues.Item3 = Math.Max(minimalValues.Item3, value.Item1);
            }

            return minimalValues;
        }

        private static bool ValidateSet(IEnumerable<(int, string)> set)
        {
            var result = true;

            foreach (var item in set)
            {
                if (item.Item2 == "red") result &= item.Item1 <= 12;
                if (item.Item2 == "blue") result &= item.Item1 <= 14;
                if (item.Item2 == "green") result &= item.Item1 <= 13;
            }

            return result;
        }

        private static IEnumerable<(int, string)> ReadSetCubes(string set)
        {
            var result = new List<(int, string)>();
            var cubes = set.Split(',');

            foreach (var cube in cubes)
            {
                var values = cube.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var number = int.Parse(values[0]);
                result.Add((number, values[1]));
            }

            return result;
        }

        private static string ReadGameId(string group)
        {
            return group.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
        }
    }
}
