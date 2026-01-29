using System;
using Core.Scenes;
using Engine;
using Engine.Screens;
using Engine.UI.Canvases;
using Engine.UI.Components;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGameGum;
using MonoGameGum.GueDeriving;

namespace Core.UI.Canvases
{
    public class MainMenuCanvas(Game game, GameObjectScreen screen) : Canvas(game, screen)
    {
        private const string text = "The quick brown fox\njumped over the lazy dog";
        
        public override void Initialize()
        {
            var panel = new Panel();
            panel.X = 32;
            panel.Y = 32;
            panel.Width = 16;
            panel.Height = 16;
            panel.WidthUnits = DimensionUnitType.RelativeToChildren;
            panel.HeightUnits = DimensionUnitType.RelativeToChildren;
            panel.AddToRoot();
            
            var playButton = new TextButton("Play");
            playButton.Click += PlayButtonOnClick;
            panel.AddChild(playButton);
            
            var editorButton = new TextButton("Editor");
            editorButton.X = 34;
            editorButton.Click += EditorButtonOnClick;
            panel.AddChild(editorButton);
            
            var customText = new TextRuntime();
            var bitmapFont = new RenderingLibrary.Graphics.BitmapFont("fonts/QuanGum.fnt");
            customText.BitmapFont = bitmapFont;
            customText.Text = text;
            customText.X = 128;
            customText.Y = 128;
            panel.AddChild(customText);
        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            Screen.ScreenManager.ShowScreen(new GameplayScreen(Game));
        }
        
        private void EditorButtonOnClick(object sender, EventArgs e)
        {
            Screen.ScreenManager.ShowScreen(new EditorScreen(Game, "screens/GameplayScreen"));
        }
    }
}