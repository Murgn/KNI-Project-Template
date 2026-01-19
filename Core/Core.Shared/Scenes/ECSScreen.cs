using Core.ECS.Systems;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
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

            var entity = Runtime.World.CreateEntity();
            entity.Attach(new Transform2(new Vector2(64, 64), 0.0f, Vector2.One * 10));
            // entity.Attach(new Sprite( ));
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

    public class Enemy
    {
        public float Speed = 100.0f;
        public float TimeLeft = 1.0f;
    }
}