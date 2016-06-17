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
    public class TileMap
    {
        private Tile[,] _tiles;

        public TileMap(int width, int height)
        {
            _tiles = new Tile[width, height];
            this.ClearTiles();
        }

        public void Update(GameTime gameTime)
        {            
            foreach (Tile t in _tiles)
            {
                t.Hovered = false;
            }

            Point hoveredTileLocation = TileAtCoordinates(Controls.MousePosition.Cartesian);
            Tile hoveredTile = TileAtPosition(hoveredTileLocation);

            // Check we are within the TileMap
            if (hoveredTile != null)
            {
                hoveredTile.Hovered = true;

                // Only apply the following logic in the World GameMode
                if (GameStateManager.Mode == GameMode.World)
                {
                    if (Controls.Mouse.RightButton == ButtonState.Pressed)
                    {
                        hoveredTile.Type = TileType.Impassable;
                    }
                    else if (Controls.Mouse.LeftButton == ButtonState.Pressed)
                    {
                        hoveredTile.Type = TileType.Passable;
                    }
                }
            }

            if (GameStateManager.Mode == GameMode.Build)
            {
                BuildModeManager.Update(this, hoveredTileLocation);
            }
            else if (GameStateManager.Mode == GameMode.Pipe)
            {
                PipeModeManager.Update(ref _tiles, hoveredTileLocation);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);

            if (GameStateManager.Mode == GameMode.Build)
            {
            }
            else if (GameStateManager.Mode == GameMode.Build)
            {
            }
            else if (GameStateManager.Mode == GameMode.Pipe)
            {
            }

            // DIAMOND MODE
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                for (int y = 0; y < Constants.MAP_HEIGHT; y++)
                {
                    Tile tile = _tiles[x, y];

                    double tileX = x * Constants.TILE_WIDTH / 2 - y * Constants.TILE_WIDTH / 2;
                    double tileY = x * Constants.TILE_HEIGHT / 2 + y * Constants.TILE_HEIGHT / 2;

                    Rectangle tileRectangle = new Rectangle(
                                (int)tileX,
                                (int)tileY + Constants.TILE_DEPTH,
                                Constants.TILE_WIDTH,
                                Constants.TILE_WIDTH + Constants.TILE_DEPTH);

                    if (tile.Type != TileType.Impassable)
                    {
                        if (tile.Hovered)
                        {
                            spriteBatch.Draw(Sprites.Get("tile"), tileRectangle, Color.Green);
                        }
                        else
                        {
                            spriteBatch.Draw(Sprites.Get("tile"), tileRectangle, Color.White);
                        }
                    }
                    else
                    {
                        if (tile.Hovered)
                        {
                            spriteBatch.Draw(Sprites.Get("tile_wall"), tileRectangle, Color.Green);
                        }
                        else
                        {
                            spriteBatch.Draw(Sprites.Get("tile_wall"), tileRectangle, Color.LightGray);
                        }
                    }

                    //spriteBatch.DrawRectangle(tileRectangle, Color.White * 0.5f);
                    //spriteBatch.DrawString(Fonts.Standard, tileX + ":" + tileY, new Vector2(tileRectangle.X, tileRectangle.Y+(Constants.TILE_WIDTH/2)), Color.White * 0.7f);
                }
            }

            spriteBatch.End();
        }

        public void ClearTiles()
        {
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                for (int y = 0; y < Constants.MAP_HEIGHT; y++)
                {
                    _tiles[x, y] = new Tile(x, y);
                    _tiles[x, y].Type = TileType.Passable;
                }
            }
        }

        public void HighlightTile(int x, int y)
        {
            if (x >= 0 && x < Constants.MAP_WIDTH)
            {
                if (y >= 0 && y < Constants.MAP_HEIGHT)
                {
                    _tiles[x, y].Hovered = true;
                }
            }
        }
        
        /// <summary>
        /// Returns the Tile position at a given set of coordinates
        /// </summary>
        /// <param name="cartesian">The cartesian coordinates to find the tile at</param>
        /// <returns>The tiles index in the TileMap</returns>
        public Point TileAtCoordinates(Vector2 cartesian)
        {            
            double xTile = cartesian.X / Constants.TILE_WIDTH;
            double yTile = (cartesian.Y - Constants.TILE_DEPTH - Constants.TILE_HEIGHT) / Constants.TILE_HEIGHT;

            int hitX = (int)((xTile + yTile) - 0.5);
            int hitY = (int)((yTile - xTile) + 0.5);

            Point tilePos = new Point(hitX, hitY);
            return tilePos;
        }

        /// <summary>
        /// Returns a Tile object at the given position in the tile array if one exists
        /// </summary>
        /// <param name="position">The position in the Tile array</param>
        /// <returns>The Tile at the position, or null if an invalid position if given</returns>
        public Tile TileAtPosition(Point position)
        {
            if (IsPositionInTilemap(position))
            {
                return _tiles[position.X, position.Y];
            }
            return null;
        }

        /// <summary>
        /// Return whether a given position is a valid location in the bounds of the Tile array
        /// </summary>
        /// <param name="position">The position in the Tile array</param>
        /// <returns>True if valid, false if not</returns>
        public bool IsPositionInTilemap(Point position)
        {
            if (position.X >= 0 && position.X < Constants.MAP_WIDTH)
            {
                if (position.Y >= 0 && position.Y < Constants.MAP_HEIGHT)
                {
                    return true;
                }
            }
            return false;
        }

        public Tile[,] Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }
    }
}
