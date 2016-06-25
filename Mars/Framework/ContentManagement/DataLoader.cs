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
    public static class DataLoader
    {
        private static string dataFileType = "*.txt";

        static DataLoader()
        {
        }

        /// <summary>Load all data within the provided directory.</summary>
        /// <param name="path">The directory from which to load data.</param>
        public static void Load(string directory, ContentManager content)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(content.RootDirectory, directory));

            if (dir.Exists)
            {
                IEnumerable<FileInfo> files = dir.EnumerateFiles(dataFileType, SearchOption.AllDirectories);

                foreach (FileInfo file in files)
                {
                    string key = Path.GetFileNameWithoutExtension(file.Name);
                    using (var reader = file.OpenText())
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.ToLower();
                            if (line.StartsWith("#") == false)
                            {
                                ParseLine(line, content);
                            }
                        }
                    }
                }
            }
        }

        private static void ParseLine(string line, ContentManager content)
        {
            string[] data = line.Split(',');

            switch (data[0])
            {
                case "font":
                    Fonts.LoadFont(data, content);
                    break;
                case "sound":
                    Audio.LoadSound(data, content);
                    break;
                case "music":
                    Audio.LoadMusic(data, content);
                    break;
                case "sprite":
                    Sprites.LoadSprite(data, content);
                    break;
                case "object":
                    break;
                case "building":
                    break;
            }
        }
    }
}
