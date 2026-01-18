using System;
using Gum.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;

namespace Engine.Graphics;

// TODO:: REFACTOR LOGIC INTO BAREBONES RENDERTEXTURE AND THEN
// TODO:: _____ RENDER TEXTURE WHICH CONTAINS RESOLUTION SCALING ETC
public class RenderTexture
{
    public RenderTarget2D Target { get; private set; }
    public int renderWidth { get; private set; }
    public int renderHeight { get; private set; }

    public bool PixelPerfectScaling { get; set; }
    
    public GraphicsDevice GraphicsDevice { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }
    public GumService GumUI { get; private set; }

    private GameWindow Window;
    
    private float scale;
    private Rectangle destination;

    public RenderTexture(GameWindow window)
    {
        renderWidth = 256;
        renderHeight = 144;
        Window = window;
    }

    public RenderTexture(int width, int height, GameWindow window)
    {
        renderWidth = width;
        renderHeight = height;
        Window = window;
    }

    public void Initialize(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, GumService gumUI)
    {
        GraphicsDevice = graphicsDevice;
        SpriteBatch = spriteBatch;
        GumUI = gumUI;
        
        Target = new RenderTarget2D(GraphicsDevice, renderWidth, renderHeight);
        Window.ClientSizeChanged += Resize;
        Resize(null, EventArgs.Empty);
    }
    
    public void Draw()
    {
        GraphicsDevice.SetRenderTarget(null);
        
        // Draw RenderTexture to Screen
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        {
            SpriteBatch.Draw(Target, destination, Color.White);
        }
        SpriteBatch.End();
    }

    public void Resize(object sender, EventArgs e)
    {
        int windowWidth  = GraphicsDevice.PresentationParameters.BackBufferWidth;
        int windowHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        float scaleX = (float)windowWidth  / renderWidth;
        float scaleY = (float)windowHeight / renderHeight;

        scale = MathF.Min(scaleX, scaleY);

        if(PixelPerfectScaling)
        {
            scale = MathF.Floor(scale);
            scale = MathF.Max(scale, 1f);
        }
        
        int drawWidth  = (int)(renderWidth  * scale);
        int drawHeight = (int)(renderHeight * scale);

        int offsetX = (windowWidth  - drawWidth)  / 2;
        int offsetY = (windowHeight - drawHeight) / 2;

        destination = new Rectangle(offsetX, offsetY, drawWidth, drawHeight);
        ResizeGumCanvas();
    }

    public void ResizeGumCanvas()
    {
        GumUI.CanvasWidth = renderWidth;
        GumUI.CanvasHeight = renderHeight;

        Matrix translateMatrix = Matrix.CreateTranslation(-destination.X, -destination.Y, 0f);
        Matrix scaleMatrix = Matrix.CreateScale(1.0f / scale, 1.0f / scale, 1f);

        GumUI.Cursor.TransformMatrix = translateMatrix * scaleMatrix;    
    }
}