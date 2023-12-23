
namespace AdventOfCode2023.Day9
{
    public static class SolutionDay9
    {
        public static int Solve()
        {
            var lines = File.ReadAllLines("Day9/input.txt");

            var sum = 0;

            foreach (var line in lines)
            {
                int[] baseValues = line.Split(' ').Select(int.Parse).ToArray();

                var listOfValues = new List<int[]> { baseValues };

                var current = baseValues;

                while (current.Any(v => v != 0))
                {
                    current = SubtractNeighborValues(current);
                    listOfValues.Add(current);
                }

                var newValues = CalculateNextValues(listOfValues);

                sum += newValues.Last();
            }

            return sum;
        }

        public static int SolveGold()
        {
            var lines = File.ReadAllLines("Day9/input.txt");

            var sum = 0;

            foreach (var line in lines)
            {
                int[] baseValues = line.Split(' ').Select(int.Parse).ToArray();

                var listOfValues = new List<int[]> { baseValues };

                var current = baseValues;

                while (current.Any(v => v != 0))
                {
                    current = SubtractNeighborValues(current);
                    listOfValues.Add(current);
                }

                var newValues = CalculatePreviousValues(listOfValues);

                sum += newValues.Last();
            }

            return sum;
        }

        private static int[] SubtractNeighborValues(int[] values)
        {
            int[] result = new int[values.Length - 1];

            for (int i = 0; i < values.Length - 1; i++)
            {
                result[i] = values[i + 1] - values[i];
            }

            return result;
        }

        private static List<int> CalculateNextValues(List<int[]> listOfValues)
        {
            var newValues = new List<int>();

            for (int i = listOfValues.Count - 1; i >= 0; i--)
            {
                if (i == listOfValues.Count - 1)
                    newValues.Add(0);
                else
                {
                    var nextValue = newValues.Last() + listOfValues[i][listOfValues[i].Length - 1];
                    newValues.Add(nextValue);
                }
            }

            return newValues;
        }

        private static List<int> CalculatePreviousValues(List<int[]> listOfValues)
        {
            var newValues = new List<int>();

            for (int i = listOfValues.Count - 1; i >= 0; i--)
            {
                if (i == listOfValues.Count - 1)
                    newValues.Add(0);
                else
                {
                    var nextValue = listOfValues[i][0] - newValues.Last();
                    newValues.Add(nextValue);
                }
            }

            return newValues;
        }
    }
}