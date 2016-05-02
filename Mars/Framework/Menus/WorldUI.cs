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
    public class WorldUI : GameUI
    {
        public WorldUI(string ID, ContentManager content)
            : base(ID)
        {
            Texture2D buttonWorldTexture = content.Load<Texture2D>("Textures/UI/Buttons/world");
            Texture2D buttonBuildTexture = content.Load<Texture2D>("Textures/UI/Buttons/build");
            Texture2D buttonPipeTexture = content.Load<Texture2D>("Textures/UI/Buttons/pipe");
            Texture2D buttonWireTexture = content.Load<Texture2D>("Textures/UI/Buttons/wire");
            Point buttonSize = new Point(40, 40);
            SpriteFont buttonFont = Fonts.Get("Menu");

            Button buttonWorld = new Button("world", "",
                new Rectangle(1, 1, buttonSize.X, buttonSize.Y),
                buttonWorldTexture, buttonFont, Color.Black);
            buttonWorld.onClick += new EHandler(ButtonWorld_Click);
            buttonWorld.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            this.Controls.Add(buttonWorld);

            Button buttonBuild = new Button("build", "",
                new Rectangle(42, 1, buttonSize.X, buttonSize.Y),
                buttonBuildTexture, buttonFont, Color.Black);
            buttonBuild.onClick += new EHandler(ButtonBuild_Click);
            buttonBuild.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            this.Controls.Add(buttonBuild);

            Button buttonPipe = new Button("pipe", "",
                new Rectangle(83, 1, buttonSize.X, buttonSize.Y),
                buttonPipeTexture, buttonFont, Color.Black);
            buttonPipe.onClick += new EHandler(ButtonPipe_Click);
            buttonPipe.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            this.Controls.Add(buttonPipe);

            Button buttonWire = new Button("wire", "",
                new Rectangle(124, 1, buttonSize.X, buttonSize.Y),
                buttonWireTexture, buttonFont, Color.Black);
            buttonWire.onClick += new EHandler(ButtonWire_Click);
            buttonWire.onMouseEnter += delegate { Audio.PlaySoundEffect("high_beep"); };
            this.Controls.Add(buttonWire);
        }

        private void ButtonWorld_Click(GUIControl sender)
        {
            GameStateManager.Mode = GameMode.World;
            Audio.PlaySoundEffect("high_double_beep");
        }

        private void ButtonBuild_Click(GUIControl sender)
        {
            GameStateManager.Mode = GameMode.Build;
            Audio.PlaySoundEffect("high_double_beep");
        }

        private void ButtonPipe_Click(GUIControl sender)
        {
            GameStateManager.Mode = GameMode.Pipe;
            Audio.PlaySoundEffect("high_double_beep");
        }

        private void ButtonWire_Click(GUIControl sender)
        {
            GameStateManager.Mode = GameMode.Wire;
            Audio.PlaySoundEffect("high_double_beep");
        }
    }
}
