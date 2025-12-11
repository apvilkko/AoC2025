namespace AoC2025;

internal class Day11 : IAocRunner
{
    public class Node(string name)
    {
        public string Name = name;

        public List<Node> Connections = new List<Node>();
    }

    public class Graph {
        public Dictionary<string, Node> Nodes = new();

        public void AddNode(string from, string[] to)
        {
            if (!Nodes.ContainsKey(from))
            {
                Nodes[from] = new Node(from);
            }
            foreach (var target in to)
            {
                if (!Nodes.ContainsKey(target))
                {
                    Nodes[target] = new Node(target);
                }
                Nodes[from].Connections.Add(Nodes[target]);
            }
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var node in Nodes.Values)
            {
                sb.AppendLine($"{node.Name} -> {string.Join(", ", node.Connections.Select(n => n.Name))}");
            }
            return sb.ToString();
        }

        private void Dfs(string src, string dest,
            HashSet<string> visited,
            List<string> path,
            List<List<string>> allPaths)
        {
            //Console.WriteLine($"Visiting {src} path: {string.Join(",", path)}");
            path.Add(src);

            if (src == dest)
            {
                allPaths.Add([.. path]);
            }
            else
            {
                var current = Nodes[src];
                foreach(var adjacent in current.Connections)
                {
                    if (!visited.Contains(adjacent.Name))
                    {
                        visited.Add(adjacent.Name);
                        Dfs(adjacent.Name, dest, visited, path, allPaths);
                        visited.Remove(adjacent.Name);
                    }
                }
            }

            path.RemoveAt(path.Count - 1);
        }

        public List<List<string>> FindPaths(string src, string dest)
        {
            var visited = new HashSet<string>();
            var path = new List<string>();
            var allPaths = new List<List<string>>();

            visited.Add(src);
            Dfs(src, dest, visited, path, allPaths);

            return allPaths;
        }
    }

    public override async Task Run(string variant)
    {
        var graph = new Graph();

        File.ReadLines($"inputs/{variant}11.txt")
            .Where(line => line.Length > 0)
            .Select(line =>
            {
                var parts = line.Split(":");
                var from = parts[0].Trim();
                var to = parts[1].Trim().Split(" ");
                graph.AddNode(from, to);
                return from;
            }).Count();

        /*List<List<string>> paths = graph.FindPaths("you", "out");

        Console.WriteLine(graph);

        foreach (var path in paths)
        {
            Console.WriteLine(string.Join("-", path));
        }
        Console.WriteLine($"Total paths: {paths.Count}");*/


        List<List<string>> dac_fft = graph.FindPaths("dac", "fft");
        Console.WriteLine($"dac->fft: {dac_fft.Count}");
        List<List<string>> fft_dac = graph.FindPaths("fft", "dac");
        Console.WriteLine($"fft->dac: {fft_dac.Count}");

        List<List<string>> svr_dac = graph.FindPaths("svr", "dac");
        Console.WriteLine($"svr->dac: {svr_dac.Count}");
        List<List<string>> svr_fft = graph.FindPaths("svr", "fft");
        Console.WriteLine($"svr->fft: {svr_fft.Count}");
        List<List<string>> dac_out = graph.FindPaths("dac", "out");
        Console.WriteLine($"dac->out: {dac_out.Count}");
        List<List<string>> fft_out = graph.FindPaths("fft", "out");
        Console.WriteLine($"fft->out: {fft_out.Count}");



        /*foreach (var path in paths2)
        {
            Console.WriteLine(string.Join("-", path));
        }
        Console.WriteLine($"Total paths (part 2): {paths2.Count}");*/
    }
}
