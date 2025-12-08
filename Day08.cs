namespace AoC2025;

internal class Day08 : IAocRunner
{
    public class Point3d(int x, int y, int z)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public int Z { get; } = z;

        public double Distance(Point3d other)
        {
            var dx = other.X - X;
            var dy = other.Y - Y;
            var dz = other.Z - Z;
            return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public override string ToString() => $"({X},{Y},{Z})";
    }

    public class ConnectedPoint(Point3d point) {
        public Point3d Point = point;
        public List<ConnectedPoint> Connections = new();

        public void Connect(ConnectedPoint other) {
            Console.WriteLine($"Connecting {this} to {other}");
            if (!Connections.Contains(other)) {
                Console.WriteLine($"  Added connection");
                Connections.Add(other);
                other.Connect(this);
            }
        }

        public bool IsConnected(ConnectedPoint other) {
            return Connections.Contains(other);
        }

        public int NumConnections() {
            return Connections.Count;
        }

        public override string ToString() => $"{Point.ToString()} ({NumConnections()} conn.)";
    }

    public class Circuit(Point3d[] initList) {
        private List<ConnectedPoint> _points = initList.Select(x => new ConnectedPoint(x)).ToList();

        public void AddConnection(ConnectedPoint a, ConnectedPoint b) {
            var found = _points.FirstOrDefault(p => p == a);
            var found2 = _points.FirstOrDefault(p => p == b);

            found.Connect(found2);
        }

        private void HandlePoint(List<ConnectedPoint> thisList, ConnectedPoint point, HashSet<Point3d> seen) {
            if (seen.Contains(point.Point))
            {
                return;
            }
            thisList.Add(point);
            seen.Add(point.Point);

            foreach (var conn in point.Connections)
            {
                HandlePoint(thisList, conn, seen);
            }
        }

        public List<List<ConnectedPoint>> GetCircuits() {
            var result = new List<List<ConnectedPoint>>();
            var seen = new HashSet<Point3d>();
            foreach (var point in _points)
            {
                var thisList = new List<ConnectedPoint>();
                var start = point;
                if (seen.Contains(start.Point)) {
                    continue;
                }
                HandlePoint(thisList, start, seen);
                result.Add(thisList);
                Console.WriteLine($"Got circuit of size: {thisList.Count}");
                foreach (var p in thisList)
                {
                    Console.WriteLine($"  {p}");
                }
            }
            return result;
        }

        public (ConnectedPoint, ConnectedPoint) GetMinDistanceNaive()
        {
            var minDistance = double.MaxValue;
            int ii = -1;
            int jj = -1;
            for (var i = 0; i < _points.Count; ++i)
            {
                if (_points[i].NumConnections() >= 2) {
                    continue;
                }
                for (var j = 0; j < _points.Count; ++j)
                {
                    if (i == j || _points[j].NumConnections() >= 2 || _points[j].IsConnected(_points[i]))
                    {
                        continue;
                    }
                    var d = _points[i].Point.Distance(_points[j].Point);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        ii = i;
                        jj = j;
                    }
                }
            }
            if (ii == -1) {
                throw new IndexOutOfRangeException("No valid point found");
            }
            return (_points[ii], _points[jj]);
        }
    }

    public (double, int, int) GetMinDistanceNaive(Point3d[] points, HashSet<(Point3d, Point3d)> alreadyConnected) {
        var minDistance = double.MaxValue;
        int ii = 0;
        int jj = 0;
        for (var i = 0; i < points.Length; ++i)
        {
            for (var j = 0; j < points.Length; ++j)
            {
                if (i == j || alreadyConnected.Contains((points[i], points[j]))) continue;
                var d = points[i].Distance(points[j]);
                if (d < minDistance)
                {
                    minDistance = d;
                    ii = i;
                    jj = j;
                }
            }
        }
        return (minDistance, ii, jj);
    }

    public override async Task Run(string variant)
    {
        var points = File.ReadLines($"inputs/{variant}08.txt")
            .Where(x => x.Length > 0)
            .Select(line => line.Split(",")
                .Select(int.Parse)
                .ToArray()
            )
            .Select(p => new Point3d(p[0], p[1], p[2]))
            .ToArray();
        
        foreach (var pt in points)
        {
            Console.WriteLine(pt);
        }

        var x = 0;
        var p = points;
        var circuit = new Circuit(points);

        //while (x < 10)
        while (true)
        {
            try
            {
                var (ca, cb) = circuit.GetMinDistanceNaive();
                Console.WriteLine($"Round {x+1}: min distance between {ca} and {cb}");
                circuit.AddConnection(ca, cb);
            }
            catch (IndexOutOfRangeException) {
                break;
            }
            x++;
        }

        var res = circuit.GetCircuits().Select(x => x.Count).OrderBy(x => x).Reverse().Take(3).Aggregate(1, (a, b) => a * b);
        Console.WriteLine($"Result: {res}");
    }
}
