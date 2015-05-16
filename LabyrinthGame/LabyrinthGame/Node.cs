using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public enum Pickup
    {
        None,
        Skeleton,
        Sword,
        Gold,
        Key,
        Gem,
        Helm,
        Book,
        Crown,
        Treasure,
        Candle,
        Map,
        Ring,
        Butterfly,
        Genie,
        Ogre,
        Pixie,
        Bat,
        Mouse,
        Ghost,
        Lizard,
        Spider,
        Dragon,
        Bug,
        Scroll
    }

    public enum Directions
    {
        Left,
        Up,
        Right,
        Down
    }
    /// <summary>
    /// The Node class defines a node in a graph representation of the Labyrinth game board
    /// </summary>
    public class Node
    {
        private Pickup _pickup;
        /// <summary>
        /// </summary>
        public Pickup Pickup
        {
            get
            {
                return _pickup;
            }
            set
            {
                _pickup = value;
            }
        }

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }

        private bool[] _open;

        public Node(Pickup pickup, bool[] open)
        {
            if (open.Length != 4)
            {
                throw new Exception("Param 'open' must contain exactly 4 entries!");
            }

            _open = open;
            _pickup = pickup;
        }

        /// <summary>
        /// Rotates the node clockwise
        /// </summary>
        public void RotateClockwise()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    _open[i] = _open[3];
                }
                else
                {
                    _open[i] = _open[i - 1];
                }
            }
        }

        /// <summary>
        /// Rotates the node counterclockwise
        /// </summary>
        public void RotateCounter()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    _open[i] = _open[0];
                }
                else
                {
                    _open[i] = _open[i + 1];
                }
            }
        }

        /// <summary>
        /// Checks to see if a direction is open in the current node
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool _isOpen(int idx)
        {
            if (idx >= 4)
            {
                throw new Exception("'idx' must be less than 4");
            }
            if (_open[idx])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
