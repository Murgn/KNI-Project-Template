using Engine;
using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Core.Scripts.Player
{
    public class PlayerController : Script
    {
        public float speed = 100;
        
        public override void Update(GameTime gameTime)
        {
            KeyboardInfo keyboard = Runtime.Input.Keyboard;
            
            Vector2 velocity = Vector2.Zero;

            if (keyboard.IsKeyDown(Keys.W)) velocity.Y--;
            if (keyboard.IsKeyDown(Keys.A)) velocity.X--;
            if (keyboard.IsKeyDown(Keys.S)) velocity.Y++;
            if (keyboard.IsKeyDown(Keys.D)) velocity.X++;
            
            if(velocity != Vector2.Zero) velocity.Normalize();

            Transform.Position += velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}