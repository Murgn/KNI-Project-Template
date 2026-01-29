using System;
using System.Linq;
using Engine.Debugging;
using Engine.Screens;
using Engine.UI.Components;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework; 
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;

namespace Engine.UI.Canvases;

public class EditorCanvas(Game game, GameObjectScreen screen) : Canvas(game, screen)
{
    private ColoredRectangleRuntime inspector;
    private BitmapFont font;
    
    public override void Initialize()
    {
        font = new RenderingLibrary.Graphics.BitmapFont("fonts/QuanGum.fnt");
        Hierarchy();
        
        var button = new TextButton(font, "Save");
        button.Anchor(Anchor.BottomLeft);
        button.AddToRoot();
        // button.Click += (((sender, args) => Screen.SaveToFile(Screen.Content, ));

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
        stackPanel.Y = 2;
        stackPanel.Spacing = 2;
        coloredPanel.AddChild(stackPanel);

        
        var text = new TextRuntime
        {
            BitmapFont = font,
            Text = "Hierarchy",
        };
        text.Anchor(Anchor.TopLeft);
        stackPanel.AddChild(text);
        
        foreach (var gameObject in Screen.GameObjects)
        {
            var button = new TextButton(font, gameObject.Name);
            button.Anchor(Anchor.Top);
            stackPanel.AddChild(button);
            var selectedGameObject = gameObject;
            button.Click += (((sender, args) => CreateInspector(sender, args, selectedGameObject)));
        }
    }

    private void CreateInspector(object sender, EventArgs e, GameObject gameObject)
    {
        inspector?.RemoveFromRoot();
        inspector = new ColoredRectangleRuntime();
        inspector.Width = 4;
        inspector.Height = 4;
        inspector.WidthUnits = DimensionUnitType.RelativeToChildren;
        inspector.HeightUnits = DimensionUnitType.RelativeToChildren;
        inspector.Color = new Color(0, 0, 0, 150);
        inspector.Anchor(Anchor.TopRight);
        inspector.AddToRoot();

        var stackPanel = new StackPanel();
        stackPanel.X = 2;
        stackPanel.Y = 2;
        stackPanel.Spacing = 2;
        inspector.AddChild(stackPanel);

        var bitmapFont = new RenderingLibrary.Graphics.BitmapFont("fonts/QuanGum.fnt");

        var text = new TextRuntime
        {
            BitmapFont = bitmapFont,
            Text = "Inspector"
        };
        text.Anchor(Anchor.TopLeft);
        stackPanel.AddChild(text);

        var positionStack = new StackPanel();
        positionStack.Orientation = Orientation.Horizontal;
        stackPanel.AddChild(positionStack);

        var x = new TextRuntime
        {
            BitmapFont = bitmapFont,
            Text = " X: "
        };
        x.Anchor(Anchor.Left);
        positionStack.AddChild(x);

        var posX = new TextInput(bitmapFont);
        posX.Text = gameObject.Transform.Position.X.ToString();
        posX.PreviewTextInput += (sender, args) =>
        {
            if (args.Text.Any(item => !char.IsDigit(item)))
                args.Handled = true;
        };
        posX.TextChanged += (o, args) => gameObject.Transform.Position = new Vector2(float.Parse(posX.Text), gameObject.Transform.Position.Y);
        posX.Placeholder = "X";
        positionStack.AddChild(posX);
        
        var y = new TextRuntime
        {
            BitmapFont = bitmapFont,
            Text = " Y: "
        };
        y.Anchor(Anchor.Left);
        positionStack.AddChild(y);
        
        var posY = new TextInput(bitmapFont);
        posY.Text = gameObject.Transform.Position.Y.ToString();
        posY.PreviewTextInput += (sender, args) =>
        {
            if (args.Text.Any(item => !char.IsDigit(item)))
                args.Handled = true;
        };
        posX.TextChanged += (o, args) => gameObject.Transform.Position = new Vector2(gameObject.Transform.Position.X, float.Parse(posY.Text));
        posY.Placeholder = "Y";
        

        positionStack.AddChild(posY);

    }
}