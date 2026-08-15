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
    
    public int speed = 50;
    public bool canMove = false;
    public Color ClearColor { get; set; } = Color.CornflowerBlue;
    
    public override void Initialize()
    {
        Runtime.onViewportAdapterResize += OnViewportAdapterResize;
        OnViewportAdapterResize();
    }

    public override void OnDestroy() => Runtime.onViewportAdapterResize -= OnViewportAdapterResize;

    public override void Update(GameTime gameTime)
    {
        OrthoCamera.Rotation = Transform.Rotation;
        OrthoCamera.Zoom = MathF.Max(Transform.Scale.X, Transform.Scale.Y);
        OrthoCamera.LookAt(Transform.Position);

        if (!canMove) return;
        
        KeyboardInfo keyboard = Runtime.Input.Keyboard;
        Vector2 velocity = Vector2.Zero;
        
        if (keyboard.IsKeyDown(Keys.Up)) velocity.Y--;
        if (keyboard.IsKeyDown(Keys.Left)) velocity.X--;
        if (keyboard.IsKeyDown(Keys.Down)) velocity.Y++;
        if (keyboard.IsKeyDown(Keys.Right)) velocity.X++;
            
        if(velocity != Vector2.Zero) velocity.Normalize();
        
        Transform.Position += velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
    
    private void OnViewportAdapterResize()
    {
        OrthoCamera = new OrthographicCamera(Runtime.ViewportAdapter)
        {
            Origin = new Vector2(Runtime.ViewportAdapter.VirtualWidth / 2f, Runtime.ViewportAdapter.VirtualHeight / 2f)
        };
    }
}