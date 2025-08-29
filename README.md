# astar-pathfinding

### Setup

First, a TXT file containing an ASCII maze (see "mazes" directory) is read and
converted into a two-dimensional array or char (`char[,]`).

### Maze

Stores information about the dimensions, walls, start and end of the maze.
Upon initialisation, converts the char array into a two-dimensional array of
bool (`bool[,]`) where coordinates with walls are set to true and empty spaces
are set to false.

### Node

Stores information about the "node" at each coordinate. This is used by the A*
algorithm to check which moves are possible and which ones have been completed.
The data in the maze object is used to generate a two-dimensional array of node
(`Node[,]`).

### A* implementation

A compact and idiomatic implementation of the A* algorithm. This approach uses
the Manhattan distance heuristic for finding the optimal path. It is restricted
to 4-way movement (up, down, left and right). Since diagonal movement is not
possible, the true minimum cost from one node to another is exactly the
Manhattan distance (multiplied by step costs).

The Manhattan distance is calculated as follows, where h(n) is the heuristic
estimate of the remaining cost to reach the goal from node n:

$$
h(n) = |x_{current} - x_{goal}| + |y_{current} - y_{goal}|
$$

G-score is the cost to reach a node from the start node. When a move is made
from a node to a neighbour, the edge cost is added to the parent's (previous
node's) g-score. This is calculated as follows, where g(u) is the g-score and
w(u,v) is the edge cost from the current node u to the neighbouring node v:

$$
g(v) = g(u) + w(u, v)
$$

The estimated total cost of the cheapest path that goes through a given node n
is calculated as follows, where f(n) is the estimated total cost, g(n) is the
cost until now and h(n) is the heuristic estimate:

$$
f(n) = g(n) + h(n)
$$

Each discovered/explored node is put into a priority queue ordered by ascending
f(n). Nodes with a low g(n) and h(n) are therefore valued higher since they
result in the lowest f(n), a.k.a. the cheapest path found so far. When the goal
node is reached (assuming the maze is possible to solve), the path with the
lowest g-score is the winner.
