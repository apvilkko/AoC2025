using System.Text;

namespace AoC2025;

internal class Day02 : IAocRunner
{
    public record Range(string Start, string End);

    private bool IsValid(Int64 number, int divisor = 2)
    {
        //Console.WriteLine($"Checking number: {number} divisor: {divisor}");
        var str = number.ToString();

        if (str.Length == 1) {
            return true;
        }

        if (str.Length % divisor != 0)
        {
            return true;
        }

        if (divisor == 1)
        {
            var parts = str.ToCharArray();
            var invalid = parts.All(c => c == parts[0]);
            if (invalid)
            {
                Console.WriteLine($"   invalid 1 {string.Join(",", parts)}");
            }
            return !invalid;
        }

        bool valid = true;
        int slice = str.Length / divisor;
        string[] values = new string[divisor];

        for (int i = 0; i < slice; i++)
        {
            for (int x = 0; x < divisor; ++x)
            {
                values[x] = str.Substring(x * slice + i, 1);
            }

            if (values.Any(v => v != values[0]))
            {
                break;
            }
            if (i == slice - 1)
            {
                Console.WriteLine($"   invalid {str} divisor {divisor}");
                return false;
            }
        }
        return valid;
    }

    private Int64 FindInvalidsInRange(Range range, bool part2 = false)
    {
        Int64 start = Int64.Parse(range.Start);
        Int64 end = Int64.Parse(range.End);
        int maxLen = Math.Max(range.Start.Length, range.End.Length);

        Int64 sum = 0;
        for (Int64 i = start; i <= end; i++)
        {
            if (part2)
            {
                for (int j = 1; j < maxLen; ++j)
                {
                    if (!IsValid(i, j))
                    {
                        sum += i;
                        break;
                    }
                }
            }
            else
            {
                if (!IsValid(i)) sum += i;
            }

        }
        return sum;
    }

    public override async Task Run(string variant)
    {
        var ranges = new List<Range>();
        using (StreamReader sr = new StreamReader($"inputs/{variant}02.txt"))
        {
            var sb = new StringBuilder();

            string start = "";
            while (!sr.EndOfStream)
            {
                char ch = (char)sr.Read();
                //Console.WriteLine($"Read character: {(char)ch}");

                if (Char.IsDigit(ch))
                {
                    sb.Append(ch);
                }
                else if (ch == '-')
                {
                    start = sb.ToString();
                    sb.Clear();
                }
                else if (ch == ',')
                {
                    ranges.Add(new Range(start, sb.ToString()));
                    sb.Clear();
                }
            }
            ranges.Add(new Range(start, sb.ToString()));
            sb.Clear();
        }

        Int64 sum = 0;
        Int64 sum2 = 0;
        foreach (var range in ranges)
        {
            var amount = FindInvalidsInRange(range);
            var amount2 = FindInvalidsInRange(range, true);
            Console.WriteLine($"Range: {range.Start} - {range.End} invalids: {amount} {amount2}");
            sum += amount;
            sum2 += amount2;
        }
        Console.WriteLine($"Part 1 Sum {sum}");
        Console.WriteLine($"Part 2 Sum {sum2}");
    }
}
