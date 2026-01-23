using Core.ECS.Components;
using Engine;
using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Core.ECS.Systems
{
    public class PlayerSystem : EntityProcessingSystem
    {
        private ComponentMapper<PlayerComponent> playerMapper;
        
        public PlayerSystem(AspectBuilder aspectBuilder) : base(Aspect.All(typeof(PlayerComponent))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            playerMapper = mapperService.GetMapper<PlayerComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            PlayerComponent player = playerMapper.Get(entityId);
            KeyboardInfo keyboard = Runtime.Input.Keyboard;

            if (keyboard.IsKeyDown(Keys.Left))
                player.Position.X -= player.Speed;
            if (keyboard.IsKeyDown(Keys.Right))
                player.Position.X += player.Speed;
            if (keyboard.IsKeyDown(Keys.Up))
                player.Position.Y -= player.Speed;
            if (keyboard.IsKeyDown(Keys.Down))
                player.Position.Y += player.Speed;
        }
    }
}