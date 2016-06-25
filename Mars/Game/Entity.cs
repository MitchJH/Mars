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

        public Entity(Vector2? position = null)
        {
            if (position != null) _position = Vector2.Zero;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}