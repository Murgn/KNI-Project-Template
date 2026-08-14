using Engine;
using Engine.Debugging;
using Engine.Screens;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Core.Scenes
{
    public class MainMenuScreen : GameObjectScreen
    {
        public MainMenuScreen(Game game) : base(game, "MainMenuScreen") { }

        public override void Initialize()
        {
            base.Initialize();
            CameraScript.ClearColor = Color.Black;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            Runtime.GuiRenderer.BeginLayout(gameTime);
                ImGui.ShowDemoWindow();
            Runtime.GuiRenderer.EndLayout();
            
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}