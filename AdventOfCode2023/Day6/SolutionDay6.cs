namespace AdventOfCode2023.Day6
{
    public static class SolutionDay6
    {
        public static int Solve()
        {
            var lines = File.ReadAllLines("Day6/input.txt");

            var TimesRaw = lines[0].Split(':')[1];
            var DistancesRaw = lines[1].Split(':')[1];

            var times = TimesRaw.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var distances = DistancesRaw.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var result = 1;

            for (int i = 0; i < times.Length; i++)
            {
                var time = times[i];
                var distance = distances[i];

                result *= GetNumberOfSolutions(long.Parse(time), long.Parse(distance));
            }

            return result;
        }

        public static int SolveGold()
        {
            var lines = File.ReadAllLines("Day6/input.txt");

            var TimesRaw = lines[0].Split(':')[1];
            var DistancesRaw = lines[1].Split(':')[1];

            var result = 1;
            var time = long.Parse(TimesRaw.Replace(" ", ""));
            var distance = long.Parse(DistancesRaw.Replace(" ", ""));
            result *= GetNumberOfSolutions(time, distance);

            return result;
        }

        private static int GetNumberOfSolutions(long time, long distance)
        {
            bool checkLeft = true, checkRight = true;

            long middle = time/2;

            long lastLeft = GetResult(middle, time), lastRight = GetResult(middle, time);

            var sum = 0;

            if (lastLeft > distance) sum++;

            for (long i = 1; i < time && (checkLeft || checkRight); i++)
            {
                if (checkLeft && middle - i > 0)
                {
                    var result = GetResult(middle - i, time);
                    if (result > lastLeft) lastLeft = result;
                    else if (result < distance) checkLeft = false;

                    if (result > distance) sum++;
                }
                if (checkRight && middle + i < time)
                {
                    var result = GetResult(middle + i, time);
                    if (result > lastRight) lastRight = result;
                    else if (result < distance) checkRight = false;

                    if (result > distance) sum++;
                }
            }

            return sum;
        }

        private static long GetResult(long buttonPresed, long time)
        {
            long speed = buttonPresed;
            long distance = (time - buttonPresed) * speed;

            return distance;
        }
    }
}
