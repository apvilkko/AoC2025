namespace AoC2025;

internal class Day01 : IAocRunner
{
    public override async Task Run(string variant)
    {
        int counter = 50;
        int modulo = 100;
        var lines = File.ReadLines($"inputs/{variant}01.txt");
        var sum = lines.Aggregate(0, (acc, line) =>
        {
            if (line.Trim().Length != 0)
            {
                var direction = line[0];
                var amount = int.Parse(line.Substring(1)) % modulo;
                if (direction == 'L') counter -= amount;
                else if (direction == 'R') counter += amount;
                if (counter < 0) counter += modulo;
                if (counter >= modulo) counter -= modulo;
                Console.WriteLine($"Direction: {direction}, Amount: {amount}, Counter: {counter}");
                if (counter == 0) acc++;
            }
            return acc;
        });
        Console.WriteLine($"Part 1 Sum: {sum}");

        counter = 50;
        modulo = 100;
        var i = 0;
        sum = lines.Aggregate(0, (acc, line) =>
        {
            if (line.Trim().Length != 0)
            {
                var direction = line[0];
                var amount = int.Parse(line.Substring(1));
                if (direction == 'L') amount *= -1;
                Console.WriteLine($"Line {++i}: {line} {counter}");
                for (int step = 0; step < Math.Abs(amount); step++)
                {
                    counter += amount > 0 ? 1 : -1;

                    if (counter % modulo == 0) {
                        Console.WriteLine($"  Hit zero at step {step + 1} of {Math.Abs(amount)}, counter is {counter}");
                        acc++;
                    }

                    if (counter < 0)
                    {
                        counter += modulo;
                    }
                    if (counter >= modulo)
                    {
                        counter -= modulo;
                    }
                }
                Console.WriteLine($"Direction: {direction}, Amount: {amount}, Counter: {counter}");
            }
            return acc;
        });
        Console.WriteLine($"Part 2 Sum: {sum}");
    }
}
