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
        private int _ID;
        private BuildingType _buildingType;

        public Building(int ID, string buildingType, Vector2 tilePosition, Vector2 size)
            : base(new Vector2(tilePosition.X * Constants.TILE_WIDTH, tilePosition.Y * Constants.TILE_WIDTH), size)
        {
            _ID = ID;
            _buildingType = BuildingManager.GetType(buildingType);
        }

        #region PROPERTIES
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public BuildingType BuildingType
        {
            get { return _buildingType; }
            set { _buildingType = value; }
        }
        #endregion
    }
}
