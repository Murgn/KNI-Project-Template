using Engine.Debugging;
using Engine.Screens;
using Microsoft.Xna.Framework;
using MonoGameGum;

namespace Core.Scenes
{
    public class MainMenuScreen : GameObjectScreen
    {
        
        public MainMenuScreen(Game game) : base(game, "MainMenuScreen") { }

        public override void Initialize()
        {
            base.Initialize();
            CameraScript.ClearColor = Color.Black;
            
            // canvas = new MainMenuCanvas(Game, this);
            // canvas.Initialize();

            var ui = new MainMenuUIRuntime();
            ui.game = Game;
            ui.AddToRoot();
            
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}