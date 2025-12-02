if (args.Length == 0)
{
    System.Console.WriteLine("Please enter day number e.g. 01");
    return 1;
}

var day = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(args[0]);
var variant = args.Length > 1 ? args[1] : "input";

System.Console.WriteLine($"Running {day} {variant}");

var type = Type.GetType("AoC2025.Day" + day);
if (type == null)
{
    System.Console.WriteLine($"No runner for {day}");
    return 1;
}
var runner = (AoC2025.IAocRunner?)Activator.CreateInstance(type);
if (runner == null)
{
    System.Console.WriteLine($"No instance for {day}");
    return 1;
}

await runner.Run(variant);

return 0;