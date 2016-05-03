using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public enum TileType
    {
        Passable,
        Impassable
    }

    public class Tile : IPathNode<Object>
    {
        private Point _position;
        private Vector2 _center;
        private TileType _type;
        private bool _hovered;
        private Pipe _pipe;

        public Tile(int x, int y)
        {
            _position = new Point(x, y);
            _hovered = false;

            //CARTESIAN
            //_center = new Vector2((x * Constants.TILE_WIDTH) + (Constants.TILE_WIDTH / 2), (y * Constants.TILE_WIDTH) + (Constants.TILE_WIDTH / 2));

            //DIAMOND
            double tile_pos_x = x * Constants.TILE_WIDTH / 2 - y * Constants.TILE_WIDTH / 2;
            double tile_pos_y = x * Constants.TILE_HEIGHT / 2 + y * Constants.TILE_HEIGHT / 2;

            Rectangle rec = new Rectangle(
                        (int)tile_pos_x,
                        (int)tile_pos_y + Constants.TILE_DEPTH,
                        Constants.TILE_WIDTH,
                        Constants.TILE_WIDTH + Constants.TILE_DEPTH);

            float r1 = rec.X + (Constants.TILE_WIDTH / 2);
            float r2 = rec.Y + (Constants.TILE_WIDTH - Constants.TILE_HEIGHT / 2);

            _center = new Vector2(r1, r2);
        }
        
        public bool IsWalkable(Object unused)
        {
            if (_type == TileType.Passable)
            {
                return true;
            }            

            return false;
        }

        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Center
        {
            get { return _center; }
            set { _center = value; }
        }

        public TileType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public bool Hovered
        {
            get { return _hovered; }
            set { _hovered = value; }
        }

        public Pipe Pipe
        {
            get { return _pipe; }
            set { _pipe = value; }
        }

        public void AddNewPipe(PipeType type)
        {
            _pipe = new Pipe(type);
        }

        public static Point WorldToMapCell(Point worldPoint, out Point localPoint)
        {
            if (Settings.RenderMode == TileRenderMode.Cartesian)
            {
                double xTile = (double)worldPoint.X / (double)Constants.TILE_HEIGHT;
                double yTile = (double)worldPoint.Y / (double)Constants.TILE_HEIGHT;

                int hitX = (int)xTile;
                int hitY = (int)yTile;

                Console.WriteLine(hitX + ":" + hitY);
                localPoint = Point.Zero;
                return new Point(hitX, hitY);
            }
            else if (Settings.RenderMode == TileRenderMode.IsometricDiamond)
            {
                //int hitx = (int)(((double)pickX / (double)tileW) + ((double)pickY / (double)tileH) - 0.5);
                //int hity = (int)(((double)pickY / (double)tileH) - ((double)pickX / (double)tileW) + 0.5);

                double xTile = (double)worldPoint.X / (double)Constants.TILE_WIDTH;
                double yTile = (double)worldPoint.Y / (double)Constants.TILE_HEIGHT;

                int hitX = (int)((xTile + yTile) - 0.5);
                int hitY = (int)((yTile - xTile) + 0.5);

                //Console.WriteLine(hitX + ":" + hitY);
                localPoint = Point.Zero;
                return new Point(hitX, hitY);
            }
            else if (Settings.RenderMode == TileRenderMode.IsometricStaggered)
            {
                Point mapCell = new Point(
                   (int)(worldPoint.X / Sprites.MOUSE_MAP.Width),
                   ((int)(worldPoint.Y / Sprites.MOUSE_MAP.Height)) * 2
                   );

                int localPointX = worldPoint.X % Sprites.MOUSE_MAP.Width;
                int localPointY = worldPoint.Y % Sprites.MOUSE_MAP.Height;

                int dx = 0;
                int dy = 0;

                uint[] myUint = new uint[1];

                if (new Rectangle(0, 0, Sprites.MOUSE_MAP.Width, Sprites.MOUSE_MAP.Height).Contains(localPointX, localPointY))
                {
                    Sprites.MOUSE_MAP.GetData(0, new Rectangle(localPointX, localPointY, 1, 1), myUint, 0, 1);

                    if (myUint[0] == 0xFF0000FF) // Red
                    {
                        dx = -1;
                        dy = -1;
                        localPointX = localPointX + (Sprites.MOUSE_MAP.Width / 2);
                        localPointY = localPointY + (Sprites.MOUSE_MAP.Height / 2);
                    }

                    if (myUint[0] == 0xFF00FF00) // Green
                    {
                        dx = -1;
                        localPointX = localPointX + (Sprites.MOUSE_MAP.Width / 2);
                        dy = 1;
                        localPointY = localPointY - (Sprites.MOUSE_MAP.Height / 2);
                    }

                    if (myUint[0] == 0xFF00FFFF) // Yellow
                    {
                        dy = -1;
                        localPointX = localPointX - (Sprites.MOUSE_MAP.Width / 2);
                        localPointY = localPointY + (Sprites.MOUSE_MAP.Height / 2);
                    }

                    if (myUint[0] == 0xFFFF0000) // Blue
                    {
                        dy = +1;
                        localPointX = localPointX - (Sprites.MOUSE_MAP.Width / 2);
                        localPointY = localPointY - (Sprites.MOUSE_MAP.Height / 2);
                    }
                }

                mapCell.X += dx;
                mapCell.Y += dy - 2;

                localPoint = new Point(localPointX, localPointY);

                return mapCell;
            }
            else
            {
                localPoint = Point.Zero;
                return Point.Zero;
            }
        }

        public static Point WorldToMapCell(Point worldPoint)
        {
            Point dummy;
            return WorldToMapCell(worldPoint, out dummy);
        }
    }
}
