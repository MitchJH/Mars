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
    public static class Fonts
    {
        private static Dictionary<string, SpriteFont> _fonts;

        static Fonts()
        {
            _fonts = new Dictionary<string, SpriteFont>();
        }

        public static void LoadFont(string[] data, ContentManager content)
        {
            string id = data[1];
            string filepath = data[2];

            SpriteFont newFont = content.Load<SpriteFont>(filepath);
            _fonts.Add(id, newFont);
        }

        public static SpriteFont Get(string key)
        {
            string keyL = key.ToLower();
            if (string.IsNullOrEmpty(keyL) == false)
            {
                if (_fonts.ContainsKey(keyL))
                {
                    return _fonts[keyL];
                }
            }

#if DEBUG
            throw new Exception("Debug Only Exception - Missing Font [\"" + key + "\"]");
#else
            return null;
#endif
        }

        public static SpriteFont Standard
        {
            get
            {
                if (_fonts.ContainsKey("standard"))
                {
                    return _fonts["standard"];
                }
                else
                {
#if DEBUG
                    throw new Exception("Debug Only Exception - Cannot find Standard font");
#else
                    return null;
#endif
                }
            }
        }
    }
}
