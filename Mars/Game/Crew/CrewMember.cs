using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mars
{
    public class CrewMember : Entity
    {
        private int _ID;
        private string _sprite;
        private Bio _bio;
        private Needs _needs;
        private Skills _skills;
        private List<Trait> _traits;

        private LinkedList<Tile> _path;
        private bool _selected;
        private bool _hovered;

        private Vector2 _velocity;
        private Vector2 _direction;

        public CrewMember(int ID, string name, int age, bool male, Country country, string story, Vector2 position)
            : base(position, new Vector2(Constants.CREW_MEMBER_WIDTH, Constants.CREW_MEMBER_HEIGHT))
        {
            _ID = ID;
            _sprite = "crew";

            _bio.Name = name;
            _bio.Age = age;
            _bio.Male = male;
            _bio.Country = country;
            _bio.Story = story;

            _needs.Health = 100;
            _needs.Energy = 100;
            _needs.Hunger = 100;
            _needs.Thirst = 100;
            _needs.Stress = 100;

            _skills.Fitness = 0;
            _skills.Engineering = 0;
            _skills.Agriculture = 0;
            _skills.Medicine = 0;

            _traits = new List<Trait>();

            _path = new LinkedList<Tile>();
            _selected = false;
            _hovered = false;
        }

        public override void Update(GameTime gameTime)
        {
            _hovered = this.Bounds.Contains(Controls.MouseWorldPosition.ToPoint());

            this.Move(gameTime);

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (_selected)
            {
                if (_path.Count > 0)
                {
                    DrawPath(spritebatch);
                }

                spritebatch.Draw(Sprites.Get(_sprite), this.Bounds, Color.Green);
            }
            else
            {
                if (_hovered)
                {
                    spritebatch.Draw(Sprites.Get(_sprite), this.Bounds, Color.Yellow);
                }
                else
                {
                    spritebatch.Draw(Sprites.Get(_sprite), this.Bounds, Color.White);
                }
            }
        }

        private void DrawPath(SpriteBatch spritebatch)
        {
            for (int i = 0; i < _path.Count; i++)
            {
                Tile pathTile = _path.ElementAt(i);

                int halfTileWidth = Constants.TILE_WIDTH / 2;
                int halfTileHeight = Constants.TILE_HEIGHT / 2;

                int tileX = pathTile.Bounds.Center.X - (halfTileWidth / 2);
                int tileY = pathTile.Bounds.Center.Y - (halfTileHeight / 2);

                if (i + 1 < _path.Count)
                {
                    spritebatch.DrawLine(_path.ElementAt(i).Bounds.Center.ToVector2(), _path.ElementAt(i + 1).Bounds.Center.ToVector2(), Color.Green);
                }

                if (i == 0)
                {
                    spritebatch.DrawLine(this.Center, _path.ElementAt(0).Bounds.Center.ToVector2(), Color.Green);
                    spritebatch.FillRectangle(new Vector2(tileX, tileY), new Vector2(halfTileWidth, halfTileHeight), Color.Red);
                }
                else if (i == _path.Count - 1)
                {
                    spritebatch.FillRectangle(new Vector2(tileX, tileY), new Vector2(halfTileWidth, halfTileHeight), Color.Green);
                }
                else
                {
                    spritebatch.DrawCircle(_path.ElementAt(i).Bounds.Center.ToVector2(), 3, 36, Color.Green, 5);
                }
            }
        }

        private void Move(GameTime gameTime)
        {
            if (_path.Count > 0)
            {
                Tile nextTile = _path.First.Value;
                _direction = Vector2.Normalize(nextTile.Center - this.Center);

                _direction.Normalize();

                this.Position += _direction * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if (Vector2.Distance(this.Center, nextTile.Center) < Constants.WAYPOINT_RADIUS)
                {
                    _path.RemoveFirst();
                }
            }
        }

        #region PROPERTIES
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }

        public Bio Bio
        {
            get { return _bio; }
            set { _bio = value; }
        }

        public Needs Needs
        {
            get { return _needs; }
            set { _needs = value; }
        }

        public Skills Skills
        {
            get { return _skills; }
            set { _skills = value; }
        }

        public List<Trait> Traits
        {
            get { return _traits; }
            set { _traits = value; }
        }

        public LinkedList<Tile> Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public bool Hovered
        {
            get { return _hovered; }
            set { _hovered = value; }
        }
        #endregion
    }

    public struct Needs
    {
        public double Health;
        public double Energy;
        public double Hunger;
        public double Thirst;
        public double Stress;
    }

    public struct Skills
    {
        public double Fitness;
        public double Engineering;
        public double Agriculture;
        public double Medicine;
    }

    public struct Bio
    {
        public string Name;
        /// <summary>The age of the character.</summary>
        public int Age;
        /// <summary>True if this character is Male, false if Female.</summary>
        public bool Male;
        /// <summary>The characters country of origin.</summary>
        public Country Country;
        /// <summary>Paragraph of this characters history before coming to Mars.</summary>
        public string Story;
    }

    public enum CrewState
    {
        Idle,
        Walking,
        Running,
        Sleeping,
        Constructing
    }
}
