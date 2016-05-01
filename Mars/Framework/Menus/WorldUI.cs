using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Mars
{
    public class WorldUI
    {
        List<GUIControl> _controls = new List<GUIControl>();

        public WorldUI(ContentManager content)
        {
            Texture2D buttonBuildTexture = content.Load<Texture2D>("Textures/UI/Buttons/build");
            Texture2D buttonPipeTexture = content.Load<Texture2D>("Textures/UI/Buttons/pipe");
            Texture2D buttonWireTexture = content.Load<Texture2D>("Textures/UI/Buttons/wire");
            Point buttonSize = new Point(40, 40);
            SpriteFont buttonFont = Fonts.Get("Menu");

            Button buttonBuild = new Button("build", "",
                new Rectangle(1, 1, buttonSize.X, buttonSize.Y),
                buttonBuildTexture, buttonFont, Color.Black);
            buttonBuild.onClick += new EHandler(ButtonBuild_Click);
            buttonBuild.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            _controls.Add(buttonBuild);

            Button buttonPipe = new Button("pipe", "",
                new Rectangle(42, 1, buttonSize.X, buttonSize.Y),
                buttonPipeTexture, buttonFont, Color.Black);
            buttonPipe.onClick += new EHandler(ButtonPipe_Click);
            buttonPipe.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            _controls.Add(buttonPipe);

            Button buttonWire = new Button("wire", "",
                new Rectangle(83, 1, buttonSize.X, buttonSize.Y),
                buttonWireTexture, buttonFont, Color.Black);
            buttonWire.onClick += new EHandler(ButtonWire_Click);
            buttonWire.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            _controls.Add(buttonWire);
        }

        public void Update()
        {
            foreach (GUIControl control in _controls)
            {
                control.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (GUIControl control in _controls)
            {
                control.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        private void ButtonBuild_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("high_double_beep");
        }

        private void ButtonPipe_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("high_double_beep");
        }

        private void ButtonWire_Click(GUIControl sender)
        {
            Audio.PlaySoundEffect("high_double_beep");
        }
    }
}
