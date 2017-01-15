using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public enum TileType
    {
        Passable,
        Impassable
    }

    public class Tile : IPathNode<Object>
    {
        private Point _position;
        private Vector2 _center;
        private Rectangle _bounds;
        private TileType _type;
        private bool _hovered;
        private Pipe _pipe;

        public Tile(int x, int y)
        {
            _position = new Point(x, y);
            _hovered = false;

            float tile_pos_x = x * Constants.TILE_WIDTH;
            float tile_pos_y = y * Constants.TILE_HEIGHT;

            _center = new Vector2(tile_pos_x + (Constants.TILE_WIDTH / 2), tile_pos_y + (Constants.TILE_HEIGHT / 2));

            _bounds = new Rectangle((int)tile_pos_x, (int)tile_pos_y, Constants.TILE_WIDTH, Constants.TILE_HEIGHT);
        }

        public void Update(GameTime gameTime)
        {
            _hovered = _bounds.Contains(Controls.Mouse.Position);
        }

        public bool IsWalkable(Object unused)
        {
            if (_type == TileType.Passable)
            {
                return true;
            }            

            return false;
        }

        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Center
        {
            get { return _center; }
            set { _center = value; }
        }

        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        public TileType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public bool Hovered
        {
            get { return _hovered; }
            set { _hovered = value; }
        }

        public Pipe Pipe
        {
            get { return _pipe; }
            set { _pipe = value; }
        }

        // Corners
        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(_bounds.X + 1, _bounds.Y + 1);
            }
        }
        public Vector2 TopRight
        {
            get
            {
                return new Vector2(_bounds.X + _bounds.Width - 1, _bounds.Y + 1);
            }
        }
        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(_bounds.X + 1, _bounds.Y + _bounds.Height - 1);
            }
        }
        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(_bounds.X + _bounds.Width - 1, _bounds.Y + _bounds.Height - 1);
            }
        }
    }
}
