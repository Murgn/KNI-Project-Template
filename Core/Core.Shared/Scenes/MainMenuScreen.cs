using Core.UI.Canvases;
using Engine;
using Engine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Core.Scenes
{
    public class MainMenuScreen : GameObjectScreen
    {
        private BitmapFont font;
        private const string text = "The quick brown fox\njumped over the lazy dog";
        private Vector2 textPos;

        private MainMenuCanvas canvas;
        
        public MainMenuScreen(Game game) : base(game, "MainMenuScreen") { }

        public override void Initialize()
        {
            base.Initialize();

            textPos.X = (int)(Runtime.RenderTexture.renderWidth / 2.0f);
            textPos.Y = (int)(Runtime.RenderTexture.renderHeight / 2.0f);

            canvas = new MainMenuCanvas(Game, this);
            canvas.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<BitmapFont>("fonts/Quan");
            
            Vector2 pos = font.MeasureString(text) / 2.0f;
            textPos.X -= (int)pos.X;
            textPos.Y -= (int)pos.Y;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            
            Runtime.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            {
                Runtime.SpriteBatch.DrawString(font, text, textPos, Color.White);
            }
            Runtime.SpriteBatch.End(); 
        }
    }
}