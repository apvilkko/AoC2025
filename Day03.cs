namespace AoC2025;

internal class Day03 : IAocRunner
{

    private int FindMaxJoltage(int[] bank)
    {
        var sorted = bank.OrderBy(x => x).Reverse().ToList();
        var candidate = 0;

        for (var i = 0; i < 3; ++i)
        {
            var index = Array.FindIndex(bank, x => x == sorted[i]);
            if (index == bank.Length - 1)
            {
                continue;
            }
            var subarray = bank[(index+1)..^0];
            var maxsub = subarray.Max();
            //Console.WriteLine($"testing {i} {sorted[i]} found at {index} max2 {maxsub}");
            var newCandidate = int.Parse($"{sorted[i]}{maxsub}");
            if (newCandidate > candidate)
            {
                candidate = newCandidate;
            }
        }
        
        return candidate;
    }

    private record SelectedNumber(int Index, int Value);

    private Int64 FindMaxJoltage2(int[] bank)
    {
        var amount = 12;
        var selected = new List<SelectedNumber>();
        
        while (selected.Count < amount)
        {
            var alreadyUsed = selected.Select(x => x.Index).ToList();
            int startFrom = Math.Min(bank.Length - amount + selected.Count, bank.Length - 1);
            int max = 0;
            int index = -1;
            for (int i = startFrom; i >= 0; --i)
            {
                if (alreadyUsed.Contains(i))
                {
                    //Console.WriteLine($"  already used {i}");
                    break;
                }
                if (bank[i] >= max)
                {
                    max = bank[i];
                    index = i;
                    //Console.WriteLine($"    setting {max} at {index}");
                }
            }
            //Console.WriteLine($"  found {max} at {index}, started from {startFrom}");
            selected.Add(new SelectedNumber(index, max));
        }
        
        return Int64.Parse(string.Join("", selected.Select(x => x.Value)));
    }

    public override async Task Run(string variant)
    {
        
        var banks = File.ReadLines($"inputs/{variant}03.txt")
            .Where(line => line.Length > 0)
            .Select(line => line
                .ToCharArray()
                .Select(s => s - '0')
                .ToArray())
            .ToList();

        Int64 sum = 0;
        foreach (var bank in banks)
        {
            // part 1
            //var joltage = FindMaxJoltage(bank);
            
            // part 2
            var joltage = FindMaxJoltage2(bank);
            
            Console.WriteLine($"{string.Join("", bank)} max {joltage}");
            sum += joltage;
        }

        Console.WriteLine($"Sum: {sum}");

    }
}