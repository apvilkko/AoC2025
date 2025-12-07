namespace AoC2025;

internal class Day07 : IAocRunner
{
    public class Grid(char[][] grid)
    {
        private char[][] _grid = grid;
        private int _currentLine = -1;
        public int NumSplits;

        public int RowCount() => _grid.Length;
        public char[] GetRow(int i) => _grid[i];

        private static int[] LeftAndRight = [-1, 1];
        private long _sum = 0;
        private Dictionary<(int, int), long> _cache = new();

        public void PrintCache()
        {
            foreach (var val in _cache)
            {
                Console.WriteLine($"{val.Key}: {val.Value}");
            }
        }

        public long GetPathsTo(int row, int col)
        {
            var cell = GetCell(row, col);
            //Console.WriteLine($"GetPathsTo {row} {col} '{cell}'");
            
            if (_cache.TryGetValue((row, col), out var cached))
            {
                return cached;
            }

            if (cell == 'S') return 1;

            long sum = 0;
            var numPaths = 0;
            if (cell == '|')
            {

                foreach (var dir in LeftAndRight)
                {
                    var adjacent = GetCell(row, col + dir);
                    //Console.WriteLine($"  adjacent {dir} ({row},{col + dir}) is {adjacent}");
                    if (adjacent == '^')
                    {
                        //Console.WriteLine($"    branching to ({row - 1},{col + dir})");
                        sum += GetPathsTo(row - 1, col + dir);
                        numPaths++;
                    }
                }

                var prev = GetCell(row - 1, col);
                //Console.WriteLine($"  prev is ({row - 1},{col}) {prev}");
                if (prev == '|' || prev == 'S')
                {
                    //Console.WriteLine($"    moving to ({row - 1},{col})");
                    sum += GetPathsTo(row - 1, col);
                }

            }

            var result = sum /*+ (numPaths == 2 ? 1 : 0)*/;
            _cache.Add((row, col), result);

            return result;
        }

        public void Print()
        {
            Console.WriteLine("");
            foreach (var line in _grid)
            {
                Console.WriteLine(string.Join("", line));
            }
            Console.WriteLine($"number of splits {NumSplits}");
        }

        public void BeamTo(int row, int col)
        {
            _grid[row][col] = '|';
        }

        public char GetCell(int row, int col)
        {
            var value = ' ';
            try
            {
                value = _grid[row][col];
            }
            catch (IndexOutOfRangeException) { }
            return value;
        }

        public bool Advance()
        {
            _currentLine++;
            if (_currentLine >= _grid.Length) return false;
            var line = _grid[_currentLine];
            for (var i = 0; i < line.Length; ++i)
            {
                if (line[i] == 'S')
                {
                    BeamTo(_currentLine + 1, i);
                }
                else if (line[i] == '^' && GetCell(_currentLine - 1, i) == '|')
                {
                    BeamTo(_currentLine, i + 1);
                    BeamTo(_currentLine, i - 1);
                    NumSplits++;
                }
                else if (line[i] == '.' && GetCell(_currentLine - 1, i) == '|')
                {
                    BeamTo(_currentLine, i);
                }
            }
            return _currentLine < _grid.Length;
        }

    }

    public override async Task Run(string variant)
    {
        var lines = File.ReadLines($"inputs/{variant}07.txt").Where(x => x.Length > 0).ToList();

        var grid = new Grid(lines.Select(line => line.ToCharArray())
                .ToArray());

        grid.Print();

        while (grid.Advance())
        {
            grid.Print();
        }


        var currentLine = grid.RowCount() - 2;
        long sum = 0;

        var row = grid.GetRow(currentLine);
        for (var i = 0; i < row.Length; ++i)
        {
            if (row[i] == '|')
            {
                sum += grid.GetPathsTo(currentLine, i);
            }
        }
        //grid.PrintCache();

        Console.WriteLine($"sum {sum}");

    }
}
