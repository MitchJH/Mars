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
        private int _ID;
        private ObjectType _objectType;

        private float _powerRequirement;
        private float _oxygenRequirement;
        private float _waterRequirement;

        public GameObject(int ID, string objectType, Vector2 tilePosition)
            : base()
        {
            _ID = ID;
            _objectType = ObjectManager.GetType(objectType);
            base.Position = new Vector2(tilePosition.X * Constants.TILE_WIDTH, tilePosition.Y * Constants.TILE_WIDTH);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        #region PROPERTIES
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
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
        #endregion
    }
}
