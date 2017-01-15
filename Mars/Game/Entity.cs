using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public class Entity
    {
        private Vector2 _position;
        private Vector2 _size;
        
        public Entity(Vector2 position, Vector2 size)
        {
            _position = position;
            _size = size;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Point TilePosition
        {
            get
            {
                return new Point((int)Center.X / Constants.TILE_WIDTH, (int)Center.Y / Constants.TILE_HEIGHT);
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y);
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(_position.X + (_size.X / 2), _position.Y + (_size.Y / 2));
            }
        }
    }
}