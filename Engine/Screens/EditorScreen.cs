using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Screens;

public class EditorScreen : GameObjectScreen
{
    private string path;
        
    public EditorScreen(Game game, string path) : base(game, "LoadedScreen") { this.path = path; }

    public override void Initialize()
    {
        base.Initialize();
            
        Runtime.ExitOnEscape = false;
            
        LoadFromFile(Content, path);
    }
        
    public override void Update(GameTime gameTime)
    {
        foreach (var gameObject in GameObjects)
            gameObject.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Runtime.GraphicsDevice.Clear(Color.CornflowerBlue);
            
        Runtime.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        {
            foreach (var gameObject in GameObjects)
                gameObject.Draw(gameTime);
        }
        Runtime.SpriteBatch.End();
    }
}