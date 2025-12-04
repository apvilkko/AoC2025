namespace AoC2025;

internal class Day04 : IAocRunner
{
    public class Cell {
        private char _value;
        
        public Cell(char c) {
            _value = c;
        }

        public bool IsPaper() => _value == '@';

        public override string ToString() {
            return _value.ToString();
        }
    }

    public class Grid {
        private Cell[][] _cells;

        private Grid(Cell[][] cells) {
            _cells = cells;
        }

        public static Grid ReadFrom(string filename) {
            var grid = new Grid(File.ReadLines(filename)
                .Where(x => x.Length > 0)
                .Select(line => line.ToCharArray()
                    .Select(c => new Cell(c)).ToArray()
                )
                .ToArray());
            return grid;
        }

        public int RowCount => _cells.Length;
        public int ColCount => _cells.Length > 0 ? _cells[0].Length : 0;

        public Cell? GetCell(int row, int col) {
            if (row < 0 || row >= _cells.Length) return null;
            if (col < 0 || col >= _cells[row].Length) return null;
            return _cells[row][col];
        }

        public override string ToString() {
            return string.Join("\n", _cells.Select(row => string.Join("", row)));
        }

        public void ApplyRemovals(HashSet<(int r, int c)> removals) {
            foreach (var removal in removals) {
                _cells[removal.r][removal.c] = new Cell('.');
            }
        }
    }

    private static (int dr, int dc)[] Adjacents = [
            (0,1),
            (1,1),
            (1,0),
            (0,-1),
            (-1,-1),
            (-1,0),
            (-1,1),
            (1,-1),
        ];

    private HashSet<(int r, int c)> GetMatched(Grid grid) {
        var matched = new HashSet<(int r, int c)>();
        for (int r = 0; r < grid.RowCount; ++r)
        {
            for (int c = 0; c < grid.ColCount; ++c)
            {
                var cell = grid.GetCell(r, c);
                if (cell == null || !cell.IsPaper())
                {
                    continue;
                }
                var numRolls = 0;
                foreach (var (dr, dc) in Adjacents)
                {
                    var neighbor = grid.GetCell(r + dr, c + dc);
                    if (neighbor != null && neighbor.IsPaper())
                    {
                        numRolls++;
                    }
                }
                if (numRolls < 4)
                {
                    matched.Add((r, c));
                    //Console.WriteLine($"Matched cell at ({r},{c}) with {numRolls} adjacent papers.");
                }
            }
        }
        return matched;
    }

    public override async Task Run(string variant)
    {
        var grid = Grid.ReadFrom($"inputs/{variant}04.txt");
        Console.WriteLine(grid);

        var matched = GetMatched(grid);
        Console.WriteLine($"Part 1 sum: {matched.Count}");

        var sum = matched.Count;
        while (matched.Count > 0) {
            grid.ApplyRemovals(matched);
            matched = GetMatched(grid);
            Console.WriteLine($"removing {matched.Count}");
            sum += matched.Count;
        }

        Console.WriteLine(grid);
        Console.WriteLine($"Part 2 sum: {sum}");
    }
}