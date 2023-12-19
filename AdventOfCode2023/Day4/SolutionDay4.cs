namespace AdventOfCode2023.Day4
{
    public static class SolutionDay4
    {
        public static int Solve()
        {
            var lines = File.ReadAllLines("Day4/input.txt");

            var result = 0;

            foreach (var line in lines)
            {
                var groups = line.Split(':');

                var cardNumber = groups[0];

                var winningNumbers = groups[1].Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var chosenNumbers = groups[1].Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var correctNumbers = GetCorrectNumbers(winningNumbers, chosenNumbers);

                result += CalculatePoints(correctNumbers);
            }

            return result;
        }

        public static int SolveGold()
        {
            var lines = File.ReadAllLines("Day4/input.txt").Select(l => new LineWithCount { Line = l, Count = 1 }).ToArray();

            var result = 0;

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                var groups = line.Line.Split(':');

                var cardNumber = groups[0];

                var winningNumbers = groups[1].Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var chosenNumbers = groups[1].Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var correctNumbers = GetCorrectNumbers(winningNumbers, chosenNumbers);

                for (int j = 1; j <= correctNumbers; j++)
                {
                    if (i + j >= lines.Length) break;

                    lines[i + j].Count += 1 * line.Count;
                }
            }

            return lines.Sum(l => l.Count);
        }

        private static int GetCorrectNumbers(string[] winningNumbers, string[] chosenNumbers)
        {
            var correctNumbers = 0;

            foreach (var winningNumber in winningNumbers)
            {
                if (chosenNumbers.Contains(winningNumber)) correctNumbers++;
            }

            return correctNumbers;
        }

        private static int CalculatePoints(int correctNumbers)
        {
            return correctNumbers == 0 ? 0
                : correctNumbers == 1 ? 1
                : (int)Math.Pow(2, correctNumbers-1);
        }
    }

    public class LineWithCount
    {
        public string Line { get; set; }
        public int Count { get; set; }
    }
}
