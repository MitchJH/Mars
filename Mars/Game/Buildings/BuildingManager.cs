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
    public static class BuildingManager
    {
        private static Dictionary<string, BuildingType> _buildingTypes;

        static BuildingManager()
        {
            _buildingTypes = new Dictionary<string, BuildingType>();
        }

        /// <summary>
        /// This will return an building type based on its key
        /// </summary>
        /// <param name="key">The key of the building type</param>
        /// <returns>An building type</returns>
        public static BuildingType GetType(string key)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                if (_buildingTypes.ContainsKey(key))
                {
                    return _buildingTypes[key];
                }
            }
            return null;
        }

        public static void LoadBuildings(string file, ContentManager content)
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
                        int width = int.Parse(split[4]);
                        int height = int.Parse(split[5]);

                        BuildingType newBuilding = new BuildingType(id, name, description, sprite, width, height);
                        _buildingTypes.Add(id, newBuilding);
                    }
                }
            }
        }

        public static Dictionary<string, BuildingType> BuildingTypes
        {
            get { return _buildingTypes; }
            set { _buildingTypes = value; }
        }
    }
}
