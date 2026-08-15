using Core.Scenes;
using Microsoft.Xna.Framework;
using Engine;

namespace Core
{
    public class CoreGame : Runtime
    {
        // virtual resolution broken
        public CoreGame() : base("Project", 1280, 720, false, 64, 32) { }

        protected override void Initialize()
        {
            base.Initialize();
            ScreenManager.ShowScreen(new GameplayScreen(this));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.SetRenderTarget(RenderTexture.Target);
            
            base.Draw(gameTime);
        }
    }
}
