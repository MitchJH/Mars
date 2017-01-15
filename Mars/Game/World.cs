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
    /// <summary>Holds the current gamestate, used for savegame serialization.</summary>
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
            _UI = new GameUI("World");
            _tileMap = new TileMap(Constants.MAP_WIDTH, Constants.MAP_HEIGHT);
            _clock = new Clock();
            _crewMembers = new List<CrewMember>();
            _objects = new List<GameObject>();
            _buildings = new List<Building>();
            _planet = new Planet() { ID = "Mars", Name = "Mars", Image = "world_mars" };

#if DEBUG
            Country wales = new Country("wales", "Wales", "Welsh", "wales_flag");
            Country ireland = new Country("ireland", "Ireland", "Irish", "ireland_flag");

            CrewMember mitch = new CrewMember(0, "Mitch", 24, true, wales, "", new Vector2(50, 50));
            CrewMember darren = new CrewMember(1, "Darren", 23, true, ireland, "", new Vector2(300, 300));

            _crewMembers.Add(mitch);
            _crewMembers.Add(darren);
#endif
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            _clock.Update(gameTime);
            _tileMap.Update(gameTime);

            // Update all Crew Members
            foreach (CrewMember crewMember in _crewMembers)
            {
                crewMember.Update(gameTime);

                if (crewMember.Hovered && Controls.LeftClick)
                {
                    DeselectAllCrew(crewMember);
                    crewMember.Selected = !crewMember.Selected;
                }
                else if (crewMember.Selected && Controls.RightClick && Tweaker.Exists("right_click_to_move"))
                {
                    var tempPath = Pathfinding.FindPath(crewMember.TilePosition, Controls.MouseWorldTilePosition);

                    if (tempPath != null)
                    {
                        crewMember.Path = tempPath;
                        Audio.PlaySoundEffect("high_beep");
                    }
                    else
                    {
                        crewMember.Path = new LinkedList<Tile>();
                        Audio.PlaySoundEffect("low_double_beep");
                    }
                }
            }

            if (HoveredCrewMember == null && Controls.LeftClick)
            {
                DeselectAllCrew();
            }

            if (SelectedCrewMember == null && Controls.Mouse.RightButton == ButtonState.Pressed)
            {
                _tileMap.HoveredTile.Type = TileType.Impassable;
            }
            else if (SelectedCrewMember == null && Controls.Mouse.LeftButton == ButtonState.Pressed)
            {
                _tileMap.HoveredTile.Type = TileType.Passable;
            }

            if (SelectedCrewMember != null && Controls.Mouse.RightButton == ButtonState.Pressed)
            {
                LinkedList<Tile> path = Pathfinding.FindPath(SelectedCrewMember.TilePosition, _tileMap.HoveredTile.Position);

                if (path != null)
                {
                    SelectedCrewMember.Path = path;
                }
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
            DebugTextManager.AddWatcher(Controls.MouseWorldPosition, "Mouse World Position: ");
            DebugTextManager.AddWatcher(Controls.MouseScreenPosition, "Mouse Screen Position: ");
            DebugTextManager.AddWatcher(GameStateManager.Mode, "GameMode: ");

            // Check for exit to Main Menu
            if (Controls.Keyboard.IsKeyDown(Keys.Escape))
            {
                GameStateManager.State = GameState.MainMenu;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);

            // Draw the tile map
            _tileMap.Draw(spriteBatch);

            // Draw all Crew Members
            foreach (CrewMember crewMember in _crewMembers)
            {
                crewMember.Draw(spriteBatch);
            }

            spriteBatch.End();

            // Draw the UI
            _UI.Draw(spriteBatch);
        }

        private void DeselectAllCrew(CrewMember exclude = null)
        {
            foreach (CrewMember crew in _crewMembers)
            {
                if (crew != exclude)
                {
                    crew.Selected = false;
                }
            }
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

        public CrewMember SelectedCrewMember
        {
            get
            {
                foreach (CrewMember crew in _crewMembers)
                {
                    if (crew.Selected)
                    {
                        return crew;
                    }
                }
                return null;
            }
        }

        public CrewMember HoveredCrewMember
        {
            get
            {
                foreach (CrewMember crew in _crewMembers)
                {
                    if (crew.Hovered)
                    {
                        return crew;
                    }
                }
                return null;
            }
        }
    }
}
