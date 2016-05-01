﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mars
{
    public static class DebugConsole
    {
        private static TextBox consoleTextBox;
        private static Rectangle consoleSize;
        private static bool _enabled;
        private static string _last;

        static DebugConsole()
        {
            consoleSize = new Rectangle(0, 0, 1920, 16);
            consoleTextBox = new TextBox("console", "", 64, consoleSize, Sprites.PIXEL, Fonts.Get("Console"), Color.White, Color.Black);
            consoleTextBox.onEnterPressed = new EHandler(CheckCommand);
            consoleTextBox.onRepeatLast = new EHandler(RepeatLast);
            _enabled = false;
        }

        private static void CheckCommand(GUIControl sender)
        {
            string command = sender.Text.ToLower();
            _last = command;
            sender.Text = "";
            bool actualCommand = true;

            //TODO:Add more robust way of creating/handling console commands
            if (command == "exit")
            {
                GameStateManager.State = GameState.Exit;
            }
            else if (command == "quit")
            {
                GameStateManager.State = GameState.Exit;
            }
            else if (command == "rendermode 1")
            {
                Settings.RenderMode = TileRenderMode.Cartesian;
            }
            else if (command == "rendermode 2")
            {
                Settings.RenderMode = TileRenderMode.IsometricDiamond;
            }
            else if (command == "rendermode 3")
            {
                Settings.RenderMode = TileRenderMode.IsometricStaggered;
            }
            else if (command == "fullscreen")
            {
                GameStateManager.ENGINE.Window.IsBorderless = false;

                if (GameStateManager.ENGINE.GRAPHICS.IsFullScreen == false)
                {
                    GameStateManager.ENGINE.GRAPHICS.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    GameStateManager.ENGINE.GRAPHICS.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    GameStateManager.ENGINE.GRAPHICS.ApplyChanges();
                    GameStateManager.ENGINE.GRAPHICS.ToggleFullScreen();
                }
            }
            else if (command == "borderless")
            {
                GameStateManager.ENGINE.Window.IsBorderless = true;

                if (GameStateManager.ENGINE.GRAPHICS.IsFullScreen == false)
                {
                    GameStateManager.ENGINE.GRAPHICS.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    GameStateManager.ENGINE.GRAPHICS.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    GameStateManager.ENGINE.GRAPHICS.ApplyChanges();
                    GameStateManager.ENGINE.GRAPHICS.ToggleFullScreen();
                }
            }
            else if (command == "windowed")
            {
                GameStateManager.ENGINE.Window.IsBorderless = false;

                if (GameStateManager.ENGINE.GRAPHICS.IsFullScreen == true)
                {
                    GameStateManager.ENGINE.GRAPHICS.PreferredBackBufferWidth = Settings.X_resolution;
                    GameStateManager.ENGINE.GRAPHICS.PreferredBackBufferHeight = Settings.Y_resolution;
                    GameStateManager.ENGINE.GRAPHICS.ApplyChanges();
                    GameStateManager.ENGINE.GRAPHICS.ToggleFullScreen();
                }
            }
            else if (command == "clear")
            {
                GameStateManager.ENGINE.WORLD.ClearGrid();
            }
            else if (command == "camerapos")
            {
                string cam_pos = "X: " + Camera.Position.X + ", Y: " + Camera.Position.Y + ", Z: " + Camera.Zoom;
                consoleTextBox.Text = cam_pos;
                actualCommand = false;
            }
            else if(command == "debug")
            {
                if(DebugTextManager.Enabled)
                {
                    DebugTextManager.Disable();
                }
                else
                {
                    DebugTextManager.Enable();
                }
            }
            else
            {
                actualCommand = false;
            }

            if (actualCommand)
            {
                _enabled = !Enabled;
                consoleTextBox.Focus = Enabled;
            }
        }

        private static void RepeatLast(GUIControl sender)
        {
            if (string.IsNullOrEmpty(_last) == false)
            {
                sender.Text = _last;
            }
        }

        public static void Update(GameTime gameTime)
        {
            if (Controls.KeyboardOld.IsKeyDown(Constants.CONSOLE_KEY) && Controls.Keyboard.IsKeyUp(Constants.CONSOLE_KEY))
            {
                _enabled = !Enabled;
                consoleTextBox.Focus = Enabled;
            }

            if (Enabled)
            {
                consoleTextBox.Update(Controls.Mouse, Controls.Keyboard);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (Enabled)
            {
                spriteBatch.Begin();
                consoleTextBox.Draw(spriteBatch);
                spriteBatch.DrawRectangle(consoleSize, Color.White);
                spriteBatch.End();
            }
        }

        public static void Enable()
        {
            _enabled = true;
        }

        public static void Disable()
        {
            _enabled = false;
        }

        public static bool Enabled
        {
            get
            {
                return _enabled;
            }
        }
    }
}
