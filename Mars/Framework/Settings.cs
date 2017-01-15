using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mars
{
    public static class Settings
    {
        private static string _appDataPath;
        // General Settings
        private static WindowMode _windowMode;
        private static GameState _startingGameState;
        private static int _resolutionX;
        private static int _resolutionY;
        private static int _window_width;
        private static int _window_height;
        private static int _windowPosX;
        private static int _windowPosY;
        private static bool _mouseVisible;
        private static bool _fixedTimestep;
        private static bool _mouseScrolling;
        // Audio Settings
        private static float _masterVolume;
        private static bool _effectsOn;
        private static float _effectVolume;
        private static bool _musicOn;
        private static float _musicVolume;
        // Debug Settings
        private static bool _debugOn;

        static Settings()
        {
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.APP_DATA_GAME_NAME);

            // General
            _windowMode = WindowMode.Fullscreen;
            _startingGameState = GameState.MainMenu;
            _resolutionX = screenWidth;
            _resolutionY = screenHeight;
            _windowPosX = 0;
            _windowPosY = 0;
            _mouseVisible = true;
            _fixedTimestep = false;
            _mouseScrolling = true;
            // Audio
            _masterVolume = 1.0f;
            _effectsOn = true;
            _effectVolume = 1.0f;
            _musicOn = true;
            _musicVolume = 0.5f;
            // Debug
            _debugOn = false;
        }

        private static void ParseSettings(List<string> settings)
        {
            foreach (string setting in settings)
            {
                if (setting.StartsWith("#") == false && string.IsNullOrEmpty(setting) == false)
                {
                    string[] split = setting.Split(' ');
                    string value = split[1];

                    if (setting.StartsWith("window_mode"))
                    {
                        _windowMode = (WindowMode)Enum.Parse(typeof(WindowMode), value);
                    }
                    else if (setting.StartsWith("starting_game_state"))
                    {
                        _startingGameState = (GameState)Enum.Parse(typeof(GameState), value);
                    }
                    else if (setting.StartsWith("x_res"))
                    {
                        _resolutionX = int.Parse(value);
                    }
                    else if (setting.StartsWith("y_res"))
                    {
                        _resolutionY = int.Parse(value);
                    }
                    else if (setting.StartsWith("window_width"))
                    {
                        _window_width = int.Parse(value);
                    }
                    else if (setting.StartsWith("window_height"))
                    {
                        _window_height = int.Parse(value);
                    }
                    else if (setting.StartsWith("x_pos"))
                    {
                        _windowPosX = int.Parse(value);
                    }
                    else if (setting.StartsWith("y_pos"))
                    {
                        _windowPosY = int.Parse(value);
                    }
                    else if (setting.StartsWith("mouse_visible"))
                    {
                        _mouseVisible = bool.Parse(value);
                    }
                    else if (setting.StartsWith("fixed_timestep"))
                    {
                        _fixedTimestep = bool.Parse(value);
                    }
                    else if (setting.StartsWith("mouse_scrolling"))
                    {
                        _mouseScrolling = bool.Parse(value);
                    }
                    else if (setting.StartsWith("master_volume"))
                    {
                        _masterVolume = MathHelper.Clamp(float.Parse(value), 0.0f, 1.0f);
                    }
                    else if (setting.StartsWith("effects_on"))
                    {
                        _effectsOn = bool.Parse(value);
                    }
                    else if (setting.StartsWith("effect_volume"))
                    {
                        _effectVolume = MathHelper.Clamp(float.Parse(value), 0.0f, 1.0f);
                    }
                    else if (setting.StartsWith("music_on"))
                    {
                        _musicOn = bool.Parse(value);
                    }
                    else if (setting.StartsWith("music_volume"))
                    {
                        _musicVolume = MathHelper.Clamp(float.Parse(value), 0.0f, 1.0f);
                    }
                    else if (setting.StartsWith("debug_mode"))
                    {
                        _debugOn = bool.Parse(value);
                    }
                }
            }
        }

        public static void LoadFromFile()
        {
            if (Directory.Exists(_appDataPath))
            {
                string file = Path.Combine(_appDataPath, @"Settings.txt");

                if (File.Exists(file))
                {
                    string[] lines = File.ReadAllLines(file);
                    ParseSettings(lines.ToList());
                }
            }
            else
            {
                Directory.CreateDirectory(_appDataPath);
            }
        }

        public static void WriteToFile()
        {
            // Collect settings text
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# GENERAL SETTINGS");
            sb.AppendLine("window_mode " + _windowMode.ToString() + GetEnumListString(_windowMode));
            sb.AppendLine("starting_game_state " + _startingGameState.ToString() + GetEnumListString(_startingGameState));
            sb.AppendLine("x_res " + _resolutionX.ToString());
            sb.AppendLine("y_res " + _resolutionY.ToString());
            sb.AppendLine("window_width " + _window_width.ToString());
            sb.AppendLine("window_height " + _window_height.ToString());
            sb.AppendLine("x_pos " + _windowPosX.ToString());
            sb.AppendLine("y_pos " + _windowPosY.ToString());
            sb.AppendLine("mouse_visible " + _mouseVisible.ToString());
            sb.AppendLine("fixed_timestep " + _fixedTimestep.ToString());
            sb.AppendLine("mouse_scrolling " + _mouseScrolling.ToString());
            sb.AppendLine("");
            sb.AppendLine("# AUDIO SETTINGS");
            sb.AppendLine("master_volume " + _masterVolume.ToString("N2"));
            sb.AppendLine("effects_on " + _effectsOn.ToString());
            sb.AppendLine("effect_volume " + _effectVolume.ToString("N2"));
            sb.AppendLine("music_on " + _musicOn.ToString());
            sb.AppendLine("music_volume " + _musicVolume.ToString("N2"));
            sb.AppendLine("");
            sb.AppendLine("# DEBUG SETTINGS");
            sb.AppendLine("debug_mode " + _debugOn.ToString());

            if (Directory.Exists(_appDataPath) == false)
            {
                Directory.CreateDirectory(_appDataPath);
            }

            string file = Path.Combine(_appDataPath, @"Settings.txt");

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            File.WriteAllText(file, sb.ToString());
        }

        public static void ParseCommandLineArguments(string[] args)
        {
            List<string> settings = new List<string>();

            foreach (string arg in args)
            {
                string fixed_arg = arg.Replace("=", " ");
                settings.Add(fixed_arg);
            }

            ParseSettings(settings);
        }

        private static string GetEnumListString(Enum e)
        {
            string enumList = " //";

            foreach (Enum wm in Enum.GetValues(e.GetType()))
            {
                enumList += " " + wm.ToString() + ",";
            }

            enumList = enumList.Substring(0, enumList.Length - 1);

            return enumList;
        }

        #region PROPERTIES
        public static WindowMode WindowMode
        {
            get { return Settings._windowMode; }
            set { Settings._windowMode = value; }
        }

        public static GameState StartingGameState
        {
            get { return Settings._startingGameState; }
            set { Settings._startingGameState = value; }
        }

        public static int ResolutionX
        {
            get { return Settings._resolutionX; }
            set { Settings._resolutionX = value; }
        }

        public static int ResolutionY
        {
            get { return Settings._resolutionY; }
            set { Settings._resolutionY = value; }
        }

        public static int Window_Width
        {
            get { return Settings._window_width; }
            set { Settings._window_width = value; }
        }

        public static int Window_Height
        {
            get { return Settings._window_height; }
            set { Settings._window_height = value; }
        }

        public static int WindowPosX
        {
            get { return Settings._windowPosX; }
            set { Settings._windowPosX = value; }
        }

        public static int WindowPosY
        {
            get { return Settings._windowPosY; }
            set { Settings._windowPosY = value; }
        }

        public static bool IsMouseVisible
        {
            get { return Settings._mouseVisible; }
            set { Settings._mouseVisible = value; }
        }

        public static bool IsFixedTimestep
        {
            get { return Settings._fixedTimestep; }
            set { Settings._fixedTimestep = value; }
        }

        public static bool MouseScrolling
        {
            get { return Settings._mouseScrolling; }
            set { Settings._mouseScrolling = value; }
        }

        public static float MasterVolume
        {
            get { return Settings._masterVolume; }
            set { Settings._masterVolume = value; }
        }

        public static bool EffectsOn
        {
            get { return Settings._effectsOn; }
            set { Settings._effectsOn = value; }
        }

        public static float EffectVolume
        {
            get { return Settings._effectVolume; }
            set { Settings._effectVolume = value; }
        }

        public static bool MusicOn
        {
            get { return Settings._musicOn; }
            set { Settings._musicOn = value; }
        }

        public static float MusicVolume
        {
            get { return Settings._musicVolume; }
            set { Settings._musicVolume = value; }
        }

        public static bool DebugOn
        {
            get { return Settings._debugOn; }
            set { Settings._debugOn = value; }
        }

        public static string AppDataPath
        {
            get { return _appDataPath; }
        }
        #endregion
    }

    public static class Tweaker
    {
        private static List<string> _tweakers = new List<string>();

        static Tweaker()
        {
            Load();
        }

        public static bool Exists(string tweaker)
        {
            return _tweakers.Contains(tweaker);
        }

        private static void Load()
        {            
            if (Directory.Exists(Settings.AppDataPath))
            {
                string file = Path.Combine(Settings.AppDataPath, @"Tweakers.txt");

                if (File.Exists(file))
                {
                    string[] lines = File.ReadAllLines(file);

                    foreach (string line in lines)
                    {
                        _tweakers.Add(line);
                    }
                }
            }
        }

        public static void Add(string tweaker)
        {
            _tweakers.Add(tweaker);
        }

        public static void Remove(string tweaker)
        {
            // The lambda expression is the predicate required
            _tweakers.RemoveAll(item => item == tweaker);
        }
    }

    public enum WindowMode
    {
        Windowed,
        Borderless,
        Fullscreen
    }
}
