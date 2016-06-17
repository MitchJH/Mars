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
        private static BuildingType _selectedBuilding;

        static BuildModeManager()
        {
        }

        public static void Update(TileMap tileMap, Point hoveredTile)
        {
            // Update the hovered tiles
            tileMap.HighlightTile(hoveredTile.X, hoveredTile.Y - 2);
            tileMap.HighlightTile(hoveredTile.X, hoveredTile.Y - 1);
            tileMap.HighlightTile(hoveredTile.X, hoveredTile.Y + 1);
            tileMap.HighlightTile(hoveredTile.X, hoveredTile.Y + 2);

            tileMap.HighlightTile(hoveredTile.X + 1, hoveredTile.Y - 2);
            tileMap.HighlightTile(hoveredTile.X + 1, hoveredTile.Y - 1);
            tileMap.HighlightTile(hoveredTile.X + 1, hoveredTile.Y);
            tileMap.HighlightTile(hoveredTile.X + 1, hoveredTile.Y + 1);
            tileMap.HighlightTile(hoveredTile.X + 1, hoveredTile.Y + 2);

            tileMap.HighlightTile(hoveredTile.X - 1, hoveredTile.Y - 2);
            tileMap.HighlightTile(hoveredTile.X - 1, hoveredTile.Y - 1);
            tileMap.HighlightTile(hoveredTile.X - 1, hoveredTile.Y);
            tileMap.HighlightTile(hoveredTile.X - 1, hoveredTile.Y + 1);
            tileMap.HighlightTile(hoveredTile.X - 1, hoveredTile.Y + 2);

            if (Controls.LeftClick)
            {
                bool buildSuccess = AttemptConstruction();

                if (buildSuccess)
                {
                    // CREATE THE BUILDING
                    CreateBuilding();

                    // Exit the build mode manager if the Left Shift key is not down.
                    if (Controls.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) == false)
                    {
                        GameStateManager.Mode = GameMode.World;
                    }
                }
            }
            else if (Controls.RightClick)
            {
                // Exit this GameMode
                GameStateManager.Mode = GameMode.World;
            }
        }

        private static bool AttemptConstruction()
        {
            Audio.PlaySoundEffect("low_double_beep");
            return false;
        }

        private static void CreateBuilding()
        {
        }
    }
}
