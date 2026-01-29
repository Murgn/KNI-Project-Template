using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;

namespace Engine.UI.Components;

public class TextButton : Button
{
    public TextButton(string text = "Text", string fontPath = "fonts/QuanGum.fnt")
    {
        Create(text, new BitmapFont(fontPath));
    }

    public TextButton(BitmapFont bitmapFont, string text = "Text")
    {
        Create(text, bitmapFont);
    }

    private void Create(string text, BitmapFont font)
    {
        ButtonVisual buttonVisual = (ButtonVisual)Visual;
        buttonVisual.Height = 14f;
        buttonVisual.HeightUnits = DimensionUnitType.Absolute;
        buttonVisual.Width = 15f;
        buttonVisual.WidthUnits = DimensionUnitType.RelativeToChildren;

        TextRuntime textInstance = buttonVisual.TextInstance;
        textInstance.Text = text;
        textInstance.BitmapFont = font;
        textInstance.Anchor(Gum.Wireframe.Anchor.Center);
        textInstance.Width = 0;
        textInstance.WidthUnits = DimensionUnitType.RelativeToChildren;
    }
}