using Core.ECS.Systems;
using Engine;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Screens;

namespace Core.Scenes
{
    public class ECSScreen : GameScreen
    {
        public ECSScreen(Game game) : base(game)
        {
            // try move this to a onscenechanged event
            Runtime.World?.Dispose();
            Runtime.World = new WorldBuilder()
                .AddSystem(new RenderSystem())
                .Build();
        }
        
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
            Runtime.World?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Runtime.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            Runtime.World?.Draw(gameTime);
        }
    }
}