using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Game
    {
        public GraphicsDeviceManager GRAPHICS;
        public SpriteBatch SPRITEBATCH;

        public World WORLD;
        public MainMenuUI MAIN_MENU;

        public Engine(string[] args)
        {
            this.GRAPHICS = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Load the settings file
            Settings.LoadFromFile();
            // Parse any command line arguments
            Settings.ParseCommandLineArguments(args);

            // Give GSM access to the Engine
            GameStateManager.ENGINE = this;
            // Set starting game state
            GameStateManager.State = Settings.StartingGameState;

            // Set mouse visibility and fixed timestep
            this.IsMouseVisible = Settings.IsMouseVisible;
            this.IsFixedTimeStep = Settings.IsFixedTimestep;

            // Set the resolution
            this.GRAPHICS.PreferredBackBufferWidth = Settings.X_resolution;
            this.GRAPHICS.PreferredBackBufferHeight = Settings.Y_resolution;

            // Adjust window mode
            if (Settings.WindowMode == WindowMode.Fullscreen)
            {
                this.GRAPHICS.IsFullScreen = true;
                this.Window.IsBorderless = false;
            }
            else if (Settings.WindowMode == WindowMode.Borderless)
            {
                this.GRAPHICS.IsFullScreen = false;
                this.Window.IsBorderless = true;
            }
            else
            {
                this.GRAPHICS.IsFullScreen = false;
                this.Window.IsBorderless = false;
            }
        }

        protected override void Initialize()
        {
            // Create the camera
            Camera.Create(GraphicsDevice.Viewport);
            Camera.Position = new Vector2(-422, 924);
            Camera.Zoom = 0.366f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SPRITEBATCH = new SpriteBatch(GraphicsDevice);

            // Load the base textures
            Sprites.MISSING_TEXTURE = Content.Load<Texture2D>("Textures/missing");
            Sprites.PIXEL = Content.Load<Texture2D>("Textures/pixel");
            Sprites.MOUSE_MAP = Content.Load<Texture2D>("Textures/mousemap");

            // Load the localization file
            Localization.LoadLocalization();

            // Load all fonts
            Fonts.LoadFontBank("Content/Data/fontbank.txt", Content);

            // Load all game sprites
            Sprites.LoadSpriteBank("Content/Data/spritebank.txt", Content);

            // Load all sound effects
            Audio.LoadSoundBank("Content/Data/soundbank.txt", Content);

            // Load all object types
            ObjectManager.LoadObjectBank("Content/Data/objectbank.txt", Content);

            // Load all building types
            BuildingManager.LoadBuildingBank("Content/Data/buildingbank.txt", Content);

            // Enable the FPS counter
            FrameRateCounter.Enable();
            FrameRateCounter.SetPosition(new Vector2(GraphicsDevice.Viewport.Width - FrameRateCounter.LikelyTextWidth, 0));

            // Enable the DebugTextManager
            DebugTextManager.SetPosition(new Vector2(1, 50));

            // Enable version display
            Version.Enable();

            // Create Game State Objects
            WORLD = new World(Content);
            MAIN_MENU = new MainMenuUI("Main Menu", GraphicsDevice.Viewport.Bounds, Content);
        }

        protected override void UnloadContent()
        {
            Settings.WriteToFile();
        }

        protected override void Update(GameTime gameTime)
        {
            Controls.Update();
            DebugConsole.Update(gameTime);

            if (GameStateManager.State == GameState.GameWorld)
            {
                WORLD.Update(gameTime);
            }
            else if (GameStateManager.State == GameState.MainMenu)
            {
                MAIN_MENU.Update();
            }

            if (GameStateManager.State == GameState.Exit)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (GameStateManager.State == GameState.GameWorld)
            {
                GraphicsDevice.Clear(Constants.BACK_COLOUR);
                WORLD.Draw(SPRITEBATCH);
                FrameRateCounter.Draw(SPRITEBATCH);
                DebugTextManager.Draw(SPRITEBATCH);
            }
            else if (GameStateManager.State == GameState.MainMenu)
            {
                GraphicsDevice.Clear(Color.Black);
                MAIN_MENU.Draw(SPRITEBATCH);
            }
            
            DebugConsole.Draw(SPRITEBATCH);

            base.Draw(gameTime);
        }
    }
}
