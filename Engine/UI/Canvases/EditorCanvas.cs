using Engine.Screens;
using Engine.UI.Components;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework; 
using MonoGameGum;
using MonoGameGum.GueDeriving;

namespace Engine.UI.Canvases;

public class EditorCanvas(Game game, GameObjectScreen screen) : Canvas(game, screen)
{
    public override void Initialize()
    {
        Hierarchy();
        Inspector();
    }

    private void Hierarchy()
    {
        var coloredPanel = new ColoredRectangleRuntime();
        coloredPanel.Width = 4;
        coloredPanel.Height = 4;
        coloredPanel.WidthUnits = DimensionUnitType.RelativeToChildren;
        coloredPanel.HeightUnits = DimensionUnitType.RelativeToChildren;
        coloredPanel.Color = new Color(0, 0, 0, 150);
        coloredPanel.Anchor(Anchor.TopLeft);
        coloredPanel.AddToRoot();
        
        var stackPanel = new StackPanel();
        stackPanel.X = 2;
        coloredPanel.AddChild(stackPanel);

        var bitmapFont = new RenderingLibrary.Graphics.BitmapFont("fonts/QuanGum.fnt");
        
        var text = new TextRuntime
        {
            BitmapFont = bitmapFont,
            Text = "GameObjects"
        };
        text.Anchor(Anchor.TopLeft);
        stackPanel.AddChild(text);
        
        foreach (var gameObject in Screen.GameObjects)
        {
            var button = new TextButton(bitmapFont, gameObject.Name);
            button.Anchor(Anchor.Top);
            stackPanel.AddChild(button);
        }
    }
    
    private void Inspector()
    {
        var coloredPanel = new ColoredRectangleRuntime();
        coloredPanel.Width = 4;
        coloredPanel.Height = 4;
        coloredPanel.WidthUnits = DimensionUnitType.RelativeToChildren;
        coloredPanel.HeightUnits = DimensionUnitType.RelativeToChildren;
        coloredPanel.Color = new Color(0, 0, 0, 150);
        coloredPanel.Anchor(Anchor.TopRight);
        coloredPanel.AddToRoot();
        
        var stackPanel = new StackPanel();
        stackPanel.X = 2;
        coloredPanel.AddChild(stackPanel);

        var bitmapFont = new RenderingLibrary.Graphics.BitmapFont("fonts/QuanGum.fnt");
        
        var text = new TextRuntime
        {
            BitmapFont = bitmapFont,
            Text = "GameObjects"
        };
        text.Anchor(Anchor.TopLeft);
        stackPanel.AddChild(text);
    }
}