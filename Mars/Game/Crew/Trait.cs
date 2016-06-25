using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mars
{
    public class Trait
    {
        private string _key;
        private string _name;
        private string _description;
        private string _icon;

        public Trait()
        {
        }
        public Trait(string key, string name, string description, string icon)
        {
            _key = key;
            _name = name;
            _description = description;
            _icon = icon;
        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
    }
}
