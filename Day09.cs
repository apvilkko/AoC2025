namespace AoC2025;

internal class Day09 : IAocRunner
{
    public long GetLargestAreaNaive((long, long)[] points)
    {
        var minX = points.Min(p => p.Item1);
        var maxX = points.Max(p => p.Item1);
        var minY = points.Min(p => p.Item2);
        var maxY = points.Max(p => p.Item2);
        long area = 0;
        for (var i = 0; i < points.Length; ++i)
        {
            for (var j = 0; j < points.Length; ++j)
            {
                if (i == j)
                {
                    continue;
                }
                var dx = Math.Abs(points[i].Item1 - points[j].Item1) + 1;
                var dy = Math.Abs(points[i].Item2 - points[j].Item2) + 1;
                long thisArea = dx * dy;
                //Console.WriteLine($"Area between point {dx} {dy} {points[i]} and {points[j]}: {thisArea}");
                if (thisArea > area) {
                    area = thisArea;
                }
            }
        }
        return area;
    }

    public override async Task Run(string variant)
    {
        var points = File.ReadLines($"inputs/{variant}09.txt")
            .Where(x => x.Length > 0)
            .Select(line => line.Split(",")
                .Select(long.Parse)
                .ToArray()
            )
            .Select<long[], (long, long)>(x => new(x[0], x[1]))
            .ToArray();

        var area = GetLargestAreaNaive(points);
        Console.WriteLine($"Largest area: {area}");
    }
}
