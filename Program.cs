using System.Diagnostics;

namespace Pathfinding;

internal class Program
{
	public static void Main(string[] _)
	{
		char[,] charGrid = ParseCharGrid("mazes/maze-1.txt");
		Maze maze = new(charGrid);
		//PrintMaze(maze);

		long startTime = Stopwatch.GetTimestamp();

		Node[,] nodeGrid = BuildNodeGrid(maze);
		AStar AStar = new(nodeGrid);
		Stack<Node> path = AStar.FindPathGrid(
			maze.Start.X,
			maze.Start.Y,
			maze.End.X,
			maze.End.Y);

		long endTime = Stopwatch.GetTimestamp();
		TimeSpan elapsed = Stopwatch.GetElapsedTime(startTime, endTime);
		string elapsedStr = string.Join(" ", [
			$"{elapsed.Seconds} s",
			$"{elapsed.Milliseconds} ms",
			$"{elapsed.Microseconds} µs"]);

		Console.WriteLine($"Path found in: {elapsedStr}");
		Console.WriteLine($"Path length:   {path.Count} nodes\n");
		PrintMaze(maze, path);
	}

	private static char[,] ParseCharGrid(string filePath)
	{
		string[] lines = [..
			File.ReadAllLines(filePath)
			.Select(l => l.Trim())
			.Where(l => !string.IsNullOrWhiteSpace(l))];
		char[,] grid = new char[lines[0].Length, lines.Length];

		for (int y = 0; y < lines.Length; y++)
		{
			for (int x = 0; x < lines[0].Length; x++)
			{
				grid[x, y] = lines[y][x];
			}
		}

		return grid;
	}

	private static Node[,] BuildNodeGrid(Maze maze)
	{
		Node[,] grid = new Node[maze.Width, maze.Height];

		for (int y = 0; y < maze.Height; y++)
		{
			for (int x = 0; x < maze.Width; x++)
			{
				bool walkable = !maze.Walls[x, y];
				grid[x, y] = new Node(x, y, walkable);
			}
		}

		return grid;
	}

	private static void PrintMaze(Maze maze, Stack<Node>? path = null)
	{
		char[,] grid = new char[maze.Width, maze.Height];

		if (path != null)
		{
			foreach (Node n in path)
			{
				if ((n.X, n.Y) != maze.Start && (n.X, n.Y) != maze.End)
				{
					grid[n.X, n.Y] = '*';
				}
			}
		}

		for (int y = 0; y < maze.Height; y++)
		{
			for (int x = 0; x < maze.Width; x++)
			{
				char c = true switch
				{
					_ when (x, y) == maze.Start => 'S',
					_ when (x, y) == maze.End => 'E',
					_ when maze.Walls[x, y] => '\u25A0',
					_ when grid[x, y] == '*' => '*',
					_ => ' ',
				};

				Console.ForegroundColor = c switch
				{
					'S' or 'E' or '*' => ConsoleColor.Green,
					_ => ConsoleColor.DarkGray,
				};
				Console.Write(c);
			}

			Console.ResetColor();
			Console.WriteLine();
		}
	}
}
