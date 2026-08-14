using Engine.Debugging;
using Engine.Screens;
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

        public override void Update(GameTime gameTime)
        {
        }
    }
}