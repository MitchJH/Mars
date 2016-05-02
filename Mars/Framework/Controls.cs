using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public static class Controls
    {
        private static MouseState _currentMouseState;
        private static KeyboardState _currentKeyboardState;

        private static MouseState _oldMouseState;
        private static KeyboardState _oldKeyboardState;

        private static Vector2 _gameMousePosition;

        static Controls()
        {
        }

        public static void Update()
        {
            _oldMouseState = _currentMouseState;
            _oldKeyboardState = _currentKeyboardState;

            _currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            _currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            Vector2 mousePosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);
            _gameMousePosition = Vector2.Transform(mousePosition, Camera.InverseTransform);
        }

        public static MouseState Mouse
        {
            get
            {
                return _currentMouseState;
            }
        }

        public static KeyboardState Keyboard
        {
            get
            {
                return _currentKeyboardState;
            }
        }

        public static MouseState MouseOld
        {
            get
            {
                return _oldMouseState;
            }
        }

        public static KeyboardState KeyboardOld
        {
            get
            {
                return _oldKeyboardState;
            }
        }

        public static Vector2 GameMousePosition
        {
            get
            {
                return _gameMousePosition;
            }
        }

        public static bool LeftClick
        {
            get
            {
                if (_oldMouseState.LeftButton == ButtonState.Pressed && _currentMouseState.LeftButton == ButtonState.Released)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool RightClick
        {
            get
            {
                if (_oldMouseState.RightButton == ButtonState.Pressed && _currentMouseState.RightButton == ButtonState.Released)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
