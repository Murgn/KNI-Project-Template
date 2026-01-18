using Microsoft.Xna.Framework;
using Core.Scenes;
using Engine;

namespace Core
{
    public class CoreGame : Runtime
    {        
        public CoreGame() : base("Project", 1280, 720, false, 320, 180) { }

        protected override void Initialize()
        {
            base.Initialize();
            
            ChangeScene(new GameScene());
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
