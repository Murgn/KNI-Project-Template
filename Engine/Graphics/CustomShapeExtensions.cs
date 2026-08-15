using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Engine.Graphics;

public static class CustomShapeExtensions
{
    private static 
#nullable disable
        Texture2D _whitePixelTexture;
    
    private static Texture2D GetTexture(SpriteBatch spriteBatch)
    {
        if (_whitePixelTexture == null || _whitePixelTexture.IsDisposed)
        {
            _whitePixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _whitePixelTexture.SetData<Color>(new Color[1]
            {
                Color.White
            });
            spriteBatch.Disposing += (EventHandler<EventArgs>) ((sender, args) =>
            {
                _whitePixelTexture?.Dispose();
                _whitePixelTexture = (Texture2D) null;
            });
        }
        return _whitePixelTexture;
    }
    
    public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, float rotation, SizeF size, Color color, float layerDepth = 0.0f)
    {
        spriteBatch.DrawRectangle(new RectangleF(position.X - size.Width / 2f, position.Y - size.Height / 2f, size.Width, size.Height), MathHelper.ToRadians(rotation), color, layerDepth);
    }
    
    public static void DrawRectangle(this SpriteBatch spriteBatch, RectangleF rectangle, float rotation, Color color, float layerDepth = 0.0f)
    {
        Texture2D texture = GetTexture(spriteBatch);
        Vector2 origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        Vector2 position = new Vector2(rectangle.X + rectangle.Width / 2f, rectangle.Y + rectangle.Height / 2f);
        spriteBatch.Draw(texture, position, null, color, rotation, origin, rectangle.Size, SpriteEffects.None, layerDepth);
    }
    
}