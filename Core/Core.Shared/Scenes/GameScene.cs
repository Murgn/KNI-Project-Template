using Engine;
using Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes
{
    public class GameScene : Scene
    {
        // Initialize Systems
        public override void Initialize()
        {
            base.Initialize();
            
            Runtime.ExitOnEscape = false;
        }

        // Load Assets
        public override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        { 

        }

        public override void Draw(GameTime gameTime)
        {
            Runtime.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            Runtime.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            {

            }
            Runtime.SpriteBatch.End();
        }
    }
}