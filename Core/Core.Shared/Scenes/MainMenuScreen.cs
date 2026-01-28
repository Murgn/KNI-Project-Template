using System;
using System.Diagnostics;
using Engine;
using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGameGum;
using MonoGameGum.GueDeriving;

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

            // var playButton = new Button();
            // playButton.AddToRoot();
            // playButton.Text = "Play";
            // playButton.Width = 32;
            // playButton.Height = 18;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<BitmapFont>("fonts/Quan");
            
            Vector2 pos = font.MeasureString(text) / 2.0f;
            textPos.X -= (int)pos.X;
            textPos.Y -= (int)pos.Y;
            
            var customText = new TextRuntime();
            var bitmapFont = new RenderingLibrary.Graphics.BitmapFont("fonts/Quan.fnt");
            customText.BitmapFont = bitmapFont;
            customText.Text = text;
            customText.X = textPos.X;
            customText.Y = textPos.Y + 32;
            customText.AddToRoot();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInfo keyboard = Runtime.Input.Keyboard;
            
            if(keyboard.IsKeyPressed(Keys.Space)) 
                ScreenManager.ShowScreen(new GameplayScreen(Game), new FadeTransition(GraphicsDevice, Color.Black));
        }

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