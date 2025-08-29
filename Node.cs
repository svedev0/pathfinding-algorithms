namespace Pathfinding;

internal class Node(int x, int y, bool walkable, float weight = 1f)
{
	public readonly int X = x; // X coordinate
	public readonly int Y = y; // Y coordinate
	public readonly bool Walkable = walkable; // Can this tile be walked on
	public readonly float Weight = weight; // Movement cost to enter this tile
}
