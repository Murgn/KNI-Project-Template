using Engine;
using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace Core.Scenes
{
    public class MainMenuScreen : GameScreen
    {
        private BitmapFont font;
        private const string text = "The quick brown fox\njumped over the lazy dog";
        private Vector2 textPos;
        
        public MainMenuScreen(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            textPos.X = (int)(Runtime.RenderTexture.renderWidth / 2.0f);
            textPos.Y = (int)(Runtime.RenderTexture.renderHeight / 2.0f);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<BitmapFont>("fonts/Quan");
            Vector2 pos = font.MeasureString(text) / 2.0f;
            textPos.X -= (int)pos.X;
            textPos.Y -= (int)pos.Y;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInfo keyboard = Runtime.Input.Keyboard;
            if(keyboard.IsKeyPressed(Keys.Space)) 
                ScreenManager.ShowScreen(new ECSScreen(Game), new FadeTransition(GraphicsDevice, Color.Black));
            
            Runtime.World?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            
            Runtime.World?.Draw(gameTime);
            
            Runtime.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            {
                Runtime.SpriteBatch.DrawString(font, text, textPos, Color.White);
            }
            Runtime.SpriteBatch.End();        
            
        }
    }
}