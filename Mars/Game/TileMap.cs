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
        private Tile _hoveredTile;

        public TileMap(int width, int height)
        {
            _tiles = new Tile[width, height];
            this.ClearTiles();
        }

        public void Update(GameTime gameTime)
        {
            if (_hoveredTile != null)
            {
                _hoveredTile.Hovered = false;
            }

            if (IsPositionInTilemap(Controls.MouseWorldTilePosition))
            {
                _hoveredTile = _tiles[Controls.MouseWorldTilePosition.X, Controls.MouseWorldTilePosition.Y];
                _hoveredTile.Hovered = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                for (int y = 0; y < Constants.MAP_HEIGHT; y++)
                {
                    Tile tile = _tiles[x, y];

                    double tileX = x * Constants.TILE_WIDTH;
                    double tileY = y * Constants.TILE_HEIGHT;

                    Rectangle tileRectangle = new Rectangle((int)tileX, (int)tileY, Constants.TILE_WIDTH, Constants.TILE_HEIGHT);

                    if (tile.Type != TileType.Impassable)
                    {
                        if (tile.Hovered)
                        {
                            spriteBatch.Draw(Sprites.Get("Tile"), tileRectangle, Color.Green);
                        }
                        else
                        {
                            spriteBatch.Draw(Sprites.Get("Tile"), tileRectangle, Color.White);
                        }
                    }
                    else
                    {
                        if (tile.Hovered)
                        {
                            spriteBatch.Draw(Sprites.Get("Tile_Wall"), tileRectangle, Color.DarkGreen);
                        }
                        else
                        {
                            spriteBatch.Draw(Sprites.Get("Tile_Wall"), tileRectangle, Color.Gray);
                        }
                    }

                    //spriteBatch.DrawRectangle(tileRectangle, Color.White * 0.5f);
                    //spriteBatch.DrawString(Fonts.Get("tiny"), x + ":" + y, new Vector2(tileRectangle.X, tileRectangle.Y+(Constants.TILE_WIDTH/2)), Color.White * 0.7f);
                }
            }

            foreach (Line line in Pathfinding.DebugLines)
            {
                spriteBatch.DrawLine(line.Start, line.End, Color.Black);
            }
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
            double yTile = cartesian.Y / Constants.TILE_HEIGHT;

            Point tilePos = new Point((int)xTile, (int)yTile);
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

        public List<Tile> TilesAroundPosition(Point position, int depth = 1, bool includeCenter = false)
        {
            List<Tile> tiles = new List<Tile>();

            int minX = position.X - depth;
            int maxX = position.X + depth;

            int minY = position.Y - depth;
            int maxY = position.Y + depth;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Point tilePos = new Point(x, y);
                    Tile tile = TileAtPosition(tilePos);

                    if (tile != null)
                    {
                        if (position != tilePos || includeCenter == true)
                        {
                            tiles.Add(tile);
                        }
                    }
                }
            }
            return tiles;
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

        public Tile HoveredTile
        {
            get { return _hoveredTile; }
        }
    }
}
