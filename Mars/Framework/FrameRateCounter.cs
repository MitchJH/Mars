﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mars
{
    public static class FrameRateCounter
    {
        private static bool _enabled;
        private static int _frameRate = 0;
        private static int _frameCounter = 0;
        private static TimeSpan _elapsedTime;
        private static SpriteFont _font;
        private static Vector2 _position;

        static FrameRateCounter()
        {
            _enabled = false;
            _frameRate = 0;
            _frameCounter = 0;
            _elapsedTime = TimeSpan.Zero;
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
            if (_enabled)
            {
                _elapsedTime += gameTime.ElapsedGameTime;

                if (_elapsedTime > TimeSpan.FromSeconds(1))
                {
                    _elapsedTime -= TimeSpan.FromSeconds(1);
                    _frameRate = _frameCounter;
                    _frameCounter = 0;
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (_enabled)
            {
                _frameCounter++;

                string fps = string.Format("FPS: {0}", _frameRate);
                spriteBatch.Begin();

                if (_frameRate >= 50)
                {
                    spriteBatch.DrawString(_font, fps, _position + new Vector2(1, 1), Color.Black);
                    spriteBatch.DrawString(_font, fps, _position, Color.Green);
                }
                else if (_frameRate < 30)
                {
                    spriteBatch.DrawString(_font, fps, _position + new Vector2(1, 1), Color.Black);
                    spriteBatch.DrawString(_font, fps, _position, Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(_font, fps, _position + new Vector2(1, 1), Color.Black);
                    spriteBatch.DrawString(_font, fps, _position, Color.Orange);
                }

                spriteBatch.End();
            }
        }

        public static bool Enabled
        {
            get { return _enabled; }
        }

        public static int FrameRate
        {
            get { return _frameRate; }
            set { _frameRate = value; }
        }

        public static int LikelyTextWidth
        {
            get
            {
                return (int)_font.MeasureString("FPS: 123").X + 1;
            }
        }
    }
}
