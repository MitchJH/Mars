﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Mars
{
    public class MainMenu
    {
        private Texture2D _backgroundImage;
        private Random rand = new Random();
        private bool showMenu = false;
        private float fadeIn = -0.01f;

        private Form _menuForm;
        private Rectangle menuRectangle;
        private Label _quote;

        public MainMenu(Rectangle screen, ContentManager content)
        {
            _backgroundImage = content.Load<Texture2D>("Textures/UI/mars_bg");

            Texture2D menuTexture = content.Load<Texture2D>("Textures/UI/menu");
            Point menuPos = new Point((screen.Width / 2) - (menuTexture.Width / 2), (screen.Height / 2) - (menuTexture.Height / 2));
            menuRectangle = new Rectangle(menuPos.X, menuPos.Y - 30, menuTexture.Width, menuTexture.Height);
            _menuForm = new Form("menuForm", "", menuRectangle, menuTexture, Fonts.Standard, Color.Black);

            Texture2D buttonTexture = content.Load<Texture2D>("Textures/UI/button");
            Point buttonSize = new Point(240, 40);
            Point buttonPos = new Point(40, 40);
            SpriteFont buttonFont = Fonts.Get("Menu");
            
            // NEW GAME BUTTON
            Button _newGame = new Button("new_game", "New Game",
                new Rectangle(40, 90, buttonSize.X, buttonSize.Y),
                buttonTexture, buttonFont, Color.Black);

            _newGame.onClick += new EHandler(NewGame_Click);
            _newGame.onMouseEnter += new EHandler(Beep);
            _menuForm.AddControl(_newGame);

            // LOAD GAME BUTTON
            Button _loadGame = new Button("load_game", "Load Game",
                new Rectangle(40, 150, buttonSize.X, buttonSize.Y),
                buttonTexture, buttonFont, Color.Black);

            _loadGame.onClick += new EHandler(LoadGame_Click);
            _loadGame.onMouseEnter += new EHandler(Beep);
            _menuForm.AddControl(_loadGame);

            // SETTINGS BUTTON
            Button _settings = new Button("settings", "Settings",
                new Rectangle(40, 210, buttonSize.X, buttonSize.Y),
                buttonTexture, buttonFont, Color.Black);

            _settings.onClick += new EHandler(Settings_Click);
            _settings.onMouseEnter += new EHandler(Beep);
            _menuForm.AddControl(_settings);

            // EXIT BUTTON
            Button _exit = new Button("exit", "Exit",
                new Rectangle(40, 270, buttonSize.X, buttonSize.Y),
                buttonTexture, buttonFont, Color.Black);

            _exit.onClick += new EHandler(Exit_Click);
            _exit.onMouseEnter += new EHandler(Beep);
            _menuForm.AddControl(_exit);

            // VERSION LABEL
            Vector2 stringSizeVersion = Fonts.Get("Tiny").MeasureString(Version.GetVersion());
            Vector2 labelPosVersion = new Vector2(160 - (stringSizeVersion.X / 2), menuTexture.Height - 20);
            Label _labelVersion = new Label("version", Version.GetVersion(), labelPosVersion, Fonts.Get("Tiny"), Color.White, Version.GetVersion().Length, 0);
            _menuForm.AddControl(_labelVersion);

            // COPYRIGHT
            float stringSize = Fonts.Get("Quote").MeasureString(Constants.GAME_COPYRIGHT).X;
            _quote = new Label("copyright", Constants.GAME_COPYRIGHT, new Vector2((screen.Width / 2) - (stringSize / 2), screen.Height - 20), Fonts.Get("Quote"), Color.White * 0.5f, Constants.GAME_COPYRIGHT.Length, 0);

            // MAIN MENU MUSIC
            Audio.Repeat = true;
            Audio.PlayMusicTrack("main_menu");
        }

        public void Update()
        {
            if (showMenu)
            {
                _menuForm.Update(Controls.Mouse, Controls.Keyboard);
            }

            if (fadeIn < 1.0f)
            {
                fadeIn = MathHelper.Lerp(fadeIn, 1.0f, 0.005f);

                if (fadeIn > 0.95f)
                {
                    fadeIn = 1.0f;
                }
                else if (fadeIn > 0.4f)
                {
                    showMenu = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle backgroundSize = GameStateManager.ENGINE.GraphicsDevice.Viewport.Bounds;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            spriteBatch.Draw(_backgroundImage, backgroundSize, Color.White * fadeIn);

            if (showMenu)
            {
                _menuForm.Draw(spriteBatch);
                _quote.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        private void NewGame_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("high_double_beep");
            GameStateManager.State = GameState.GameWorld;
        }

        private void LoadGame_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("low_double_beep");
        }

        private void Settings_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("low_double_beep");
        }

        private void Exit_Click(GUIControl sender)
        {
            GameStateManager.State = GameState.Exit;
        }

        private void Beep(GUIControl sender)
        {
            Audio.PlaySoundEffect("high_beep");
        }
    }
}