using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars
{
    public static class GameStateManager
    {
        private static Engine _engine;
        private static GameState _state;
        private static GameMode _mode;

        static GameStateManager()
        {
            _engine = null;
            _state = GameState.MainMenu;
            _mode = GameMode.World;
        }

        public static Engine ENGINE
        {
            get
            {
                return _engine;
            }
            set
            {
                if (_engine == null)
                {
                    _engine = value;
                }
            }
        }

        public static GameState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public static GameMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
    }

    public enum GameState
    {
        GameWorld,
        MainMenu,
        Exit
    }

    public enum GameMode
    {
        Menu,
        World,
        Build,
        Objects,
        Wire,
        Pipe
    }
}
