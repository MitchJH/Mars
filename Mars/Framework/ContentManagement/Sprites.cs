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
    public static class Sprites
    {
        private static Texture2D _MISSING_TEXTURE;
        private static Texture2D _PIXEL;

        private static Dictionary<string, Texture2D> _sprites;

        static Sprites()
        {
            _sprites = new Dictionary<string, Texture2D>();

            /* SPLITTING A SPRITESHEET
            int tile_size = 64;
            int tiles_W = originalTexture.Width / tile_size;
            int tiles_H = originalTexture.Height / tile_size;

            for (int x = 0; x < tiles_W; x++)
            {
                for (int y = 0; y < tiles_H; y++)
                {
                    Rectangle sourceRectangle = new Rectangle(x * tile_size, y * tile_size, tile_size, tile_size);
                    Texture2D cropTexture = new Texture2D(GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
                    Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                    originalTexture.GetData(0, sourceRectangle, data, 0, data.Length);
                    cropTexture.SetData(data);
                    dirt_tiles.Add(cropTexture);
                }
            }
            */
        }

        public static void LoadSprite(string[] data, ContentManager content)
        {
            string id = data[1];
            string filepath = data[2];

            Texture2D newTexture = content.Load<Texture2D>(filepath);
            _sprites.Add(id, newTexture);
        }

        public static Texture2D Get(string key)
        {
            string keyL = key.ToLower();
            if (string.IsNullOrEmpty(keyL) == false)
            {
                if (_sprites.ContainsKey(keyL))
                {
                    return _sprites[keyL];
                }
            }
            return _MISSING_TEXTURE;
        }

        public static Texture2D MISSING_TEXTURE
        {
            get { return _MISSING_TEXTURE; }
            set { _MISSING_TEXTURE = value; }
        }

        public static Texture2D PIXEL
        {
            get { return _PIXEL; }
            set { _PIXEL = value; }
        }
    }
}
