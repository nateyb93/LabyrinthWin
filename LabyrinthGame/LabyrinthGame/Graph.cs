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
    public class Graph
    {
        private Dictionary<Node, List<Node>> _adjacencyList;

        public Graph()
        {
            _adjacencyList = new Dictionary<Node, List<Node>>();
        }


        /// <summary>
        /// Adds a node to the graph
        /// </summary>
        /// <param name="n"></param>
        public void AddNode(Node n)
        {
            if (!_adjacencyList.ContainsKey(n))
            {
                _adjacencyList[n] = new List<Node>();
            }
        }

        /// <summary>
        /// Adds an edge to the graph between two nodes
        /// </summary>
        /// <param name="from">First node in edge</param>
        /// <param name="to">End node in edge</param>
        public void AddEdge(Node from, Node to)
        {
            if (!_adjacencyList.ContainsKey(from))
            {
                _adjacencyList[from] = new List<Node>();
            }

            _adjacencyList[from].Add(to);
        }

        public bool HasPath(Node from, Node to)
        {
            return false;
        }
    }
}
