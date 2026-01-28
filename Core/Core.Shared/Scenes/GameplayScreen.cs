using Core.Scripts.Player;
using Engine;
using Engine.Screens;
using Engine.Scripts.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens.Transitions;

namespace Core.Scenes
{
    public class GameplayScreen : GameObjectScreen
    {
        public GameplayScreen(Game game) : base(game, "GameplayScreen") { }
        
        // Initialize Systems
        public override void Initialize()
        {
            base.Initialize();
            
            Runtime.ExitOnEscape = false;

            var player = new GameObject("Player");
            player.Transform.Position = new Vector2(128, 72);
            player.AddScript<PlayerController>();
            player.AddScript<PrimitiveRenderer>();
            GameObjects.Add(player);
            
            var player2 = new GameObject("Player");
            player2.Transform.Position = new Vector2(0, 0);
            player2.AddScript<PlayerController>();
            player2.AddScript<PrimitiveRenderer>();
            GameObjects.Add(player2);
        }

        // Load Assets
        public override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var gameObject in GameObjects)
                gameObject.Update(gameTime);
            
            if(Runtime.Input.Keyboard.IsKeyPressed(Keys.Space))
                SaveToFile(Content, $"../../../../../CoreContent/screens/");
            
            if(Runtime.Input.Keyboard.IsKeyPressed(Keys.Enter)) 
                ScreenManager.ShowScreen(new LoadedScreen(Game, "screens/GameplayScreen.xml"), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public override void Draw(GameTime gameTime)
        {
            Runtime.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            Runtime.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            {
                    foreach (var gameObject in GameObjects)
                        gameObject.Draw(gameTime);
            }
            Runtime.SpriteBatch.End();
        }
    }
}