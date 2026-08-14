using Core.Scripts;
using Engine;
using Engine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Core.Scenes
{
    public class GameplayScreen : GameObjectScreen
    {
        private readonly string romToLoad;
        private readonly byte[] romBytes;
        
        public GameplayScreen(Game game, string romToLoad) : base(game, "GameplayScreen") { this.romToLoad = romToLoad; }        
        public GameplayScreen(Game game, byte[] romBytes) : base(game, "GameplayScreen") { this.romBytes = romBytes; }        
        
        public override void Initialize()
        {
            base.Initialize();
            CameraScript.ClearColor = Color.Black;
            
            Runtime.ExitOnEscape = false;
            
            var go = CreateGameObject("Chip8", new[] { typeof(ChipScript) });
            var script = go.GetScript<ChipScript>();
            
            if(!string.IsNullOrEmpty(romToLoad)) script.Setup(romToLoad);
            else script.Setup(romBytes);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var gameObject in GameObjects)
                gameObject.Update(gameTime);
            
            var keyboard = Runtime.Input.Keyboard;
            
            if(keyboard.IsKeyDown(Keys.Escape) || keyboard.IsKeyDown(Keys.Back))
                ScreenManager.ShowScreen(new MainMenuScreen(Game));
            
            if(keyboard.IsKeyDown(Keys.Space))
                ScreenManager.ShowScreen(!string.IsNullOrEmpty(romToLoad) ? new GameplayScreen(Game, romToLoad) : new GameplayScreen(Game, romBytes));
        }
    }
}