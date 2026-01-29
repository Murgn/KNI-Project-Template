using System;
using Microsoft.Xna.Framework;
using Core.Scenes;
using Engine;
using Engine.Screens;

namespace Core
{
    public class CoreGame : Runtime
    {
        public CoreGame() : base("Project", 1280, 720, false, 640 / 2, 360 / 2)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            ScreenManager.ShowScreen(new MainMenuScreen(this));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(RenderTexture.Target);
            
            base.Draw(gameTime);
        }
    }
}
