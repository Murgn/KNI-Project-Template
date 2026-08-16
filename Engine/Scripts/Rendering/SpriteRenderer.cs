using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace Engine.Scripts.Rendering;

public class SpriteRenderer : Script
{
    public string spritePath;
    public int spriteWidth = 16, spriteHeight = 16;
    
    public float sortingOrder
    {
        get { if(sprite != null) return sprite.Depth; return 0; }
        set { if(sprite != null) sprite.Depth = value; }
    }

    private Texture2D texture;
    private Texture2DAtlas atlas;
    private Sprite sprite;
    
    // need to sort out initialization
    public void Setup()
    {
        texture = Runtime.Content.Load<Texture2D>(spritePath);
        
        string name = spritePath[(spritePath.LastIndexOfAny(['/', '\\']) + 1)..];
        atlas = Texture2DAtlas.Create($"Atlas/{name}", texture, spriteWidth, spriteHeight);
        sprite = atlas.CreateSprite(0);
        sprite.Origin = sprite.Size.ToVector2() / 2;
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        
        Runtime.SpriteBatch.Draw(sprite, Transform);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        texture.Dispose();
    }
}