namespace Pathfinding;

internal class Maze
{
	public int Width;
	public int Height;
	public bool[,] Walls;
	public (int X, int Y) Start;
	public (int X, int Y) End;

	public Maze(char[,] charGrid)
	{
		Width = charGrid.GetLength(0);
		Height = charGrid.GetLength(1);
		if (Width == 0 || Height == 0)
		{
			throw new ArgumentException("Maze cannot have zero width or height");
		}

		Walls = new bool[Width, Height];

		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{
				char c = charGrid[x, y];
				switch (c)
				{
					case '#':
						Walls[x, y] = true;
						break;

					case 'S':
						Start = (x, y);
						break;

					case 'E':
						End = (x, y);
						break;
				}
			}
		}
	}
}
