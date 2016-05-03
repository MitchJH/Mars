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
    public class MainMenuUI : GameUI
    {
        private Random rand = new Random();
        private bool showMenu = false;
        private float fadeIn = -0.01f;

        private PictureBox _background;
        private Form _menuForm;
        private Rectangle menuRectangle;
        private Button _sfx_toggle;
        private Button _music_toggle;
        private Label _quote;

        private Texture2D soundTexture;
        private Texture2D soundTexture_off;
        private Texture2D musicTexture;
        private Texture2D musicTexture_off;

        public MainMenuUI(string ID, Rectangle screen, ContentManager content)
            : base(ID)
        {
            Texture2D backgroundImage = content.Load<Texture2D>("Textures/UI/mars_bg");
            _background = new PictureBox("mainMenuBackground", new Rectangle(100, 100, 100, 100), backgroundImage, SizeMode.CenterImage);

            Texture2D menuTexture = content.Load<Texture2D>("Textures/UI/menu");
            Point menuPos = new Point((screen.Width / 2) - (menuTexture.Width / 2), (screen.Height / 2) - (menuTexture.Height / 2));
            menuRectangle = new Rectangle(menuPos.X, menuPos.Y - 40, menuTexture.Width, menuTexture.Height);
            _menuForm = new Form("menuForm", "", menuRectangle, menuTexture, Fonts.Standard, Color.Black);
            _menuForm.Position = new Vector2(-menuRectangle.Width, _menuForm.Position.Y);

            Texture2D buttonTexture = content.Load<Texture2D>("Textures/UI/button");
            Point buttonSize = new Point(240, 40);
            Point buttonPos = new Point(40, 40);
            SpriteFont buttonFont = Fonts.Get("Menu");
            
            // NEW GAME BUTTON
            Button _newGame = new Button("new_game", "New Game",
                new Rectangle(40, 90, buttonSize.X, buttonSize.Y),
                buttonTexture, buttonFont, Color.Black);

            _newGame.onClick += new EHandler(NewGame_Click);
            _newGame.onMouseEnter += delegate{ Audio.PlaySoundEffect("high_beep"); };
            _menuForm.AddControl(_newGame);

            // LOAD GAME BUTTON
            Button _loadGame = new Button("load_game", "Load Game",
                new Rectangle(40, 150, buttonSize.X, buttonSize.Y),
                buttonTexture, buttonFont, Color.Black);

            _loadGame.onClick += new EHandler(LoadGame_Click);
            _loadGame.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            _menuForm.AddControl(_loadGame);

            // SETTINGS BUTTON
            Button _settings = new Button("settings", "Settings",
                new Rectangle(40, 210, buttonSize.X, buttonSize.Y),
                buttonTexture, buttonFont, Color.Black);

            _settings.onClick += new EHandler(Settings_Click);
            _settings.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            _menuForm.AddControl(_settings);

            // EXIT BUTTON
            Button _exit = new Button("exit", "Exit",
                new Rectangle(40, 270, buttonSize.X, buttonSize.Y),
                buttonTexture, buttonFont, Color.Black);

            _exit.onClick += new EHandler(Exit_Click);
            _exit.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            _menuForm.AddControl(_exit);

            // SOUND BUTTON
            soundTexture = content.Load<Texture2D>("Textures/UI/Buttons/sfx_toggle");
            soundTexture_off = content.Load<Texture2D>("Textures/UI/Buttons/sfx_toggle_off");
            _sfx_toggle = new Button("soundToggle", "",
                new Rectangle(2, 2, 20, 20),
                soundTexture, buttonFont, Color.Black);
            if (Settings.EffectsOn == false) _sfx_toggle.Texture = soundTexture_off;
            _sfx_toggle.onClick += new EHandler(ToggleEffects_Click);

            // MUSIC BUTTON
            musicTexture = content.Load<Texture2D>("Textures/UI/Buttons/music_toggle");
            musicTexture_off = content.Load<Texture2D>("Textures/UI/Buttons/music_toggle_off");
            _music_toggle = new Button("musicToggle", "",
                new Rectangle(25, 2, 20, 20),
                musicTexture, buttonFont, Color.Black);
            if (Settings.MusicOn == false) _music_toggle.Texture = musicTexture_off;
            _music_toggle.onClick += new EHandler(ToggleMusic_Click);

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
            Audio.PlayMusicTrack("main_menu_ogg");
        }

        public override void Update()
        {
            _background.PositionWidthHeight = GameStateManager.ENGINE.GraphicsDevice.Viewport.Bounds;
            _background.CalcRectangles();

            _menuForm.Position = new Vector2(
                (GameStateManager.ENGINE.GraphicsDevice.Viewport.Bounds.Width / 2) - (_menuForm.Width / 2),
                (GameStateManager.ENGINE.GraphicsDevice.Viewport.Bounds.Height / 2) - (_menuForm.Height / 2) - 40);


            float stringSize = Fonts.Get("Quote").MeasureString(Constants.GAME_COPYRIGHT).X;
            _quote.Position = new Vector2(
                (GameStateManager.ENGINE.GraphicsDevice.Viewport.Bounds.Width / 2) - (stringSize / 2),
                GameStateManager.ENGINE.GraphicsDevice.Viewport.Bounds.Height - 20);

            _quote.Text = Audio.SongPosition;

            if (showMenu)
            {
                if (_menuForm.Position.X < menuRectangle.X)
                {
                    float newPos = MathHelper.Lerp(_menuForm.Position.X, 370, 0.05f);
                    _menuForm.Position = new Vector2(newPos, _menuForm.Position.Y);
                }

                _menuForm.Update();
                _sfx_toggle.Update();
                _music_toggle.Update();
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            _background.DrawWithFade(spriteBatch, fadeIn);

            if (showMenu)
            {
                _menuForm.Draw(spriteBatch);
                _sfx_toggle.Draw(spriteBatch);
                _music_toggle.Draw(spriteBatch);
                _quote.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        private void NewGame_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("high_double_beep");
            GameStateManager.State = GameState.GameWorld;
            GameStateManager.Mode = GameMode.World;
        }

        private void LoadGame_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("low_double_beep");
        }

        private void Settings_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("low_double_beep");
        }

        private void ToggleEffects_Click(GUIControl sender)
        {
            Settings.EffectsOn = !Settings.EffectsOn;

            if (Settings.EffectsOn)
            {
                _sfx_toggle.Texture = soundTexture;
            }
            else
            {
                _sfx_toggle.Texture = soundTexture_off;
            }

            Audio.PlaySoundEffect("high_beep");
        }

        private void ToggleMusic_Click(GUIControl sender)
        {
            Settings.MusicOn = !Settings.MusicOn;

            if (Settings.MusicOn)
            {
                _music_toggle.Texture = musicTexture;
                Audio.PlayMusicTrack("main_menu");
            }
            else
            {
                _music_toggle.Texture = musicTexture_off;
                Audio.StopMusic();
            }

            Audio.PlaySoundEffect("high_beep");
        }

        private void Exit_Click(GUIControl sender)
        {
            GameStateManager.State = GameState.Exit;
        }
    }
}
