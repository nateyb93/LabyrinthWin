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
        RedCircle,
        RedStar,
        RedSquare,
        RedTriangle,
        RedHex,
        RedHeart,
        GreenCircle,
        GreenStar,
        GreenSquare,
        GreenTriangle,
        GreenHex,
        GreenHeart,
        BlueCircle,
        BlueStar,
        BlueSquare,
        BlueTriangle,
        BlueHex,
        BlueHeart,
        YellowCircle,
        YellowStar,
        YellowSquare,
        YellowTriangle,
        YellowHex,
        YellowHeart
    }

    public enum Shape
    {
        S,
        L,
        T
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
        public const int ROTATE_0 = 0;
        public const int ROTATE_1 = 1;
        public const int ROTATE_2 = 2;
        public const int ROTATE_3 = 3;

        private int _rotation;
        public int Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        
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

        private Shape _shape;
        public Shape Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
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

        public Node(Pickup pickup, Shape shape, int rotation)
        {
            _pickup = pickup;
            _shape = shape;
            _rotation = rotation;
        }

        /// <summary>
        /// Rotates the node clockwise
        /// </summary>
        public void RotateClockwise()
        {
            if (_rotation == 3)
            {
                _rotation = 0;
            }
            else
            {
                _rotation += 1;
            }
        }

        /// <summary>
        /// Rotates the node counterclockwise
        /// </summary>
        public void RotateCounter()
        {
            if (_rotation == 3)
            {
                _rotation = 0;
            }
            else
            {
                _rotation += 1;
            }
        }

        /// <summary>
        /// Returns whether the top edge of the tile is open
        /// </summary>
        /// <returns></returns>
        public bool IsOpenTop()
        {
            switch (_shape)
            {
                case Shape.S:
                    if (_rotation == ROTATE_0 || _rotation == ROTATE_2)
                        return true;
                    break;
                case Shape.L:
                    if (_rotation == ROTATE_0 || _rotation == ROTATE_3)
                        return true;
                    break;
                case Shape.T:
                    if (_rotation == ROTATE_1 || _rotation == ROTATE_2 || _rotation == ROTATE_3)
                        return true;
                    break;
            }

            return false;
        }

        /// <summary>
        /// Returns whether the bottom edge of the tile is open
        /// </summary>
        /// <returns></returns>
        public bool IsOpenBottom()
        {
            switch (_shape)
            {
                case Shape.S:
                    if (_rotation == ROTATE_0 || _rotation == ROTATE_2)
                        return true;
                    break;
                case Shape.L:
                    if (_rotation == ROTATE_1 || _rotation == ROTATE_2)
                        return true;
                    break;
                case Shape.T:
                    if (_rotation == ROTATE_0 || _rotation == ROTATE_1 || _rotation == ROTATE_3)
                        return true;
                    break;
            }

            return false;
        }

        /// <summary>
        /// Returns whether the left edge of the tile is open
        /// </summary>
        /// <returns></returns>
        public bool IsOpenLeft()
        {
            switch (_shape)
            {
                case Shape.S:
                    if (_rotation == ROTATE_1 || _rotation == ROTATE_3)
                        return true;
                    break;
                case Shape.L:
                    if (_rotation == ROTATE_2 || _rotation == ROTATE_3)
                        return true;
                    break;
                case Shape.T:
                    if (_rotation == ROTATE_0 || _rotation == ROTATE_1 || _rotation == ROTATE_2)
                        return true;
                    break;
            }

            return false;
        }

        /// <summary>
        /// Returns whether the right edge of the tile is open
        /// </summary>
        /// <returns></returns>
        public bool IsOpenRight()
        {
            switch (_shape)
            {
                case Shape.S:
                    if (_rotation == ROTATE_1 || _rotation == ROTATE_3)
                        return true;
                    break;
                case Shape.L:
                    if (_rotation == ROTATE_0 || _rotation == ROTATE_1)
                        return true;
                    break;
                case Shape.T:
                    if (_rotation == ROTATE_0 || _rotation == ROTATE_2 || _rotation == ROTATE_3)
                        return true;
                    break;
            }

            return false;
        }

    }
}
