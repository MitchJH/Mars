using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public enum PipeType
    {
        Water,
        Oxygen
    }

    public class Pipe
    {
        private Point _tile_position;
        private PipeType _type;

        public Pipe(PipeType type)
        {
            _type = type;
        }
        

        public Point TilePosition
        {
            get { return _tile_position; }
            set { _tile_position = value; }
        }


        public PipeType Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
