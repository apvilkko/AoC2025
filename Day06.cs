using System.Text;

namespace AoC2025;

internal class Day06 : IAocRunner
{
    public override async Task Run(string variant)
    {
        var lines = File.ReadLines($"inputs/{variant}06.txt").Where(x => x.Length > 0).ToList();

        var results = new List<long>();
        var values = new List<long[]>();
        var operations = new List<string>();
        foreach (var line in lines)
        {
            var tokens = line.Split(' ')
                .Select(x => x.Trim())
                .Where(x => x.Length > 0).ToList();

            if (!line.Contains('+'))
            {
                values.Add(tokens.Select(x => long.Parse(x)).ToArray());
            }
            else
            {
                operations = tokens;
                for (var i = 0; i < tokens.Count; ++i)
                {
                   results.Add(operations[i] == "*" ? 1 : 0); 
                }
            
            }
        }
        for (var r = 0; r < values.Count; ++r)
        {
            for (var i = 0; i < values[r].Length; ++i)
            {
                var value = values[r][i];
                //Console.WriteLine($"result is before {i} {results[i]}");
                switch (operations[i])
                {
                    case "+": { results[i] += value; break; }
                    case "-": { results[i] -= value; break; }
                    case "*": { results[i] *= value; break; }
                    case "/": { results[i] /= value; break; }
                }
                //Console.WriteLine($"result is now {i} {results[i]}");
            }
        }

        Console.WriteLine($"Part 1 sum {results.Sum()}");

        var operationsLine = lines[lines.Count-1];
        var indexes = operationsLine
            .ToCharArray()
            .Select((x, i) => new {Item=x, Index=i})
            .Where(o => o.Item != ' ')
            .Select(x => x.Index).ToList();
        Console.WriteLine($"{string.Join(", ",indexes)}");

        results.Clear();

        for (var i = 0; i < indexes.Count; ++i)
        {
            var index = indexes[i];
            var nextIndex = i == indexes.Count - 1 ? operationsLine.Length + 1 : indexes[i+1];
            var numbers = new List<long>();
            Console.WriteLine($"reading from {i} {index}-{nextIndex-2}");
            for (var ii = nextIndex-2; ii >= index; --ii)
            {
                var sb = new StringBuilder();
                foreach (var line in lines)
                {
                    if (line.Contains("+")) break;
                    if (line[ii] != ' ') {
                        sb.Append(line[ii]);
                    }
                }
                var value = long.Parse(sb.ToString());
                Console.WriteLine($"got value {value} from {ii}");
                numbers.Add(value);
            }
            Console.WriteLine($"operation: {operationsLine[index]}");
            var result = operationsLine[index] == '+' ? numbers.Sum() : numbers.Aggregate(1L, (a,b) => a*b);
            results.Add(result);
        }
        Console.WriteLine($"Part 2 sum {results.Sum()}");

    }
}
