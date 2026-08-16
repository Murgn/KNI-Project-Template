using Core.Scripts;
using Engine;
using Engine.Debugging;
using Engine.Screens;
using Engine.Scripts.Physics;
using Engine.Scripts.Rendering;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tilemaps;
using MonoGame.Extended.Tilemaps.LDtk;
using MonoGame.Extended.Tilemaps.Rendering;
using nkast.Aether.Physics2D.Diagnostics;
using TilemapRenderer = Engine.Scripts.Rendering.TilemapRenderer;

namespace Core.Scenes
{
    public class GameplayScreen : GameObjectScreen
    {
        public GameplayScreen(Game game) : base(game, "MainMenuScreen") { }
        private DebugView _debugView;
        
        public override void Initialize()
        {
            base.Initialize();
            CameraScript.ClearColor = new Color(49, 5, 30);

            var player = CreateGameObject("Player", new[] { typeof(PlayerScript), typeof(PhysicsBody2D), typeof(SpriteRenderer) });
            var sprite = player.GetScript<SpriteRenderer>();
            sprite.spritePath = "sprites/Dog";
            sprite.Setup();
            sprite.sortingOrder = 1;

            player.Transform.Position = new Vector2(50, 216);
            
            var tilemap = CreateGameObject("Tilemap", new[]  { typeof(TilemapRenderer), typeof(TilemapCollider2D) });
            var renderer = tilemap.GetScript<TilemapRenderer>();
            renderer.backgroundLayers.Add("Background");
            renderer.backgroundLayers.Add("Platforms");
            tilemap.GetScript<TilemapCollider2D>().Setup();

            CameraScript.lookAt = player;
            
            _debugView = new DebugView(Runtime.PhysicsWorld);
            _debugView.LoadContent(GraphicsDevice, Content);
            _debugView.AppendFlags(DebugViewFlags.Shape);
            _debugView.AppendFlags(DebugViewFlags.AABB);
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
    }
}