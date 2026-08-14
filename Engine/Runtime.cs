using System;
using Engine.Audio;
using Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.ImGuiNet;

namespace Engine;

public class Runtime : Game
{
    internal static Runtime s_instance;

    public static Runtime Instance => s_instance;
    
    public static GraphicsDeviceManager Graphics { get; private set; }
    public new static GraphicsDevice GraphicsDevice { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    
    public static AudioController Audio { get; private set; }
    public static InputManager Input { get; private set; }

    public static ScreenManager ScreenManager { get; private set; }
    
    public new static ContentManager Content { get; private set; }

    public static BoxingViewportAdapter ViewportAdapter { get; set; }
    public static Action onViewportAdapterResize;
    
    public static ImGuiRenderer GuiRenderer;
    
    public static bool ExitOnEscape { get; set; }

    public static int VirtualWidth
    {
        get
        {
            if (ViewportAdapter == null) throw new NullReferenceException("ViewportAdapter has not been initialized.");
            return ViewportAdapter.VirtualWidth;
        }
        set
        {
            if (value != VirtualWidth)
                SetVirtualResolution(value, VirtualHeight);
        }
    }

    public static int VirtualHeight
    {
        get 
        {
            if (ViewportAdapter == null) throw new NullReferenceException("ViewportAdapter has not been initialized.");
            return ViewportAdapter.VirtualHeight;
        }
        set
        {
            if (value != VirtualHeight)
                SetVirtualResolution(VirtualWidth, value);
        }
    }
    
    private static int virtualWidth;
    private static int virtualHeight;

    public static bool Paused;

    public Runtime(string title, int width, int height, bool fullScreen, int virtualWidth = -1, int virtualHeight = -1)
    {
        if (s_instance != null)
            throw new InvalidOperationException($"Only a single runtime can be created");

        s_instance = this;
        
        Graphics = new GraphicsDeviceManager(this);
#if !BLAZORGL
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;
        Graphics.ApplyChanges();
#endif
        Runtime.virtualWidth = virtualWidth <= 0 ? width : virtualWidth;
        Runtime.virtualHeight = virtualHeight <= 0 ? height : virtualHeight;
        
        Window.Title = title;
        Window.AllowUserResizing = true;
        
        Content = base.Content;
        Content.RootDirectory = "Content";
        
        IsMouseVisible = true;
        ExitOnEscape = true;

        ScreenManager = new ScreenManager();
    }

    protected override void Initialize()
    {
        base.Initialize();
        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        
        GuiRenderer = new ImGuiRenderer(this);
        GuiRenderer.RebuildFontAtlas();
        
        Input = new InputManager();
        Audio = new AudioController();
        ScreenManager.Initialize();
        SetVirtualResolution(virtualWidth, virtualHeight);
    }

    protected override void UnloadContent()
    {
        Audio.Dispose();
        ScreenManager.Dispose();
        
        base.UnloadContent();
    }
    
    protected override void Update(GameTime gameTime)
    {
        if (Paused) return;
        
        Input.Update(gameTime);
        Audio.Update();

        if (ExitOnEscape && (Input.Keyboard.IsKeyDown(Keys.Escape) || Input.GamePads[0].IsButtonDown(Buttons.Back)))
        {
            try { Exit(); }
            catch (PlatformNotSupportedException) { /* ignore */ }
        }
        
        ScreenManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        ScreenManager.Draw(gameTime);
        
        base.Draw(gameTime);
    }

    public static void SetVirtualResolution(int virtualWidth, int virtualHeight)
    {
        Runtime.virtualWidth = virtualWidth;
        Runtime.virtualHeight = virtualHeight;
        ViewportAdapter = new BoxingViewportAdapter(Instance.Window, GraphicsDevice, virtualWidth, virtualHeight);
        onViewportAdapterResize?.Invoke();
    }
}