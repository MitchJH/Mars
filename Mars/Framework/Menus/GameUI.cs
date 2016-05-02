using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Mars
{
    public class GameUI
    {
        private string _ID;
        private List<GUIControl> _controls = new List<GUIControl>();

        public GameUI(string ID)
        {
            _ID = ID;
        }

        public virtual void Update()
        {
            foreach (GUIControl control in _controls)
            {
                control.Update();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (GUIControl control in _controls)
            {
                control.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public string ID
        {
            get
            {
                return _ID;
            }
        }

        public List<GUIControl> Controls
        {
            get
            {
                return _controls;
            }
            set
            {
                _controls = value;
            }
        }
    }
}
