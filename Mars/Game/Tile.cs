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
        private TileType _type;
        private bool _hovered;
        private Pipe _pipe;

        public Tile(int x, int y)
        {
            _position = new Point(x, y);
            _hovered = false;

            //CARTESIAN
            //_center = new Vector2((x * Constants.TILE_WIDTH) + (Constants.TILE_WIDTH / 2), (y * Constants.TILE_WIDTH) + (Constants.TILE_WIDTH / 2));

            //DIAMOND
            double tile_pos_x = x * Constants.TILE_WIDTH / 2 - y * Constants.TILE_WIDTH / 2;
            double tile_pos_y = x * Constants.TILE_HEIGHT / 2 + y * Constants.TILE_HEIGHT / 2;

            Rectangle rec = new Rectangle(
                        (int)tile_pos_x,
                        (int)tile_pos_y + Constants.TILE_DEPTH,
                        Constants.TILE_WIDTH,
                        Constants.TILE_WIDTH + Constants.TILE_DEPTH);

            float r1 = rec.X + (Constants.TILE_WIDTH / 2);
            float r2 = rec.Y + (Constants.TILE_WIDTH - Constants.TILE_HEIGHT / 2);

            _center = new Vector2(r1, r2);
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
    }
}
