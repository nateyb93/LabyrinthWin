﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace LabyrinthGame
{
    public enum Shape
    {
        S,
        L,
        T
    }

    /// <summary>
    /// The Node class defines a node in a graph representation of the Labyrinth game board
    /// </summary>
    public class BoardNode
    {
        public const int ROTATE_0 = 0;
        public const int ROTATE_1 = 1;
        public const int ROTATE_2 = 2;
        public const int ROTATE_3 = 3;

        private int _rotation;
        /// <summary>
        /// Rotation of the path
        /// </summary>
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
        
        private Color _color;
        /// <summary>
        /// Color of pickup on this square. White means no pickup
        /// </summary>
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        private Shape _shape;
        /// <summary>
        /// Shape of path on node
        /// </summary>
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

        /// <summary>
        /// List of players present on the node
        /// </summary>
        public List<Player> Players;

        public BoardNode(Color pickup, Shape shape, int rotation)
        {
            Players = new List<Player>();
            Color = pickup;
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
                _rotation++;
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
                _rotation++;
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
