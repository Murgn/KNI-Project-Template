using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;
using MonoGame.Extended.BitmapFonts;
using MonoGameGum.GueDeriving;

namespace Engine.UI.Components;

public class TextButton : Button
{
    public TextButton(string text = "Text", string fontPath = "fonts/QuanGum.fnt")
    {
        ButtonVisual buttonVisual = (ButtonVisual)Visual;
        buttonVisual.Height = 14f;
        buttonVisual.HeightUnits = DimensionUnitType.Absolute;
        buttonVisual.Width = 15f;
        buttonVisual.WidthUnits = DimensionUnitType.RelativeToChildren;

        TextRuntime textInstance = buttonVisual.TextInstance;
        var bitmapFont = new RenderingLibrary.Graphics.BitmapFont(fontPath);
        textInstance.Text = text;
        textInstance.BitmapFont = bitmapFont;
        textInstance.Anchor(Gum.Wireframe.Anchor.Center);
        textInstance.Width = 0;
        textInstance.WidthUnits = DimensionUnitType.RelativeToChildren;
    }

    public TextButton(RenderingLibrary.Graphics.BitmapFont bitmapFont, string text = "Text")
    {
        ButtonVisual buttonVisual = (ButtonVisual)Visual;
        buttonVisual.Height = 14f;
        buttonVisual.HeightUnits = DimensionUnitType.Absolute;
        buttonVisual.Width = 15f;
        buttonVisual.WidthUnits = DimensionUnitType.RelativeToChildren;

        TextRuntime textInstance = buttonVisual.TextInstance;
        textInstance.Text = text;
        textInstance.BitmapFont = bitmapFont;
        textInstance.Anchor(Gum.Wireframe.Anchor.Center);
        textInstance.Width = 0;
        textInstance.WidthUnits = DimensionUnitType.RelativeToChildren;
    }
}