using Microsoft.Xna.Framework;

namespace Core.ECS.Components
{
    public class PlayerComponent
    {
        public int Speed;
        public Vector2 Position;

        public PlayerComponent(int speed, Vector2 position)
        {
            Speed = speed;
            Position = position;
        }
    }
}