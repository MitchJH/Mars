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
    public static class PipeModeManager
    {
        private static bool _active;
        private static PipeType _selectedPipe;
        private static bool _blocked;

        static PipeModeManager()
        {
        }

        public static void EnterMode()
        {
            _active = true;
        }

        public static bool Update(ref Tile[,] tilemap, Point hoveredTile)
        {
            Blocked = false;

            if (Controls.LeftClick)
            {
                bool placed = AttemptConstruction();

                if (placed)
                {
                    // LAY THE PIPE
                    PlacePipe();

                    // This keeps the pipe mode manager active if the Left Shift key is down.
                    _active = Controls.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
                }
            }
            else if (Controls.RightClick)
            {
                _active = false;
            }

            return _active;
        }

        private static bool AttemptConstruction()
        {
            Audio.PlaySoundEffect("low_double_beep");
            return false;
        }

        private static void PlacePipe()
        {
            Console.WriteLine("PipePlaced");
        }

        private static void HighlightExtraTile(ref Tile[,] tilemap, int x, int y)
        {
            if (x >= 0 && x < Constants.MAP_WIDTH)
            {
                if (y >= 0 && y < Constants.MAP_HEIGHT)
                {
                    tilemap[x, y].Hovered = true;

                    if (tilemap[x, y].Type == TileType.Impassable)
                    {
                        Blocked = true;
                    }
                }
            }
        }

        public static bool Blocked
        {
            get
            {
                return _blocked;
            }

            set
            {
                _blocked = value;
            }
        }
    }
}
