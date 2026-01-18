using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace Engine.Graphics;

public class AnimatedSprite : Sprite
{
    private int currentFrame;
    private TimeSpan elapsed;
    private Animation animation;

    public Animation Animation
    {
        get => animation;
        set
        {
            animation = value;
            Region = animation.Frames[0];
        }
    }
    
    public AnimatedSprite() {}

    public AnimatedSprite(Animation animation)
    {
        Animation = animation;
    }

    public void Update(GameTime gameTime)
    {
        elapsed += gameTime.ElapsedGameTime;

        if (elapsed >= animation.Delay)
        {
            elapsed -= animation.Delay;
            currentFrame++;

            if (currentFrame >= animation.Frames.Count)
                currentFrame = 0;

            Region = animation.Frames[currentFrame];
        }
    }
}