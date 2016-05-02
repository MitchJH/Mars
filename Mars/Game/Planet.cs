using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;

namespace Mars
{
    [DataContract]
    public class Planet
    {
        private string _ID;
        private string _name;
        private string _image;

        public Planet()
        {
        }

        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }
    }
}
