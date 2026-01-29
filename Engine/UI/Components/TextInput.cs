using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;

namespace Engine.UI.Components;

public class TextInput : TextBox
{
    public TextInput(string text = "Text", string fontPath = "fonts/QuanGum.fnt")
    {
        Create(text, new BitmapFont(fontPath));
    }

    public TextInput(BitmapFont bitmapFont, string text = "Text")
    {
        Create(text, bitmapFont);
    }

    private void Create(string text, BitmapFont font)
    {
        TextBoxVisual textBoxVisual = (TextBoxVisual)Visual;
        textBoxVisual.Height = 14f;
        textBoxVisual.HeightUnits = DimensionUnitType.Absolute;
        textBoxVisual.Width = 48f;
        textBoxVisual.WidthUnits = DimensionUnitType.RelativeToChildren;

        // TextRuntime textInstance = buttonVisual.TextInstance;
        // textInstance.Text = text;
        // textInstance.BitmapFont = font;
        // textInstance.Anchor(Gum.Wireframe.Anchor.Center);
        // textInstance.Width = 0;
        // textInstance.WidthUnits = DimensionUnitType.RelativeToChildren;

        coreTextObject.BitmapFont = font;
        placeholderTextObject.BitmapFont = font;
    }
}