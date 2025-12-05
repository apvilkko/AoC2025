namespace AoC2025;

internal class Day05 : IAocRunner
{
    public record Range(long Start, long End);

    private List<Range> MergeRanges(List<Range> ranges)
    {
        List<Range> newRanges = [];
        var removed = new HashSet<int>();
        for (var i = 0; i < ranges.Count; ++i)
        {
            if (removed.Contains(i)) continue;
            var current = ranges[i];
            
            for (var j = 0; j < ranges.Count; ++j)
            {
                if (removed.Contains(j) || i == j) continue;
                
                var other = ranges[j];

                // current   i     i
                // other   i     i
                //
                // current   i i
                // other   i     i
                var case12 = current.Start >= other.Start && other.End >= current.Start;

                // current i     i
                // other     i     i
                //
                // current i     i
                // other     i i
                var case34 = other.Start >= current.Start && current.End >= other.Start;

                if (case12 || case34)
                {
                    var start = Math.Min(current.Start, other.Start);
                    var end = Math.Max(current.End, other.End);
                    newRanges.Add(new Range(start, end));
                    removed.Add(j);
                    removed.Add(i);
                }
            }
        }

        for (var i = 0; i < ranges.Count; ++i)
        {
            if (!removed.Contains(i))
            {
                newRanges.Add(ranges[i]);
            }
        }
        return newRanges;
    }

    public override async Task Run(string variant)
    {
        var lines = File.ReadLines($"inputs/{variant}05.txt");
        var ranges = new List<Range>();
        var ids = new List<long>();
        foreach (var line in lines)
        {
            var parts = line.Split('-');
            if (parts.Length == 2)
            {
                ranges.Add(new Range(long.Parse(parts[0]), long.Parse(parts[1])));
            }
            else if (parts.Length == 1 && line.Length > 0)
            {
                ids.Add(long.Parse(parts[0]));
            }

        }
        foreach (var range in ranges)
        {
            Console.WriteLine($"Range: {range.Start}-{range.End}");
        }
        foreach (var n in ids)
        {
            Console.WriteLine($"ID: {n}");
        }

        var numFresh = ids.Aggregate(0, (acc, id) =>
        {
            var isFresh = ranges.Any(r => id >= r.Start && id <= r.End);
            return acc + (isFresh ? 1 : 0);
        });
        Console.WriteLine($"Part 1: {numFresh}");

        // Part 2
        int rangesBefore = ranges.Count;
        int rangesAfter = 0;
        while (rangesBefore != rangesAfter) {
            rangesBefore = ranges.Count;
            var newRanges = MergeRanges(ranges);
            rangesAfter = newRanges.Count;
            ranges = newRanges;

            Console.WriteLine("\nMerged ranges:");
            foreach (var range in newRanges)
            {
                Console.WriteLine($"Range: {range.Start}-{range.End}");
            }
        }

        var totalFresh = ranges.Aggregate(0L, (acc, range) => acc + range.End - range.Start + 1);
        Console.WriteLine($"Part 2: {totalFresh}");
    }
}