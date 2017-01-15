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

        private static Vector2 _mouseWorldPosition;
        private static Point _mouseWorldTilePosition;
        private static Vector2 _mouseScreenPosition;

        static Controls()
        {
            _mouseWorldPosition = Vector2.Zero;
            _mouseWorldTilePosition = Point.Zero;
            _mouseScreenPosition = Vector2.Zero;
        }

        public static void Update()
        {
            _oldMouseState = _currentMouseState;
            _oldKeyboardState = _currentKeyboardState;

            _currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            _currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            if (_currentMouseState.IsInCameraView())
            {
                _mouseScreenPosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);
                _mouseWorldPosition = Vector2.Transform(_mouseScreenPosition, Camera.InverseTransform);
                
                _mouseWorldTilePosition = new Point(
                    (int)Math.Floor(_mouseWorldPosition.X / Constants.TILE_WIDTH),
                    (int)Math.Floor(_mouseWorldPosition.Y / Constants.TILE_HEIGHT));
            }
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

        /// <summary>
        /// The position of the mouse in Cartesian World coordinates.
        /// (The Camera Matrix Transformation HAS been applied)
        /// </summary>
        public static Vector2 MouseWorldPosition
        {
            get
            {
                return _mouseWorldPosition;
            }
        }

        /// <summary>Returns the game tile the mouse is currently over.</summary>
        public static Point MouseWorldTilePosition
        {
            get
            {
                return _mouseWorldTilePosition;
            }
        }

        /// <summary>
        /// The position of the mouse on the screen.
        /// (The Camera Matrix Transformation has NOT been applied)
        /// </summary>
        public static Vector2 MouseScreenPosition
        {
            get
            {
                return _mouseScreenPosition;
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
