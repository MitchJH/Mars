using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public class Building : Entity
    {
        private string _key; // The unique identifier of this object
        private BuildingType _buildingType; // What type of object this is

        public Building(string key, string buildingType, Vector2 tilePosition)
            : base()
        {
            _key = key;
            _buildingType = BuildingManager.GetType(buildingType);
            base.TilePosition = tilePosition; // Set the tile position of the entity
            base.Position = new Vector2(tilePosition.X * Constants.TILE_WIDTH, tilePosition.Y * Constants.TILE_WIDTH);
        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public BuildingType BuildingType
        {
            get { return _buildingType; }
            set { _buildingType = value; }
        }
    }
}
