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
        private Bio _bio;
        private Needs _needs;
        private Skills _skills;
        private List<Trait> _traits;

        private LinkedList<Tile> _path;
        private bool _selected;

        public CrewMember(int ID, string name, int age, bool male, Country country, string story, Vector2? position = null) : base(position)
        {
            _ID = ID;

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
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spritebatch)
        {

        }

        #region PROPERTIES
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
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
