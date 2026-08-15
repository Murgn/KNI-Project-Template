using System;
using System.Runtime.InteropServices;
using Engine.Audio;
using Engine.Debugging;
using Engine.Input;
using Engine.Screens;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.ImGuiNet;
using nkast.Aether.Physics2D.Diagnostics;
using nkast.Aether.Physics2D.Dynamics;

namespace Engine;

public class Runtime : Game
{
    internal static Runtime s_instance;
    public static Runtime Instance => s_instance;

    public static World PhysicsWorld { get; private set; }
    
    public static GraphicsDeviceManager Graphics { get; private set; }
    public new static GraphicsDevice GraphicsDevice { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    
    public static AudioController Audio { get; private set; }
    public static InputManager Input { get; private set; }

    public static CustomScreenManager ScreenManager { get; private set; }
    
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
    
// #if !BLAZORGL
//     [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
//     public static extern void SDL_MaximizeWindow(IntPtr window);
// #endif
    
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

        ScreenManager = new CustomScreenManager();
        
        PhysicsWorld = new World();
        PhysicsWorld.Gravity = new Vector2(0f, 0f);
        
    }

    protected override void Initialize()
    {
        base.Initialize();
        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        
        GuiRenderer = new ImGuiRenderer(this);
        GuiRenderer.RebuildFontAtlas();
        SetupImGuiStyle();
        
        var consoleWriter = new ConsoleWriter(Console.Out);
        consoleWriter.OnWrite += text => Debug.ConsoleLogs.Add(text);
        Console.SetOut(consoleWriter);
        
        Input = new InputManager();
        Audio = new AudioController();
        ScreenManager.Initialize();
        SetVirtualResolution(virtualWidth, virtualHeight);
        
// #if !BLAZORGL
//         SDL_MaximizeWindow(Window.Handle);
// #endif
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
        
        if (ExitOnEscape && (Input.Keyboard.IsKeyDown(Keys.Escape) || Input.GamePads[0].IsButtonDown(Buttons.Back)))
        {
            try { Exit(); }
            catch (PlatformNotSupportedException) { /* ignore */ }
        }
        
        Input.Update(gameTime);
        Audio.Update();
        
        ScreenManager.Update(gameTime);
        
        float dt = 1f/60f;
        PhysicsWorld.Step(dt);
        
        ScreenManager.LateUpdate(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        ScreenManager.Draw(gameTime);
        
        GuiRenderer.BeginLayout(gameTime);
            if (ScreenManager.ActiveScreen is GameObjectScreen screen)
                screen.DrawUI(gameTime);
        GuiRenderer.EndLayout();
        
        
        
        base.Draw(gameTime);
    }

    private void SetupImGuiStyle()
    {
        var colors = ImGui.GetStyle().Colors;

        for (int i = 0; i < (int)ImGuiCol.COUNT; i++)
        {
            System.Numerics.Vector4 color = colors[i];

            float gray = color.X * 0.299f + color.Y * 0.587f + color.Z * 0.114f;

            colors[i] = new System.Numerics.Vector4(gray, gray, gray, color.W);
        }
    }

    public static void SetVirtualResolution(int virtualWidth, int virtualHeight)
    {
        Runtime.virtualWidth = virtualWidth;
        Runtime.virtualHeight = virtualHeight;
        ViewportAdapter = new BoxingViewportAdapter(Instance.Window, GraphicsDevice, virtualWidth, virtualHeight);
        onViewportAdapterResize?.Invoke();
    }
}