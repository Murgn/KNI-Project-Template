using Core.Scenes;
using Engine;
using Engine.Debugging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

namespace Core.ECS.Systems
{
    public class RenderSystem : EntityDrawSystem
    {
        private ComponentMapper<Transform2> transformMapper;
        //private ComponentMapper<Sprite> spriteMapper;
        
        public RenderSystem() : base(Aspect.All(/*typeof(Sprite), */typeof(Transform2))) {}

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            //spriteMapper = mapperService.GetMapper<Sprite>();
        }

        public override void Draw(GameTime gameTime)
        {
            Debug.Log("RenderSystem ECS Draw");
            Runtime.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            {
                foreach (var entity in ActiveEntities)
                {
                    Debug.Log($"{entity}");
                    var transform = transformMapper.Get(entity);
                    //var sprite = spriteMapper.Get(entity);
                    
                    Runtime.SpriteBatch.DrawRectangle(transform.Position, transform.Scale, Color.White);
                }
            }
            Runtime.SpriteBatch.End();
        }
    }
}