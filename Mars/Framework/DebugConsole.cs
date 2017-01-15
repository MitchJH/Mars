using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

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
            string wholeCommand = sender.Text.ToLower();
            string[] commandList = wholeCommand.Split(' ');
            string command = commandList[0];
            _last = wholeCommand;
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
                    GameStateManager.ENGINE.GRAPHICS.PreferredBackBufferWidth = Settings.ResolutionX;
                    GameStateManager.ENGINE.GRAPHICS.PreferredBackBufferHeight = Settings.ResolutionY;
                    GameStateManager.ENGINE.GRAPHICS.ApplyChanges();
                    GameStateManager.ENGINE.GRAPHICS.ToggleFullScreen();
                }
            }
            else if (command == "clear")
            {
                GameStateManager.ENGINE.WORLD.TileMap.ClearTiles();
            }
            else if (command == "camerapos")
            {
                string cam_pos = "X: " + Camera.Position.X + ", Y: " + Camera.Position.Y + ", Z: " + Camera.Zoom;
                consoleTextBox.Text = cam_pos;
                actualCommand = false;
            }
            else if (command == "debug")
            {
                DebugTextManager.Enabled = !DebugTextManager.Enabled;
                Settings.DebugOn = DebugTextManager.Enabled;
            }
            else if (command == "tweaker")
            {
                if (string.IsNullOrEmpty(commandList[1]) == false)
                {
                    if (Tweaker.Exists(commandList[1]))
                    {
                        Tweaker.Remove(commandList[1]);
                    }
                    else
                    {
                        Tweaker.Add(commandList[1]);
                    }
                }
                else
                {
                    actualCommand = false;
                }
            }
            else if (command == "save")
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(World));

                XmlWriterSettings XML_Settings = new XmlWriterSettings { Indent = true };

                using (XmlWriter xml = XmlWriter.Create(Settings.AppDataPath + "\\save.xml", XML_Settings))
                {
                    serializer.WriteObject(xml, GameStateManager.ENGINE.WORLD);
                }
            }
            else if (command == "kill")
            {
                if (GameStateManager.ENGINE.WORLD.SelectedCrewMember != null)
                {
                    GameStateManager.ENGINE.WORLD.CrewMembers.Remove(GameStateManager.ENGINE.WORLD.SelectedCrewMember);
                }
            }
            else if (command == "move_to")
            {
                if (GameStateManager.ENGINE.WORLD.SelectedCrewMember != null)
                {
                    GameStateManager.ENGINE.WORLD.SelectedCrewMember.Path = Pathfinding.FindPath(GameStateManager.ENGINE.WORLD.SelectedCrewMember.TilePosition, Controls.MouseWorldTilePosition);
                }
            }
            else
            {
                actualCommand = false;
            }

            if (actualCommand)
            {
                _enabled = !_enabled;
                consoleTextBox.Focus = Enabled;
                Audio.PlaySoundEffect("high_double_beep");
            }
            else
            {
                Audio.PlaySoundEffect("low_double_beep");
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
                _enabled = !_enabled;
                consoleTextBox.Focus = _enabled;
            }

            if (_enabled)
            {
                consoleTextBox.Update();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (_enabled)
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
            get { return _enabled; }
        }
    }
}
