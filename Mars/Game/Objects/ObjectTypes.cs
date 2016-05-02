using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars
{
    public class ObjectType
    {
        private string _key;
        private string _name;
        private string _description;
        private string _sprite;

        private bool _isInternal;
        private bool _isInteractable;

        private bool _producesPower;
        private bool _requiresPower;

        private bool _producesOxygen;
        private bool _requiresOxygen;

        private int _width;
        private int _height;

        public ObjectType()
        {
        }
        public ObjectType(string key, string name, string description, string sprite, bool isInternal, bool isInteractable, bool requiresPower, bool producesPower, bool requiresOxygen, bool producesOxygen, int width, int height)
        {
            _key = key;
            _name = name;
            _description = description;
            _sprite = sprite;
            _isInternal = isInternal;
            _isInteractable = isInteractable;
            _requiresPower = requiresPower;
            _producesPower = producesPower;
            _requiresPower = requiresOxygen;
            _producesPower = producesOxygen;
            _width = width;
            _height = height;
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

        public string Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }

        public bool Internal
        {
            get { return _isInternal; }
            set { _isInternal = value; }
        }

        public bool Interactable
        {
            get { return _isInteractable; }
            set { _isInteractable = value; }
        }

        public bool RequiresPower
        {
            get { return _requiresPower; }
            set { _requiresPower = value; }
        }

        public bool ProducesPower
        {
            get { return _producesPower; }
            set { _producesPower = value; }
        }

        public bool RequiresOxygen
        {
            get { return _requiresOxygen; }
            set { _requiresOxygen = value; }
        }

        public bool ProducesOxygen
        {
            get { return _producesOxygen; }
            set { _producesOxygen = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
    }
}
