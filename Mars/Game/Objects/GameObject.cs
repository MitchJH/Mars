using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public class GameObject : Entity
    {
        private ObjectType _objectType; // What type of object this is

        private float _powerRequirement;    //How much power it requires.
        private bool _isPowered;            //If the object is currently powered or not.

        public GameObject(string id, string objectType, Vector2 tilePosition)
            : base()
        {
            ID = id;
            _objectType = ObjectManager.GetType(objectType);
            base.TilePosition = tilePosition; // Set the tile position of the entity
            base.Position = new Vector2(tilePosition.X * Constants.TILE_WIDTH, tilePosition.Y * Constants.TILE_WIDTH);

            OverwriteTiles();
        }

        public GameObject(string id, string objectType, Vector2 tilePosition, float powerRequirement)
            : base()
        {
            ID = id;
            _objectType = ObjectManager.GetType(objectType);
            base.TilePosition = tilePosition; // Set the tile position of the entity
            base.Position = new Vector2(tilePosition.X * Constants.TILE_WIDTH, tilePosition.Y * Constants.TILE_WIDTH);

            OverwriteTiles();

            //If the object requires power, set it's power requirement.
            if (_objectType.RequiresPower)
            {
                _powerRequirement = powerRequirement;
                _isPowered = false;
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /*
             * A power manager will dicate the current amount available in it's pool of connections. 
            if(_powerRequirement <= ???POWER_MANAGER.POWER???)
            {
                _isPowered = true;
            }
            else
            {
                _isPowered = false;
            }*/

        }

        public bool OverwriteTiles()
        {
            //TODO: Scenario for rotating the object. Most likely this will just change the dimensions when rotated before it even comes to this method.
            int x = (int)_objectType.Width;
            int y = (int)_objectType.Height;

            while (y > 0)
            {
                while (x > 0)
                {
                    GameStateManager.ENGINE.WORLD.Tiles[((int)this.TilePosition.X - 1) + x, ((int)this.TilePosition.Y - 1) + y].Type = TileType.Impassable;
                    x--;
                }
                x = (int)_objectType.Width;

                GameStateManager.ENGINE.WORLD.Tiles[((int)this.TilePosition.X - 1) + x, ((int)this.TilePosition.Y - 1) + y].Type = TileType.Impassable;

                y--;
            }
            return true;
        }
       

        public ObjectType ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }

        public float PowerRequirement
        {
            get { return _powerRequirement; }
            set { _powerRequirement = value; }
        }
    }
}
