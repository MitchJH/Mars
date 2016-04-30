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

        static GameStateManager()
        {
            _engine = null;
            _state = GameState.MainMenu;
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
    }

    public enum GameState
    {
        GameWorld,
        MainMenu,
        Exit
    }
}
