namespace AdventOfCode2023.Day8
{
    public static class SolutionDay8
    {
        public static int Solve()
        {
            var lines = File.ReadAllLines("Day8/input.txt");

            var moves = lines[0];

            IDictionary<string, (string, string)> dict = new Dictionary<string, (string, string)>();

            foreach (var line in lines.Skip(2))
            {
                var groups = line.Split('=');

                var place = groups[0].Trim();

                var directions = groups[1].Split(',');

                var left = directions[0].Replace("(", "").Trim();
                var right = directions[1].Replace(")", "").Trim();

                dict.Add(place, (left, right));
            }

            var sum = 0;
            var actualPlace = "AAA";

            while (actualPlace != "ZZZ")
            {
                foreach (var ch in moves)
                {
                    var directions = dict[actualPlace];
                    if (ch == 'L') actualPlace = directions.Item1;
                    if (ch == 'R') actualPlace = directions.Item2;

                    sum++;

                    if (actualPlace == "ZZZ") break;
                }
            }

            return sum;
        }

        public static UInt64 SolveGold()
        {
            var lines = File.ReadAllLines("Day8/input.txt");

            var moves = lines[0];

            IDictionary<string, (string, string)> dict = new Dictionary<string, (string, string)>();

            foreach (var line in lines.Skip(2))
            {
                var groups = line.Split('=');

                var place = groups[0].Trim();

                var directions = groups[1].Split(',');

                var left = directions[0].Replace("(", "").Trim();
                var right = directions[1].Replace(")", "").Trim();

                dict.Add(place, (left, right));
            }

            var sums = new List<UInt64>();
            var actualPlaces = dict.Where(d => d.Key.EndsWith('A')).Select(d => d.Key).ToList();

            for (int i = 0; i < actualPlaces.Count; i++)
            {
                UInt64 sum = 0;
                while (!actualPlaces[i].EndsWith('Z'))
                {
                    foreach (var ch in moves)
                    {
                        var directions = dict[actualPlaces[i]];
                        if (ch == 'L') actualPlaces[i] = directions.Item1;
                        if (ch == 'R') actualPlaces[i] = directions.Item2;

                        sum++;

                        if (actualPlaces[i].EndsWith('Z')) break;
                    }
                }
                sums.Add(sum);
            }

            return GetLCM(sums.ToArray());
        }

        private static UInt64 gcd(UInt64 n1, UInt64 n2)
        {
            if (n2 == 0)
            {
                return n1;
            }
            else
            {
                return gcd(n2, n1 % n2);
            }
        }

        private static UInt64 GetLCM(UInt64[] numbers)
        {
            return numbers.Aggregate((S, val) => S * val / gcd(S, val));
        }
    }
}
