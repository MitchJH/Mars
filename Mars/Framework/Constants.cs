using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public static class Constants
    {
        // GAME
        /// <summary>The folder name used by the game in the appdata folder.</summary>
        public const string APP_DATA_GAME_NAME = "Mars Game";
        /// <summary>The copyright displayed in the game.</summary>
        public const string GAME_COPYRIGHT = "Copyright © DAM Games 2016";
        /// <summary>The background colour for the renderer.</summary>
        public readonly static Color BACK_COLOUR = Color.Black;//Color.FromNonPremultiplied(100, 100, 100, 255);

        // TILE MAP
        /// <summary>The width of a game tile.</summary>
        public const int TILE_WIDTH = 128;
        /// <summary>The height of a game tile.</summary>
        public const int TILE_HEIGHT = 64;
        /// <summary>Half of a tiles width to offset the row below.</summary>
        public const int ROW_OFFSET = TILE_WIDTH / 2;
        /// <summary>Quater of a tiles height to offset the next row.</summary>
        public const int ROW_STEP = TILE_WIDTH / 4;
        /// <summary>Excess height at the bottom of a tile to give a depth effect.</summary>
        public const int TILE_DEPTH = 10;
        /// <summary>Width of the map in tiles.</summary>
        public const int MAP_WIDTH = 40;
        /// <summary>Height of the map in tiles.</summary>
        public const int MAP_HEIGHT = 40;

        // CAMERA
        // Zoom
        /// <summary>Speed of the camera when zooming in.</summary>
        public const float CAMERA_ZOOM_SPEED = 5.0f;
        /// <summary>The inertia value used in the camera movement lerp function.</summary>
        public const float CAMERA_ZOOM_INERTIA = 0.1f;
        /// <summary>Maximum magnification of the camera.</summary>
        public const float CAMERA_MAX_ZOOM_IN = 1.0f; // 1.5f
        /// <summary>Minimum magnification of the camera.</summary>
        public const float CAMERA_MAX_ZOOM_OUT = 0.2f;
        // Scroll
        /// <summary>The distance the camera will travel per second.</summary>
        public const float CAMERA_SCROLL_SPEED = 400.0f;
        /// <summary>THe intertia value used in the camera zooming lerp function.</summary>
        public const float CAMERA_SCROLL_INERTIA = 0.1f;
        /// <summary>The pixels in from the edge of the screen that count as scrolling for the cursor.</summary>
        public const int CAMERA_EDGE_SCROLL_SIZE = 2;
        /// <summary>Minimum X coordinate the camera can travel to.</summary>
        public const int CAMERA_BOUNDS_MIN_X = -(TILE_WIDTH * 10);
        /// <summary>Minimum Y coordinate the camera can travel to.</summary>
        public const int CAMERA_BOUNDS_MIN_Y = -(TILE_WIDTH * 10);
        /// <summary>Maximum X coordinate the camera can travel to.</summary>
        public const int CAMERA_BOUNDS_MAX_X = (TILE_WIDTH * MAP_WIDTH) - (TILE_WIDTH * 10);
        /// <summary>Maximum Y coordinate the camera can travel to.</summary>
        public const int CAMERA_BOUNDS_MAX_Y = (TILE_WIDTH * MAP_HEIGHT) - (TILE_WIDTH * 10);

        // DEBUG CONSOLE
        /// <summary>The key used to open the in-game debug console.</summary>
        public const Keys CONSOLE_KEY = Keys.OemTilde;

        static Constants()
        {
        }
    }
}
