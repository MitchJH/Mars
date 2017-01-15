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
    public static class Pathfinding
    {
        public static List<Line> DebugLines = new List<Line>();

        static Pathfinding()
        {
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
                return new LinkedList<Tile>();
            }
            else if (start.X > Constants.MAP_WIDTH || start.Y > Constants.MAP_HEIGHT || end.X > Constants.MAP_WIDTH || end.Y > Constants.MAP_HEIGHT)
            {
                return new LinkedList<Tile>();
            }
            else if (start == end)
            {
                return new LinkedList<Tile>();
            }

            // Generate a search map from our worlds tiles
            SpatialAStar<Tile, Object> aStar = new SpatialAStar<Tile, Object>(GameStateManager.ENGINE.WORLD.TileMap.Tiles);

            // Begin the search and return the result
            LinkedList<Tile> tempPath = aStar.Search(start, end, extraContext);

            if (Constants.WAYPOINT_CULLING || Tweaker.Exists("force_path_culling") || Tweaker.Exists("force_waypoint_culling"))
            {
                // Cull all tiles in the path that aren't needed
                LinkedList<Tile> culledPath = CullPath(tempPath);
                return culledPath;
            }

            return tempPath;
        }

        private static LinkedList<Tile> CullPath2(LinkedList<Tile> path)
        {
            DebugLines.Clear();
            LinkedList<Tile> culledPath = new LinkedList<Tile>(path);

            // Iterate through each node in the path, we test each node to see if it can be removed
            // We test this by casting a ray between the node before and after it, if this rays path
            // is traversable then we know we don't need the node in question and we can remove it

            // Example Node Iteration Visualization
            // [0] [1] [2] [3] [4] [5] [6] [7] [8]
            // [N] [Y] [Y] [Y] [Y] [Y] [Y] [Y] [N]


            bool firstStepSkippable = CanMoveBetweenTiles(path.ElementAt(0), path.ElementAt(1));

            if (firstStepSkippable == true)
            {
                // If we can walk between the start and first waypoint get rid of the start
                culledPath.RemoveFirst();
            }

            // Start at the second node to avoid trying to get rid of the first waypoint in our path as we know it's needed (see above)
            // End at the node before the last waypoint to avoid trying to get rid of our end destination, we'll always need that
            for (int i = 1; i < path.Count - 1; i++)
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

        private static LinkedList<Tile> CullPath(LinkedList<Tile> pathR)
        {
            DebugLines.Clear();
            LinkedList<Tile> path = new LinkedList<Tile>(pathR);

            bool firstStepSkippable = CanMoveBetweenTiles(path.ElementAt(0), path.ElementAt(1));

            if (firstStepSkippable == true)
            {
                // If we can walk between the start and first waypoint get rid of the start
                path.RemoveFirst();
            }

            if (path.Count > 1)
            {
                int curI = 1;
                Tile checkPoint = path.First();

                Tile currentPoint = path.ElementAt(curI);

                while (curI + 1 < path.Count)
                {
                    if (CanMoveBetweenTiles(checkPoint, path.ElementAt(curI + 1)))
                    {
                        Tile temp = currentPoint;
                        currentPoint = path.ElementAt(curI + 1);
                        path.Remove(temp);
                    }
                    else
                    {
                        checkPoint = currentPoint;
                        currentPoint = path.ElementAt(curI + 1);
                        curI++;
                    }
                }
            }

            return path;
        }

        /*
        while (currentPoint->next != NULL)
            if Walkable(checkPoint, currentPoint->next)
                // Make a straight path between those points:
                temp = currentPoint
                currentPoint = currentPoint->next
                delete temp from the path
            else
                checkPoint = currentPoint
                currentPoint = currentPoint->next
        */

        private static bool CanMoveBetweenTiles(Tile start, Tile end)
        {
            List<List<Vector2>> bresenhamLines = new List<List<Vector2>>();
            bresenhamLines.Add(BresenhamLine(start.TopLeft, end.TopLeft));
            bresenhamLines.Add(BresenhamLine(start.TopRight, end.TopRight));
            bresenhamLines.Add(BresenhamLine(start.BottomLeft, end.BottomLeft));
            bresenhamLines.Add(BresenhamLine(start.BottomRight, end.BottomRight));

            DebugLines.Add(new Line(start.TopLeft, end.TopLeft));
            DebugLines.Add(new Line(start.TopRight, end.TopRight));
            DebugLines.Add(new Line(start.BottomLeft, end.BottomLeft));
            DebugLines.Add(new Line(start.BottomRight, end.BottomRight));

            foreach (List<Vector2> bresenhamLine in bresenhamLines)
            {
                foreach (Vector2 vector in bresenhamLine)
                {
                    Point tilePos = GameStateManager.ENGINE.WORLD.TileMap.TileAtCoordinates(vector);
                    Tile tile = GameStateManager.ENGINE.WORLD.TileMap.TileAtPosition(tilePos);

                    if (tile.Type == TileType.Impassable)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>Returns a list of Vectors in a line between two Vector positions</summary>
        public static List<Vector2> BresenhamLine(Vector2 start, Vector2 end)
        {
            List<Vector2> bresenhamLine = new List<Vector2>();
            int x = (int)start.X;
            int y = (int)start.Y;
            int x2 = (int)end.X;
            int y2 = (int)end.Y;

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                bresenhamLine.Add(new Vector2(x, y));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else {
                    x += dx2;
                    y += dy2;
                }
            }
            return bresenhamLine;
        }
    }

    public class Line
    {
        public Vector2 Start;
        public Vector2 End;

        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }
    }
}
