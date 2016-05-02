using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mars
{
    public static class DebugTextManager
    {
        private static bool _enabled;
        private static string _headerText;
        private static string _text;
        private static SpriteFont _font;
        private static Vector2 _position;

        static DebugTextManager()
        {
            _enabled = Settings.DebugOn;
            _headerText = "Debug Text, console 'debug' to disable/enable. ";
            _text = _headerText;
            _font = Fonts.Standard;
            _position = Vector2.Zero;
        }

        public static void Enable()
        {
            _enabled = true;
        }

        public static void Disable()
        {
            _enabled = false;
        }

        public static void SetPosition(Vector2 pos)
        {
            _position = pos;
        }

        public static void Update(GameTime gameTime)
        {
            if (Settings.DebugOn)
            {
                _text = _headerText;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (Settings.DebugOn)
            {
                spriteBatch.Begin();

                spriteBatch.DrawString(_font, _text, _position + new Vector2(1, 1), Color.Black);
                spriteBatch.DrawString(_font, _text, _position, Color.Green);
                
                spriteBatch.End();
            }
        }

        public static void AddWatcher(object watcher, string description)
        {
            string line = description + " " + watcher.ToString();
            AddLine(line);
        }

        public static void AddLine(string newLine)
        {
            _text += "\n";
            _text += newLine;
        }

        public static bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public static int LikelyTextWidth
        {
            get
            {
                return (int)_font.MeasureString("Debug: Testing debug length.").X + 1;
            }
        }
    }
}
