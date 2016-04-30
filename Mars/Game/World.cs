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
    /// <summary>
    /// Holds the current gamestate, used for savegame serialization.
    /// </summary>
    public class World
    {
        private static Tile[,] _tiles;
        private static Clock _clock;
        private static List<CrewMember> _crewMembers;
        private static List<GameObject> _objects;
        private static List<Building> _buildings;
        private static Planet _planet;
     
        //REMOVE //TEST
        private bool active_selection = false;
        Texture2D tile_texture;
        Texture2D iso_texture;
        Texture2D iso_texture_wall;

        public World(ContentManager content)
        {
            _tiles = new Tile[Constants.MAP_WIDTH, Constants.MAP_HEIGHT];
            _clock = new Clock();
            _crewMembers = new List<CrewMember>();
            _objects = new List<GameObject>();
            _buildings = new List<Building>();
            this.ClearGrid();

            //REMOVE
            tile_texture = content.Load<Texture2D>("Textures/tile");
            iso_texture = content.Load<Texture2D>("Textures/iso");
            iso_texture_wall = content.Load<Texture2D>("Textures/isob");
            _crewMembers.Add(new CrewMember("darren", 23, 800, 1500, "crew"));
            
        }

        public void ClearGrid()
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

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            World.Clock.Update(gameTime);
            EntityCollider.Collide(gameTime);

            if (Controls.Mouse.IsInCameraView()) // Don't do anything with the mouse if it's not in our cameras viewport.
            {
                Vector2 mousePosition = Controls.GameMousePosition;

                // REMOVE/EDIT
                foreach (Tile t in _tiles)
                {
                    t.Hovered = false;
                }

                Point hoveredTile = Point.Zero;

                if (Settings.RenderMode == TileRenderMode.Cartesian)
                {
                    hoveredTile = Tile.WorldToMapCell(new Point((int)mousePosition.X, (int)mousePosition.Y));
                }
                else if (Settings.RenderMode == TileRenderMode.IsometricDiamond)
                {
                    hoveredTile = Tile.WorldToMapCell(new Point((int)mousePosition.X, (int)mousePosition.Y - Constants.TILE_DEPTH - Constants.TILE_HEIGHT));
                }
                else if (Settings.RenderMode == TileRenderMode.IsometricStaggered)
                {
                    hoveredTile = Tile.WorldToMapCell(new Point((int)mousePosition.X, (int)mousePosition.Y - Constants.TILE_DEPTH));
                }

                if (hoveredTile.X >= 0 && hoveredTile.Y >= 0 && hoveredTile.X < Constants.MAP_WIDTH && hoveredTile.Y < Constants.MAP_HEIGHT)
                {
                    if (Controls.Mouse.RightButton == ButtonState.Pressed)
                    {
                        _tiles[hoveredTile.X, hoveredTile.Y].Texture = "CLIFF";
                        _tiles[hoveredTile.X, hoveredTile.Y].Type = TileType.Impassable;
                    }
                    else if (Controls.Mouse.LeftButton == ButtonState.Pressed)
                    {
                        _tiles[hoveredTile.X, hoveredTile.Y].Texture = "";
                        _tiles[hoveredTile.X, hoveredTile.Y].Hovered = true;
                        _tiles[hoveredTile.X, hoveredTile.Y].Type = TileType.Passable;
                    }
                    else
                    {
                        _tiles[hoveredTile.X, hoveredTile.Y].Hovered = true;
                    }
                }

                //Update crew in real time.
                foreach (CrewMember c in World.CrewMembers)
                {
                    c.Update(gameTime);
                }

                if (Controls.Mouse.LeftButton == ButtonState.Pressed && Controls.MouseOld.LeftButton == ButtonState.Released)
                {
                    foreach (CrewMember c in World.CrewMembers)
                    {
                        //If the mouseposition is within the textures bounds of a crew member...
                        //This could be done radius based instead of texture bounds based to make it simpler, but might not work then for long rectangular objects such as solar panels or something.
                        if (mousePosition.X > c.Position.X - (Sprites.MISSING_TEXTURE.Bounds.Width / 2) && mousePosition.X < c.Position.X + (Sprites.MISSING_TEXTURE.Bounds.Width / 2))
                        {
                            if (mousePosition.Y > c.Position.Y - (Sprites.MISSING_TEXTURE.Bounds.Height / 2) && mousePosition.Y < c.Position.Y + (Sprites.MISSING_TEXTURE.Bounds.Height / 2))
                            {
                                //deselect any crew that are already selected.
                                foreach (CrewMember cm in World.CrewMembers)
                                {
                                    if (cm.Selected)
                                    {
                                        cm.Selected = false;
                                    }
                                }
                                //TODO: Some more formal UI class will need to handle when things are selected, not the object itself.
                                c.Selected = true;
                                active_selection = true;

                                Audio.PlaySoundEffect("high_beep");
                                Console.WriteLine(c.Name + " " + " has been selected");

                                break;
                            }
                        }
                    }
                }
                else if (Controls.Mouse.RightButton == ButtonState.Pressed && Controls.MouseOld.RightButton == ButtonState.Released)
                {
                    if (active_selection)
                    {
                        foreach (CrewMember cm in World.CrewMembers)
                        {
                            if (cm.Selected)
                            {
                                Point start_location = new Point((int)cm.Position.X / Constants.TILE_WIDTH, (int)cm.Position.Y / Constants.TILE_WIDTH);
                                Point destination = Controls.GameMousePosition.ToPoint();
                                Console.WriteLine("Generating Path from point (" + start_location.X + "," + start_location.Y + ") to point (" + destination.X + "," + destination.Y + ")");

                                LinkedList<Tile> path = World.FindPath(start_location, destination);
                                cm.DeterminePath(path);
                            }
                        }
                    }
                }
            }

            FrameRateCounter.Update(gameTime);

            // Check for exit.
            if (Controls.Keyboard.IsKeyDown(Keys.Escape))
            {
                GameStateManager.State = GameState.MainMenu;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);

            if (Settings.RenderMode == TileRenderMode.Cartesian)
            {
                // CARTESIAN MODE
                for (int x = 0; x < Constants.MAP_WIDTH; x++)
                {
                    for (int y = 0; y < Constants.MAP_HEIGHT; y++)
                    {
                        Tile tile = World.Tiles[x, y];

                        double xx = x * Constants.TILE_HEIGHT;
                        double yy = y * Constants.TILE_HEIGHT;

                        Rectangle rec = new Rectangle(
                                    (int)xx,
                                    (int)yy,
                                    Constants.TILE_HEIGHT,
                                    Constants.TILE_HEIGHT);

                        if (tile.Texture != "CLIFF")
                        {
                            if (tile.Hovered)
                            {
                                spriteBatch.Draw(tile_texture, rec, Color.Green);
                            }
                            else
                            {
                                spriteBatch.Draw(tile_texture, rec, Color.White);
                            }
                        }
                        else
                        {
                            if (tile.Hovered)
                            {
                                spriteBatch.Draw(tile_texture, rec, Color.Green);
                            }
                            else
                            {
                                spriteBatch.Draw(tile_texture, rec, Color.LightGray);
                            }
                        }
                    }
                }
            }
            else if (Settings.RenderMode == TileRenderMode.IsometricDiamond)
            {
                // DIAMOND MODE
                for (int x = 0; x < Constants.MAP_WIDTH; x++)
                {
                    for (int y = 0; y < Constants.MAP_HEIGHT; y++)
                    {
                        Tile tile = World.Tiles[x, y];

                        double tile_pos_x = x * Constants.TILE_WIDTH / 2 - y * Constants.TILE_WIDTH / 2;
                        double tile_pos_y = x * Constants.TILE_HEIGHT / 2 + y * Constants.TILE_HEIGHT / 2;

                        Rectangle rec = new Rectangle(
                                    (int)tile_pos_x,
                                    (int)tile_pos_y + Constants.TILE_DEPTH,
                                    Constants.TILE_WIDTH,
                                    Constants.TILE_WIDTH + Constants.TILE_DEPTH);

                        if (tile.Texture != "CLIFF")
                        {
                            if (tile.Hovered)
                            {
                                spriteBatch.Draw(iso_texture, rec, Color.Green);
                            }
                            else
                            {
                                spriteBatch.Draw(iso_texture, rec, Color.White);
                            }
                        }
                        else
                        {
                            if (tile.Hovered)
                            {
                                spriteBatch.Draw(iso_texture_wall, rec, Color.Green);
                            }
                            else
                            {
                                spriteBatch.Draw(iso_texture_wall, rec, Color.LightGray);
                            }
                        }

                        //spriteBatch.DrawRectangle(rec, Color.White);
                        //spriteBatch.DrawString(Fonts.Standard, xx + ":" + yy, new Vector2(rec.X, rec.Y+(Constants.TILE_WIDTH/2)), Color.White);
                    }
                }


                //REMOVE


                double tile_pos_x2 = _crewMembers[0].Position.X / 2 - _crewMembers[0].Position.Y / 2;
                double tile_pos_y2 = _crewMembers[0].Position.X / 2 + _crewMembers[0].Position.Y / 2;

                Rectangle rec2 = new Rectangle(
                            (int)tile_pos_x2,
                            (int)tile_pos_y2 + Constants.TILE_DEPTH,
                            Constants.TILE_WIDTH,
                            Constants.TILE_WIDTH + Constants.TILE_DEPTH);

                spriteBatch.Draw(Sprites.Get("crew"), rec2, Color.White);

               

            }
            else if (Settings.RenderMode == TileRenderMode.IsometricStaggered)
            {
                // STAGGERED MODE
                for (int y = 0; y < Constants.MAP_HEIGHT; y++)
                {
                    int rowOffset = 0;
                    if ((y) % 2 == 1)
                    {
                        rowOffset = Constants.ROW_OFFSET;
                    }

                    for (int x = 0; x < Constants.MAP_WIDTH; x++)
                    {
                        Tile tile = World.Tiles[x, y];

                        Rectangle rec = new Rectangle(
                                    (x * Constants.TILE_WIDTH) + rowOffset,
                                    (y * Constants.ROW_STEP) + Constants.TILE_DEPTH,
                                    Constants.TILE_WIDTH,
                                    Constants.TILE_WIDTH + Constants.TILE_DEPTH);

                        if (tile.Texture != "CLIFF")
                        {
                            if (tile.Hovered)
                            {
                                spriteBatch.Draw(iso_texture, rec, Color.Green);
                            }
                            else
                            {
                                spriteBatch.Draw(iso_texture, rec, Color.White);
                            }
                        }
                        else
                        {
                            if (tile.Hovered)
                            {
                                spriteBatch.Draw(iso_texture_wall, rec, Color.Green);
                            }
                            else
                            {
                                spriteBatch.Draw(iso_texture_wall, rec, Color.LightGray);
                            }
                        }
                    }
                }
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Find a path through the world
        /// </summary>
        /// <param name="start">The tile to start the path from</param>
        /// <param name="end">The tile the path should end at</param>
        /// <param name="extraContext">Optional: Allows the passing of extra information to the IsWalkable function in the Tile object
        /// thus allowing a tile to be walkable for different entities by only changing input parameters rather than gamestate</param>
        /// <returns>A linked list of Tile objects that is the best path between the start and end tiles</returns>
        public static LinkedList<Tile> FindPath(Point start, Point end, Object extraContext = null)
        {
            if (start.X < 0 || start.Y < 0 || end.X < 0 || end.Y < 0)
            {
                return null;
            }
            else if (start.X > Constants.MAP_WIDTH || start.Y > Constants.MAP_HEIGHT || end.X > Constants.MAP_WIDTH || end.Y > Constants.MAP_HEIGHT)
            {
                return null;
            }

            // Generate a search map from our worlds tiles
            SpatialAStar<Tile, Object> aStar = new SpatialAStar<Tile, Object>(_tiles);

            // Begin the search and return the result
            LinkedList<Tile> tempPath = aStar.Search(start, end, extraContext);

            // Cull all tiles in the path that aren't needed
            LinkedList<Tile> culledPath = CullPath(tempPath);

            return culledPath;
        }

        private static LinkedList<Tile> CullPath(LinkedList<Tile> path)
        {
            LinkedList<Tile> culledPath = new LinkedList<Tile>(path);

            // Iterate through each node in the path, we test each node to see if it can be removed
            // We test this by casting a ray between the node before and after it, if this rays path
            // is traversable then we know we don't need the node in question and we can remove it

            // Example Node Iteration Visualization
            // [0] [1] [2] [3] [4] [5] [6] [7] [8]
            // [N] [Y] [Y] [Y] [Y] [Y] [Y] [Y] [N]

            // Start at the second node to avoid trying to get rid of the first waypoint in our path
            // End at the node before the last waypoint to avoid trying to get rid of our end destination
            for (int i = 1; i < path.Count-1; i++)
            {
                // If we can walk between the node before and after the current node then we don't need the current node
                bool shouldRemove = CanMoveBetweenTiles(path.ElementAt(i - 1), path.ElementAt(i + 1));

                if (shouldRemove == true)
                {
                    culledPath.Remove(path.ElementAt(i));
                }
            }

            return culledPath;
        }

        private static bool CanMoveBetweenTiles(Tile start, Tile end)
        {
            List<Vector2> hits = BresenhamLine(start.Center, end.Center);
            return (hits.Count == 0);
        }

        // Returns the list of points between two Vector positions
        private static List<Vector2> BresenhamLine(Vector2 start, Vector2 end)
        {
            int x0 = (int)start.X;
            int y0 = (int)start.Y;
            int x1 = (int)end.X;
            int y1 = (int)end.Y;

            List<Vector2> result = new List<Vector2>();

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1; else ystep = -1;
            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    if (TileFromCoordinate(y, x).Type == TileType.Impassable)
                    {
                        result.Add(new Vector2(y, x));
                    }
                }
                else
                {
                    if (TileFromCoordinate(x, y).Type == TileType.Impassable)
                    {
                        result.Add(new Vector2(x, y));
                    }
                }
                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
            }

            return result;
        }
        private static void Swap<T>(ref T a, ref T b)
        {
            T c = a; a = b; b = c;
        }

        private static Tile TileFromCoordinate(float x, float y)
        {
            int x_t = (int)(x / Constants.TILE_WIDTH);
            int y_t = (int)(y / Constants.TILE_WIDTH);
            return _tiles[x_t, y_t];
        }

        // Properties
        public static Tile[,] Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        public static Clock Clock
        {
            get { return _clock; }
            set { _clock = value; }
        }

        public static List<CrewMember> CrewMembers
        {
            get { return _crewMembers; }
            set { _crewMembers = value; }
        }

        public static List<GameObject> Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        public static List<Building> Buildings
        {
            get { return World._buildings; }
            set { World._buildings = value; }
        }

        public static Planet Planet
        {
            get { return _planet; }
            set { _planet = value; }
        }
    }
}
