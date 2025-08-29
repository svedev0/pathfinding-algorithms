namespace Pathfinding;

internal class AStar
{
	private readonly Node[,] _grid;
	public int Width => _grid.GetLength(0);
	public int Height => _grid.GetLength(1);

	private static readonly (int dX, int dY)[] _directions = [
		(0, 1),
		(0, -1),
		(-1, 0),
		(1, 0)
	];

	public AStar(Node[,] grid)
	{
		_grid = grid;

		if (Width == 0 || Height == 0)
		{
			throw new ArgumentException("Grid is empty");
		}
	}

	// Find a path on the grid
	public Stack<Node> FindPathGrid(int startX, int startY, int endX, int endY)
	{
		if (!InBounds(startX, startY) || !InBounds(endX, endY))
		{
			throw new ArgumentException("Start or end coords out of bounds");
		}

		Node startNode = _grid[startX, startY];
		Node goalNode = _grid[endX, endY];
		if (!startNode.Walkable || !goalNode.Walkable)
		{
			throw new ArgumentException("Start or end node is not walkable");
		}

		Dictionary<(int x, int y), (int px, int py)> parents = []; // node XY, parent XY
		Dictionary<(int x, int y), float> gScores = new(Width * Height); // node XY, g score
		Dictionary<(int x, int y), float> fScores = new(Width * Height); // node XY, f score

		(int x, int y) startCoords = (startX, startY);
		(int x, int y) goalCoords = (endX, endY);

		gScores[startCoords] = 0f;
		fScores[startCoords] = CalcHeuristic(startCoords, goalCoords);

		PriorityQueue<(int x, int y), float> frontier = new(); // node XY, f score
		frontier.Enqueue(startCoords, fScores[startCoords]);

		HashSet<(int x, int y)> frontierSet = [startCoords];
		HashSet<(int x, int y)> exploredSet = [];

		while (frontier.Count > 0)
		{
			(int x, int y) current = frontier.Dequeue();
			frontierSet.Remove(current);

			if (current == goalCoords)
			{
				// Goal reached
				return ReconstructPath(parents, current);
			}

			exploredSet.Add(current);

			foreach ((int dirX, int dirY) in _directions)
			{
				int neighbourX = current.x + dirX;
				int neighbourY = current.y + dirY;
				if (!InBounds(neighbourX, neighbourY))
				{
					// Neighbour coords out of bounds
					continue;
				}

				Node neighbour = _grid[neighbourX, neighbourY];
				if (!neighbour.Walkable)
				{
					// Neighbour is not walkable
					continue;
				}

				(int x, int y) neighbourCoords = (neighbourX, neighbourY);
				if (exploredSet.Contains(neighbourCoords))
				{
					// Neighbour has already been explored
					continue;
				}

				float tentativeG = gScores[current] + neighbour.Weight;

				if (gScores.TryGetValue(neighbourCoords, out float gExisting) &&
					tentativeG >= gExisting)
				{
					// Previous neighbour g score is cheaper
					continue;
				}

				parents[neighbourCoords] = current;
				gScores[neighbourCoords] = tentativeG;

				float h = CalcHeuristic(neighbourCoords, goalCoords);
				float f = tentativeG + h;
				fScores[neighbourCoords] = f;

				// Add neighbour to frontier
				if (!frontierSet.Contains(neighbourCoords))
				{
					frontier.Enqueue(neighbourCoords, f);
					frontierSet.Add(neighbourCoords);
				}
			}
		}

		throw new Exception("No path found");
	}

	// Calculate Manhattan distance heuristic
	private static float CalcHeuristic((int x, int y) neighbour, (int x, int y) goal)
	{
		return Math.Abs(neighbour.x - goal.x) + Math.Abs(neighbour.y - goal.y);
	}

	// Reconstruct path from end node to start node
	private Stack<Node> ReconstructPath(
		Dictionary<(int x, int y), (int px, int py)> parents,
		(int x, int y) current)
	{
		Stack<Node> path = new();

		while (true)
		{
			path.Push(_grid[current.x, current.y]);

			if (!parents.TryGetValue(current, out (int px, int py) prev))
			{
				break;
			}

			current = prev;
		}

		return path;
	}

	// Check if coordinates are within grid bounds
	private bool InBounds(int x, int y)
	{
		return x >= 0 && x < Width && y >= 0 && y < Height;
	}
}
