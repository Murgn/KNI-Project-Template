using System;
using Engine.Audio;
using Engine.Graphics;
using Engine.Input;
using Engine.Scenes;
using Gum.Forms.Controls;
using MonoGameGum;
using Gum.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGameGum.Input;
using ToolsUtilities;

namespace Engine;

public class Runtime : Game
{
    internal static Runtime s_instance;

    public static Runtime Instance => s_instance;

    private static Scene activeScene;
    private static Scene nextScene;
    
    public static GraphicsDeviceManager Graphics { get; private set; }
    public new static GraphicsDevice GraphicsDevice { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    public static RenderTexture RenderTexture { get; private set; }
    
    public static AudioController Audio { get; private set; }
    public static InputManager Input { get; private set; }

    public static ScreenManager ScreenManager { get; private set; }
    
    public new static ContentManager Content { get; private set; }
    public static GumService GumUI => GumService.Default;

    public static bool ExitOnEscape { get; set; }
    

    public Runtime(string title, int width, int height, bool fullScreen, int virtualWidth = 256, int virtualHeight = 144)
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

        RenderTexture = new RenderTexture(virtualWidth, virtualHeight, Window)
        {
            PixelPerfectScaling = false
        };

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
        InitializeGum();
        RenderTexture.Initialize(GraphicsDevice, SpriteBatch, GumUI);
        
        Input = new InputManager();
        Audio = new AudioController();
        ScreenManager.Initialize();
    }

    protected override void UnloadContent()
    {
        Audio.Dispose();
        ScreenManager.Dispose();
        
        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update(gameTime);
        GumUI.Update(gameTime);
        
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
        
        GumUI.Draw();
        RenderTexture.Draw();

        base.Draw(gameTime);
    }

    private void InitializeGum()
    {
        GumUI.Initialize(this, DefaultVisualsVersion.V3);
        FileManager.RelativeDirectory = Content.RootDirectory;
        // TODO: Not included in this version of Gum.KNI (as of 17/01/26), update NuGet package in a week or two!
        // GumService.Default.ContentLoader.XnaContentManager = Core.Content;
        FrameworkElement.KeyboardsForUiControl.Add(GumUI.Keyboard);
        FrameworkElement.TabReverseKeyCombos.Add(new KeyCombo { PushedKey = Keys.Up});
        FrameworkElement.TabKeyCombos.Add(new KeyCombo { PushedKey = Keys.Down});
    }
}