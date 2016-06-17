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

        private static MousePosition _mousePosition;

        static Controls()
        {
            _mousePosition = new MousePosition();
        }

        public static void Update()
        {
            _oldMouseState = _currentMouseState;
            _oldKeyboardState = _currentKeyboardState;

            _currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            _currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            _mousePosition.Update(_currentMouseState);
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
        /// Helper class to get various Mouse Position translations.
        /// </summary>
        public static MousePosition MousePosition
        {
            get
            {
                return _mousePosition;
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

    public class MousePosition
    {
        private Vector2 _mouseCartesianPosition;
        private Vector2 _mouseIsometricPosition;
        private Vector2 _mouseScreenPosition;

        public MousePosition()
        {
            _mouseCartesianPosition = Vector2.Zero;
            _mouseIsometricPosition = Vector2.Zero;
            _mouseScreenPosition = Vector2.Zero;
        }

        public void Update(MouseState currentMouseState)
        {
            _mouseScreenPosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            _mouseCartesianPosition = Vector2.Transform(_mouseScreenPosition, Camera.InverseTransform);
            _mouseIsometricPosition = new Vector2(_mouseCartesianPosition.X + _mouseCartesianPosition.Y, _mouseCartesianPosition.Y - _mouseCartesianPosition.X);
        }

        /// <summary>
        /// The position of the mouse in Cartesian World coordinates.
        /// (The Camera Matrix Transformation HAS been applied)
        /// </summary>
        public Vector2 Cartesian
        {
            get
            {
                return _mouseCartesianPosition;
            }
        }

        /// <summary>
        /// The position of the mouse in Isometric World coordinates.
        /// (The coordinates have been converted from Cartesian to Isometric)
        /// </summary>
        public Vector2 Isometric
        {
            get
            {
                return _mouseIsometricPosition;
            }
        }

        /// <summary>
        /// The position of the mouse on the screen.
        /// (The Camera Matrix Transformation has NOT been applied)
        /// </summary>
        public Vector2 Screen
        {
            get
            {
                return _mouseScreenPosition;
            }
        }
    }
}
