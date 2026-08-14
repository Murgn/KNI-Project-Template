using Core.Scenes;
using Microsoft.Xna.Framework;
using Engine;

namespace Core
{
    public class CoreGame : Runtime
    {
        public CoreGame() : base("Chip-8", 1024, 512, false, 64, 32) { }

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
            //GraphicsDevice.SetRenderTarget(RenderTexture.Target);
            
            base.Draw(gameTime);
        }
    }
}
