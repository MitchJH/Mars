using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Mars
{
    public static class ObjectManager
    {
        // This holds all types of objects
        private static Dictionary<string, ObjectType> _objectTypes;

        static ObjectManager()
        {
            _objectTypes = new Dictionary<string, ObjectType>();
        }

        /// <summary>
        /// Returns an object type based on its key
        /// </summary>
        /// <param name="key">The key of the object type</param>
        /// <returns>An object type</returns>
        public static ObjectType GetType(string key)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                if (_objectTypes.ContainsKey(key))
                {
                    return _objectTypes[key];
                }
            }
            return null;
        }

        public static void LoadObjects(string file, ContentManager content)
        {
            if (File.Exists(file) == false) return;

            using (var reader = new StreamReader(TitleContainer.OpenStream(file)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#") == false && string.IsNullOrEmpty(line) == false)
                    {
                        string[] split = line.Split(',');
                        string id = split[0];
                        string name = split[1];
                        string description = split[2];
                        string sprite = split[3];
                        bool isInternal = bool.Parse(split[4]);
                        bool isInteractable = bool.Parse(split[5]);
                        bool producesPower = bool.Parse(split[6]);
                        bool requiresPower = bool.Parse(split[7]);
                        bool producesOxygen = bool.Parse(split[8]);
                        bool requiresOxygen = bool.Parse(split[9]);
                        bool producesWater = bool.Parse(split[10]);
                        bool requiresWater = bool.Parse(split[11]);
                        int width = int.Parse(split[12]);
                        int height = int.Parse(split[13]);

                        ObjectType newObject = new ObjectType(id, name, description, sprite, isInternal, isInteractable, producesPower, requiresPower, 
                            producesOxygen, requiresOxygen, producesWater, requiresWater, width, height);
                        _objectTypes.Add(id, newObject);
                    }
                }
            }
        }

        public static Dictionary<string, ObjectType> ObjectTypes
        {
            get { return ObjectManager._objectTypes; }
            set { ObjectManager._objectTypes = value; }
        }
    }
}
