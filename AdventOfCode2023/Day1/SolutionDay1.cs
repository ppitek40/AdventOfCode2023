namespace AdventOfCode2023.Day1
{
    using System.Text;

    internal static class SolutionDay1
    {
        public static int Solve()
        {
            var lines = File.ReadAllLines("Day1/input.txt");

            var sum = 0;

            foreach (var line in lines)
            {
                bool checkLeft = true, checkRight = true;
                char[] lineNumber = new char[2];
                for (var i = 0; i < line.Length && (checkLeft || checkRight); i++)
                {
                    if (checkLeft && char.IsNumber(line[i]))
                    {
                        lineNumber[0] = line[i];
                        checkLeft = false;
                    }

                    if (checkRight && char.IsNumber(line[line.Length - 1 - i]))
                    {
                        lineNumber[1] = line[line.Length - 1 - i];
                        checkRight = false;
                    }
                }
                sum += int.Parse(new string(lineNumber));
            }
            return sum;
        }

        public static int SolveGold()
        {
            var lines = File.ReadAllLines("Day1/input.txt");

            var sum = 0;

            foreach (var rawLine in lines)
            {
                var lineForward = NormalizeLineToNumbers(rawLine, true);
                char[] lineNumber = new char[2];

                // Look for first number
                foreach (var ch in lineForward)
                {
                    if (char.IsNumber(ch))
                    {
                        lineNumber[0] = ch;
                        break;
                    }
                }

                //Look for the last number
                var lineBackwards = NormalizeLineToNumbers(rawLine, false);
                foreach (var ch in lineBackwards)
                {
                    if (char.IsNumber(ch))
                    {
                        lineNumber[1] = ch;
                        break;
                    }
                }

                sum += int.Parse(new string(lineNumber));
            }
            return sum;
        }

        public static KeyValuePair<string, int>[] TextToNumberDictionary = new KeyValuePair<string, int>[]
        {
            new KeyValuePair<string,int>( "one", 1 ),
            new KeyValuePair<string,int>("two", 2),
            new KeyValuePair<string,int>("three", 3),
            new KeyValuePair<string,int>("four", 4),
            new KeyValuePair<string,int>("five", 5),
            new KeyValuePair<string,int>("six", 6),
            new KeyValuePair<string,int>("seven", 7),
            new KeyValuePair<string,int>("eight", 8),
            new KeyValuePair<string,int>("nine", 9),
            new KeyValuePair<string,int>("zero", 0)
        };

        private static string NormalizeLineToNumbers(string rawline, bool forwards)
        {
            var finds = new List<(int, KeyValuePair<string, int>)>();
            var line = forwards ? rawline : new string(rawline.Reverse().ToArray());
            if (forwards)
            {
                foreach (var map in TextToNumberDictionary)
                {
                    var index = line.IndexOf(map.Key);
                    if (index > -1)
                        finds.Add((index, map));
                }
            }
            else
            {
                foreach (var map in TextToNumberDictionary)
                {
                    var index = line.IndexOf(new string(map.Key.Reverse().ToArray()));
                    if (index > -1)
                        finds.Add((index, map));
                }
            }

            return forwards ?
                finds.OrderBy(x => x.Item1).Aggregate(line, (current, value) => current.Replace(value.Item2.Key, value.Item2.Value.ToString())) :
                finds.OrderBy(x => x.Item1).Aggregate(line, (current, value) => current.Replace(new string(value.Item2.Key.Reverse().ToArray()), value.Item2.Value.ToString()));
        }

        public static String ReplaceAll(String str, KeyValuePair<String, int>[] map)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            StringBuilder result = new StringBuilder(str.Length);
            StringBuilder word = new StringBuilder(str.Length);
            Int32[] indices = new Int32[map.Length];

            for (Int32 characterIndex = 0; characterIndex < str.Length; characterIndex++)
            {
                Char c = str[characterIndex];
                word.Append(c);

                for (var i = 0; i < map.Length; i++)
                {
                    String old = map[i].Key;
                    if (word.Length - 1 != indices[i])
                        continue;

                    if (old.Length == word.Length && old[word.Length - 1] == c)
                    {
                        indices[i] = -old.Length;
                        continue;
                    }

                    if (old.Length > word.Length && old[word.Length - 1] == c)
                    {
                        indices[i]++;
                        continue;
                    }

                    indices[i] = 0;
                }

                Int32 length = 0, index = -1;
                Boolean exists = false;
                for (int i = 0; i < indices.Length; i++)
                {
                    if (indices[i] > 0)
                    {
                        exists = true;
                        break;
                    }

                    if (-indices[i] > length)
                    {
                        length = -indices[i];
                        index = i;
                    }
                }

                if (exists)
                    continue;

                if (index >= 0)
                {
                    String value = map[index].Value.ToString();
                    word.Remove(0, length);
                    result.Append(value);

                    if (word.Length > 0)
                    {
                        characterIndex -= word.Length;
                        word.Length = 0;
                    }
                }

                result.Append(word);
                word.Length = 0;
                for (int i = 0; i < indices.Length; i++)
                    indices[i] = 0;
            }

            if (word.Length > 0)
                result.Append(word);

            return result.ToString();
        }
    }
}
