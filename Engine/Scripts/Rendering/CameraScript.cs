using System;
using Engine.Debugging;
using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Engine.Scripts.Rendering;

public class CameraScript : Script
{
    public OrthographicCamera OrthoCamera { get; private set; }
    public Color ClearColor { get; set; } = Color.CornflowerBlue;
    
    public override void Initialize()
    {
        Runtime.onViewportAdapterResize += OnViewportAdapterResize;
        OrthoCamera = new OrthographicCamera(Runtime.ViewportAdapter);
    }

    public override void OnDestroy() => Runtime.onViewportAdapterResize -= OnViewportAdapterResize;
    

    public override void Update(GameTime gameTime)
    {
        // OrthoCamera.Position = Transform.Position;
        // KeyboardInfo keyboard = Runtime.Input.Keyboard;
        //     
        // Vector2 velocity = Vector2.Zero;
        //
        // if (keyboard.IsKeyDown(Keys.W)) velocity.Y--;
        // if (keyboard.IsKeyDown(Keys.A)) velocity.X--;
        // if (keyboard.IsKeyDown(Keys.S)) velocity.Y++;
        // if (keyboard.IsKeyDown(Keys.D)) velocity.X++;
        //     
        // if(velocity != Vector2.Zero) velocity.Normalize();
        //
        // Transform.Position += velocity * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
    
    private void OnViewportAdapterResize()
    {
        OrthoCamera = new OrthographicCamera(Runtime.ViewportAdapter);
    }
}