using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Runtime.Serialization;

namespace Mars
{
    /// <summary>
    /// Holds the current gamestate, used for savegame serialization.
    /// </summary>
    [DataContract]
    public class World
    {
        private GameUI _UI;
        private TileMap _tileMap;
        private Clock _clock;
        private List<CrewMember> _crewMembers;
        private List<GameObject> _objects;
        private List<Building> _buildings;
        private Planet _planet;

        public World(ContentManager content)
        {
            _UI = new WorldUI("World", content);
            _tileMap = new TileMap(Constants.MAP_WIDTH, Constants.MAP_HEIGHT);
            _clock = new Clock();
            _crewMembers = new List<CrewMember>();
            _objects = new List<GameObject>();
            _buildings = new List<Building>();
            _planet = new Planet() { ID = "Mars", Name = "Mars", Image = "world_mars" };
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            _clock.Update(gameTime);
            EntityCollider.Collide(gameTime);
            _tileMap.Update(gameTime);

            // Update all Crew Members
            foreach (CrewMember crewMember in _crewMembers)
            {
                crewMember.Update(gameTime);
            }

            // Update all Objects
            foreach (GameObject gameObject in _objects)
            {
                gameObject.Update(gameTime);
            }

            // Update all Buildings
            foreach (Building building in _buildings)
            {
                building.Update(gameTime);
            }

            // UI updates after all entities
            _UI.Update();

            // Update Frame Rate
            FrameRateCounter.Update(gameTime);

            // Update Debug Text
            DebugTextManager.Update(gameTime);
            DebugTextManager.AddWatcher(Camera.DebugInfo, "Camera: ");
            DebugTextManager.AddWatcher(Controls.MousePosition.Screen, "Mouse Screen: ");
            DebugTextManager.AddWatcher(Controls.MousePosition.Cartesian, "Mouse Cartesian: ");
            DebugTextManager.AddWatcher(Controls.MousePosition.Isometric, "Mouse Isometric: ");
            DebugTextManager.AddWatcher(GameStateManager.Mode, "GameMode: ");

            // Check for exit to Main Menu
            if (Controls.Keyboard.IsKeyDown(Keys.Escape))
            {
                GameStateManager.State = GameState.MainMenu;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _tileMap.Draw(spriteBatch);
            _UI.Draw(spriteBatch);
        }

        #region PROPERTIES
        public TileMap TileMap
        {
            get { return _tileMap; }
            set { _tileMap = value; }
        }

        [DataMember]
        public Clock Clock
        {
            get { return _clock; }
            set { _clock = value; }
        }

        public List<CrewMember> CrewMembers
        {
            get { return _crewMembers; }
            set { _crewMembers = value; }
        }

        public List<GameObject> Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        public List<Building> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; }
        }

        [DataMember]
        public Planet Planet
        {
            get { return _planet; }
            set { _planet = value; }
        }
        #endregion
    }
}
