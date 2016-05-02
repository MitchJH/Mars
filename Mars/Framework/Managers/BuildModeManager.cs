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
    public static class BuildModeManager
    {
        private static bool _active;
        private static BuildingType _selectedBuilding;
        private static bool _blocked;

        static BuildModeManager()
        {
        }

        public static void EnterMode()
        {
            _active = true;
        }

        public static bool Update(ref Tile[,] tilemap, Point hoveredTile)
        {
            Blocked = false;

            // Update the hovered tiles
            HighlightExtraTile(ref tilemap, hoveredTile.X, hoveredTile.Y - 2);
            HighlightExtraTile(ref tilemap, hoveredTile.X, hoveredTile.Y - 1);
            HighlightExtraTile(ref tilemap, hoveredTile.X, hoveredTile.Y + 1);
            HighlightExtraTile(ref tilemap, hoveredTile.X, hoveredTile.Y + 2);

            HighlightExtraTile(ref tilemap, hoveredTile.X + 1, hoveredTile.Y - 2);
            HighlightExtraTile(ref tilemap, hoveredTile.X + 1, hoveredTile.Y - 1);
            HighlightExtraTile(ref tilemap, hoveredTile.X + 1, hoveredTile.Y);
            HighlightExtraTile(ref tilemap, hoveredTile.X + 1, hoveredTile.Y + 1);
            HighlightExtraTile(ref tilemap, hoveredTile.X + 1, hoveredTile.Y + 2);

            HighlightExtraTile(ref tilemap, hoveredTile.X - 1, hoveredTile.Y - 2);
            HighlightExtraTile(ref tilemap, hoveredTile.X - 1, hoveredTile.Y - 1);
            HighlightExtraTile(ref tilemap, hoveredTile.X - 1, hoveredTile.Y);
            HighlightExtraTile(ref tilemap, hoveredTile.X - 1, hoveredTile.Y + 1);
            HighlightExtraTile(ref tilemap, hoveredTile.X - 1, hoveredTile.Y + 2);

            if (Controls.LeftClick)
            {
                bool placed = AttemptConstruction();

                if (placed)
                {
                    // CREATE THE BUILDING
                    CreateBuilding();

                    // This keeps the build mode manager active if the Left Shift key is down.
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

        private static void CreateBuilding()
        {
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
