using Jamarino.IntervalTree;

namespace AdventOfCode2023.Day5
{
    public static class SolutionDay5
    {
        public static LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> SeedToSoil;
        public static LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> SoilToFertilizer;
        public static LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> FertilizerToWater;
        public static LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> WaterToLight;
        public static LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> LightToTemperature;
        public static LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> TemperatureToHumidity;
        public static LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> HumidityToLocation;

        public static UInt32 Solve()
        {
            var lines = File.ReadAllLines("Day5/input.txt");

            var seeds = GetSeeds(lines[0]);

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (line.StartsWith("seed-to-soil")) SeedToSoil = MapMapper(lines, i);
                if (line.StartsWith("soil-to-fertilizer")) SoilToFertilizer = MapMapper(lines, i);
                if (line.StartsWith("fertilizer-to-water")) FertilizerToWater = MapMapper(lines, i);
                if (line.StartsWith("water-to-light")) WaterToLight = MapMapper(lines, i);
                if (line.StartsWith("light-to-temperature")) LightToTemperature = MapMapper(lines, i);
                if (line.StartsWith("temperature-to-humidity")) TemperatureToHumidity = MapMapper(lines, i);
                if (line.StartsWith("humidity-to-location")) HumidityToLocation = MapMapper(lines, i);
            }

            FillSeeds(seeds);

            return seeds.OrderBy(s => s.Location).ToList()[0].Location;
        }

        public static UInt32 SolveGold()
        {
            var lines = File.ReadAllLines("Day5/input.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (line.StartsWith("seed-to-soil")) SeedToSoil = MapMapper(lines, i);
                if (line.StartsWith("soil-to-fertilizer")) SoilToFertilizer = MapMapper(lines, i);
                if (line.StartsWith("fertilizer-to-water")) FertilizerToWater = MapMapper(lines, i);
                if (line.StartsWith("water-to-light")) WaterToLight = MapMapper(lines, i);
                if (line.StartsWith("light-to-temperature")) LightToTemperature = MapMapper(lines, i);
                if (line.StartsWith("temperature-to-humidity")) TemperatureToHumidity = MapMapper(lines, i);
                if (line.StartsWith("humidity-to-location")) HumidityToLocation = MapMapper(lines, i);
            }

            return GetSeedsGold(lines[0]);
        }
        private static void FillSeeds(IEnumerable<Seed> seeds)
        {
            foreach (var seed in seeds)
            {
                FillSeed(seed);
            }
        }

        private static void FillSeed(Seed seed)
        {
            //        seed.Soil = MapValue(SeedToSoil, seed.Id);
            //        seed.Fertilizer = MapValue(SoilToFertilizer, seed.Soil);
            //        seed.Water = MapValue(FertilizerToWater, seed.Fertilizer);
            //        seed.Light = MapValue(WaterToLight, seed.Water);
            //        seed.Temp = MapValue(LightToTemperature, seed.Light);
            //        seed.Humidity = MapValue(TemperatureToHumidity, seed.Temp);
            //        seed.Location = MapValue(HumidityToLocation, seed.Humidity);
        }

        private static UInt32 FillSeedGold(UInt32 seed)
        {
            return MapValue(HumidityToLocation, MapValue(TemperatureToHumidity, MapValue(LightToTemperature, MapValue(WaterToLight, MapValue(FertilizerToWater, MapValue(SoilToFertilizer, MapValue(SeedToSoil, seed)))))));
        }

        private static UInt32 MapValue(LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> mapper, UInt32 value)
        {
            var map = mapper.Query(value).SingleOrDefault();
            //var map = mapper.Where(m => m.Item2 <= value && value < m.Item2 + m.Item3).SingleOrDefault();

            if (map == default)
                return value;

            return map.Item1 + (value - map.Item2);
        }

        private static LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> MapMapper(string[] lines, int i)
        {
            LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)> maps = new LightIntervalTree<UInt32, (UInt32, UInt32, UInt32)>();

            for (int j = 1; i + j < lines.Length; j++)
            {
                if (string.IsNullOrEmpty(lines[i+j])) break;

                var values = lines[i+j].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var dest = UInt32.Parse(values[0]);
                var source = UInt32.Parse(values[1]);
                var range = UInt32.Parse(values[2]);

                maps.Add(source, source + range - 1, (dest, source, range));
            }

            return maps;
        }

        private static IEnumerable<Seed> GetSeeds(string line)
        {
            var seeds = new List<Seed>();

            var seedsRaw = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

            foreach (var seed in seedsRaw[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                seeds.Add(new Seed { Id = UInt32.Parse(seed) });
            }

            return seeds;
        }

        private static UInt32 GetSeedsGold(string line)
        {
            var seedsRaw = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

            var seedList = seedsRaw[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => UInt32.Parse(s)).ToList();

            UInt32 min = UInt32.MaxValue;

            for (int i = 0; i < seedList.Count(); i += 2)
            {
                for (UInt32 j = 0; j < seedList[i+1]; j++)
                {
                    var location = FillSeedGold(seedList[i]+j);
                    min = Math.Min(min, location);
                }
            }

            return min;
        }
    }

    public class Seed
    {
        public UInt32 Id { get; set; }
        public UInt32 Soil { get; set; }
        public UInt32 Fertilizer { get; set; }
        public UInt32 Water { get; set; }
        public UInt32 Light { get; set; }
        public UInt32 Temp { get; set; }
        public UInt32 Humidity { get; set; }
        public UInt32 Location { get; set; }
    }
}
