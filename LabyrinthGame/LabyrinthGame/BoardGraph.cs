using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    /// <summary>
    /// Defines an unweighted graph data structure
    /// </summary>
    public class BoardGraph
    {
        private Dictionary<BoardNode, List<BoardNode>> _adjacencyList;

        /// <summary>
        /// Constructs a graph from a game board
        /// </summary>
        /// <param name="nodes"></param>
        public BoardGraph(BoardNode[,] nodes)
        {
            _adjacencyList = new Dictionary<BoardNode, List<BoardNode>>();
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    _adjacencyList[nodes[x, y]] = new List<BoardNode>();
                    _adjacencyList[nodes[x, y]].Add(nodes[x, y]);

                    //Checks each direction from the current node for valid connections
                    //this should have the effect of building the graph from the game board
                    if (x != 0)
                    {
                        _checkHorizontalConnection(nodes[x - 1, y], nodes[x, y]);
                    }
                    if (y != 0)
                    {
                        _checkVerticalConnection(nodes[x, y - 1], nodes[x, y]);
                    }
                    if (x != nodes.GetLength(0) - 1)
                    {
                        _checkHorizontalConnection(nodes[x, y], nodes[x + 1, y]);
                    }
                    if (y != nodes.GetLength(1) - 1)
                    {
                        _checkVerticalConnection(nodes[x, y], nodes[x, y + 1]);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes an empty graph with test values
        /// </summary>
        /// <param name="test"></param>
        public BoardGraph()
        {

        }

        /// <summary>
        /// Checks for a vertical connection between two tiles.
        /// 
        /// If it finds that a valid connection exists between the two tiles,
        /// each node will be added to the other's list of adjacent nodes
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        private void _checkVerticalConnection(BoardNode top, BoardNode bottom)
        {
            if (top.IsOpenBottom() && bottom.IsOpenTop())
            {
                AddEdge(top, bottom);
                AddEdge(bottom, top);
            }
        }

        /// <summary>
        /// Checks for a horizontal connection between two tiles.
        /// If it finds that a valid connection exists between the two tiles,
        /// each node will be added to the other's list of adjacent nodes
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private void _checkHorizontalConnection(BoardNode left, BoardNode right)
        {
            if (left.IsOpenRight() && right.IsOpenLeft())
            {
                AddEdge(left, right);
                AddEdge(right, left);
            }
        }


        /// <summary>
        /// Adds a node to the graph
        /// </summary>
        /// <param name="n"></param>
        public void AddNode(BoardNode n)
        {
            if (!_adjacencyList.ContainsKey(n))
            {
                _adjacencyList[n] = new List<BoardNode>();
            }
        }

        /// <summary>
        /// Adds an edge to the graph between two nodes
        /// </summary>
        /// <param name="from">First node in edge</param>
        /// <param name="to">End node in edge</param>
        public void AddEdge(BoardNode from, BoardNode to)
        {
            if (!_adjacencyList.ContainsKey(from))
            {
                _adjacencyList[from] = new List<BoardNode>();
            }

            if(!_adjacencyList[from].Contains(to))
                _adjacencyList[from].Add(to);
        }

        //Checks to see if there's a path from one node to another
        public bool HasPath(BoardNode from, BoardNode to)
        {
            List<BoardNode> visited = new List<BoardNode>();
            Queue<BoardNode> queue = new Queue<BoardNode>();

            visited.Add(from);
            queue.Enqueue(from);

            while (queue.Count != 0)
            {
                BoardNode current = queue.Dequeue();
                foreach (BoardNode n in _adjacencyList[current])
                {
                    if (n == to)
                    {
                        return true;
                    }
                    else if (!visited.Contains(n))
                    {
                        visited.Add(n);
                        queue.Enqueue(n);
                    }
                }
            }

            return false;
        }
    }
}
