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

            HighlightTile(ref tilemap, hoveredTile);
        

            if (Controls.LeftClick)
            {
                bool placed = AttemptConstruction(ref tilemap, hoveredTile);

                if (placed)
                {
                    // LAY THE PIPE
                    PlacePipe(ref tilemap, hoveredTile);

                    // This keeps the pipe mode manager active if the Left Shift key is down. 
                    // Probably not the best user experience.
                    //_active = Controls.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
                }
            }
            else if (Controls.RightClick)
            {
                _active = false;
            }

            return _active;
        }

        private static bool AttemptConstruction(ref Tile[,] tilemap, Point hoveredTile)
        {
            //If within bounds of tile map.
            if (hoveredTile.X >= 0 && hoveredTile.X < Constants.MAP_WIDTH)
            {
                if (hoveredTile.Y >= 0 && hoveredTile.Y < Constants.MAP_HEIGHT)
                {
                    tilemap[hoveredTile.X, hoveredTile.Y].Hovered = true;

                    //If user is trying to place pipe on impassable tile.
                    if (tilemap[hoveredTile.X, hoveredTile.Y].Type == TileType.Impassable)
                    {
                        Audio.PlaySoundEffect("low_double_beep");
                        return false;
                    }
                    else
                    {
                        PlacePipe(ref tilemap, hoveredTile);
                    }
                }
            }

            Audio.PlaySoundEffect("low_double_beep");
            return false;
        }

        private static void PlacePipe(ref Tile[,] tilemap, Point hoveredTile)
        {
            tilemap[hoveredTile.X, hoveredTile.Y].Pipe = new Pipe(PipeType.Water);
            Console.WriteLine("PipePlaced");
        }

        private static void HighlightTile(ref Tile[,] tilemap, Point hoveredTile)
        {
            if (hoveredTile.X >= 0 && hoveredTile.X < Constants.MAP_WIDTH)
            {
                if (hoveredTile.Y >= 0 && hoveredTile.Y < Constants.MAP_HEIGHT)
                {
                    tilemap[hoveredTile.X, hoveredTile.Y].Hovered = true;

                    if (tilemap[hoveredTile.X, hoveredTile.Y].Type == TileType.Impassable)
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
