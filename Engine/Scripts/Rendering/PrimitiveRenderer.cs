using System;
using Engine.Debugging;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Engine.Graphics;

namespace Engine.Scripts.Rendering;

public class PrimitiveRenderer : Script
{
    public Primitive2D Primitive { get; set; }
    public Color Color { get; set; } = Color.White;
    
    public override void Draw(GameTime gameTime)
    {
        var spriteBatch = Runtime.SpriteBatch;
        var transform = GameObject.Transform;

        switch (Primitive)
        {
            case Primitive2D.Rectangle:
                spriteBatch.DrawRectangle(GameObject.Transform.Position, GameObject.Transform.Rotation, GameObject.Transform.Scale, Color);
                break;
            case Primitive2D.Circle:
                spriteBatch.DrawCircle(GameObject.Transform.Position, GameObject.Transform.Scale.Length(), 8, Color);
                break;
        }
    }
}

public enum Primitive2D
{
    Rectangle,
    Circle,
}