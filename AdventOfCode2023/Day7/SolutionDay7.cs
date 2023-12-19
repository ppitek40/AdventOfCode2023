namespace AdventOfCode2023.Day7
{
    public static class SolutionDay7
    {
        public static UInt32 Solve()
        {
            var lines = File.ReadAllLines("Day7/input.txt");

            var sets = new List<Set>();

            foreach (var line in lines)
            {
                var values = line.Split(' ');
                sets.Add(new Set(values[0].Replace('A', 'Z').Replace('K', 'W').Replace('T', 'A'), values[1]));
            }

            var sortedSets = sets.OrderBy(s => s).ToList();
            UInt32 sum = 0;

            for (int i = 0; i < sortedSets.Count; i++)
            {
                sum += (UInt32)(sortedSets[i].Bid * (i+1));
            }

            return sum;
        }

        public static UInt32 SolveGold()
        {
            var lines = File.ReadAllLines("Day7/input.txt");

            var sets = new List<SetGold>();

            foreach (var line in lines)
            {
                var values = line.Split(' ');
                sets.Add(new SetGold(values[0].Replace('A', 'Z').Replace('K', 'W').Replace('T', 'A').Replace('J', '0'), values[1]));
            }

            var sortedSets = sets.OrderBy(s => s).ToList();
            UInt32 sum = 0;

            for (int i = 0; i < sortedSets.Count; i++)
            {
                sum += (UInt32)(sortedSets[i].Bid * (i+1));
            }

            return sum;
        }
    }

    public class Set : IComparable
    {
        public string Cards { get; set; }
        public int Bid { get; set; }

        public int Rank { get; set; }

        public Set(string cards, string bid)
        {
            Cards = cards;
            Bid = int.Parse(bid);
            Rank = (int)GetRank();
        }

        public int CompareTo(object? obj)
        {
            if (obj is not Set set || obj == null)
                return 1;

            if (this.Rank > set.Rank) return 1;
            if (this.Rank < set.Rank) return -1;

            for (int i = 0; i < Cards.Length; i++)
            {
                if (Cards[i] == set.Cards[i]) continue;

                if (char.IsDigit(Cards[i]) == char.IsDigit(set.Cards[i])) return Cards[i].CompareTo(set.Cards[i]);

                if (char.IsDigit(Cards[i])) return -1;

                return 1;
            }

            return 0;
        }

        public Ranks GetRank()
        {
            var cards = Cards.ToCharArray().GroupBy(c => c).Select(group => Tuple.Create(group.Key, group.Count())).OrderByDescending(g => g.Item2).ToList();

            if (cards[0].Item2 == 5) return Ranks.FiveOfKind;
            if (cards[0].Item2 == 4) return Ranks.FourOfKind;
            if (cards[0].Item2 == 3) return cards[1].Item2 == 2 ? Ranks.FullHouse : Ranks.ThreeOfKind;
            if (cards[0].Item2 == 2) return cards[1].Item2 == 2 ? Ranks.TwoPairs : Ranks.Pair;

            return Ranks.HighCard;
        }
    }

    public class SetGold : IComparable
    {
        public string Cards { get; set; }
        public int Bid { get; set; }

        public int Rank { get; set; }

        public SetGold(string cards, string bid)
        {
            Cards = cards;
            Bid = int.Parse(bid);
            Rank = (int)GetRank();
        }

        public int CompareTo(object? obj)
        {
            if (obj is not SetGold set || obj == null)
                return 1;

            if (this.Rank > set.Rank) return 1;
            if (this.Rank < set.Rank) return -1;

            for (int i = 0; i < Cards.Length; i++)
            {
                if (Cards[i] == set.Cards[i]) continue;

                if (char.IsDigit(Cards[i]) == char.IsDigit(set.Cards[i])) return Cards[i].CompareTo(set.Cards[i]);

                if (char.IsDigit(Cards[i])) return -1;

                return 1;
            }

            return 0;
        }

        public Ranks GetRank()
        {
            var cards = Cards.ToCharArray().GroupBy(c => c).Select(group => Tuple.Create(group.Key, group.Count())).OrderByDescending(g => g.Item2).ToList();

            var numberOfJs = (cards.FirstOrDefault(c => c.Item1 == '0')?.Item2).GetValueOrDefault();

            if (cards[0].Item1 == '0' && cards[0].Item2 == 5) return Ranks.FiveOfKind;
            if (cards[0].Item1 == '0') cards.RemoveAt(0);

            if (cards[0].Item2 + numberOfJs == 5) return Ranks.FiveOfKind;
            if (cards[0].Item2 + numberOfJs == 4) return Ranks.FourOfKind;
            if (cards[0].Item2 + numberOfJs == 3) return cards[1].Item2 == 2 ? Ranks.FullHouse : Ranks.ThreeOfKind;
            if (cards[0].Item2 + numberOfJs == 2) return cards[1].Item2 == 2 ? Ranks.TwoPairs : Ranks.Pair;

            return Ranks.HighCard;
        }
    }

    public enum Ranks
    {
        HighCard = 0,
        Pair = 1,
        TwoPairs = 2,
        ThreeOfKind = 3,
        FullHouse = 4,
        FourOfKind = 5,
        FiveOfKind = 6
    }
}
