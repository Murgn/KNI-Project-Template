using Core.Scripts;
using Engine;
using Engine.Debugging;
using Engine.Screens;
using Engine.Scripts.Physics;
using Engine.Scripts.Rendering;
using ImGuiNET;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Diagnostics;
using nkast.Aether.Physics2D.Dynamics;

///
/// PUSH THE WEB FIXING BRANCH INTO DEV GAME READY !!!!!!!!!!!!!!!!
///
///
///
///
///
///
///
///
///
///
///
///
///
///
///
///
///
///
///
///
/// 

namespace Core.Scenes
{
    public class GameplayScreen : GameObjectScreen
    {
        public GameplayScreen(Game game) : base(game, "MainMenuScreen") { }
        private DebugView _debugView;
        
        GameObject selectedObject = null;

        public override void Initialize()
        {
            base.Initialize();
            CameraScript.ClearColor = Color.Black;

            CreateGameObject("Player", new[] { typeof(PlayerScript), typeof(PrimitiveRenderer), typeof(PhysicsBody2D) });
            MakeFloor(new Vector2(0.0f, 7.0f), new Vector2(10.0f, 3.0f));
            MakeFloor(new Vector2(10.0f, 5.0f), new Vector2(10.0f, 1.0f));
            MakeFloor(new Vector2(-10.0f, 0.0f), new Vector2(10.0f, 2.0f));
            
            _debugView = new DebugView(Runtime.PhysicsWorld);
            _debugView.LoadContent(GraphicsDevice, Content);
            _debugView.AppendFlags(DebugViewFlags.Shape);
            _debugView.AppendFlags(DebugViewFlags.AABB);
        }

        private int floorCount;
        
        public void MakeFloor(Vector2 pos, Vector2 scale)
        {
            var floor = CreateGameObject($"Floor {floorCount}", new[] { typeof(PrimitiveRenderer), typeof(PhysicsBody2D) });
            floor.Transform.Position = pos;
            floor.Transform.Scale = scale;
            var physics = floor.GetScript<PhysicsBody2D>();
            physics.bodyType = BodyType.Kinematic;
            physics.RebuildCollider();
            floorCount++;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void DrawUI(GameTime gameTime)
        {
            base.DrawUI(gameTime);

            #if DEBUG && !BLAZORGL

            if (Editor.ShowDebug)
            {
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, -1, 1);
                Matrix view = CameraScript.OrthoCamera.GetViewMatrix();
                _debugView.RenderDebugData(ref projection, ref view);
            }
            this.DrawDebugEditor();
            #endif
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}