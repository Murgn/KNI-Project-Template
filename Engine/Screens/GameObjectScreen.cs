using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Engine.Debugging;
using Engine.Maths;
using Engine.Scripts.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace Engine.Screens;

public abstract class GameObjectScreen : GameScreen
{
    public string Name { get; private set; }
    public readonly List<GameObject> GameObjects = new();
    
    public GameObject CameraGameObject { get; private set; }
    public CameraScript CameraScript { get; private set; }
    
    protected GameObjectScreen(Game game, string name) : base(game)
    {
        Name = name;
    }

    public override void Initialize()
    {
        base.Initialize();
        
        CameraGameObject = CreateGameObject("Camera", new[] { typeof(CameraScript) });
        CameraScript = CameraGameObject.GetScript<CameraScript>();
    }

    public override void Draw(GameTime gameTime)
    {
        Runtime.GraphicsDevice.Clear(CameraScript.ClearColor);
            
        var orthoCamera = CameraScript.OrthoCamera;
        Matrix cameraMatrix = orthoCamera.GetViewMatrix();
            
        Runtime.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: orthoCamera.GetViewMatrix(), rasterizerState: RasterizerState.CullNone);
        {
            foreach (var gameObject in GameObjects)
                gameObject.Draw(gameTime);
        }
        Runtime.SpriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var gameObject in GameObjects)
            gameObject.Update(gameTime);
    }

    public virtual void LateUpdate(GameTime gameTime)
    {
        foreach (var gameObject in GameObjects)
            gameObject.LateUpdate(gameTime);
    }

    public virtual void DrawUI(GameTime gameTime) { }

    public override void UnloadContent()
    {
        base.UnloadContent();

        foreach (var gameObject in GameObjects)
            gameObject.Destroy();
        
        GameObjects.Clear();
    }

    public void SaveToFile(ContentManager content, string path)
    {
        XElement screenElement = new XElement("Screen", new XAttribute("name", Name));
        
        foreach (var gameObject in GameObjects)
        {
            XElement gameObjectElement = new XElement("GameObject", new XAttribute("name", gameObject.Name));
            
            XElement transformElement = new XElement("Transform",
                new XAttribute("position", gameObject.Transform.Position),
                new XAttribute("rotation", gameObject.Transform.Rotation),
                new XAttribute("scale", gameObject.Transform.Scale));
            
            XElement scriptsElement = new XElement("Scripts");
            foreach (var script in gameObject.scripts)
            {
                XElement scriptElement = new XElement("Script", new XAttribute("type", script.GetType().AssemblyQualifiedName));
                
                scriptsElement.Add(scriptElement);
            }
            
            gameObjectElement.Add(transformElement);
            gameObjectElement.Add(scriptsElement);
            
            screenElement.Add(gameObjectElement);
        }
        
        Directory.CreateDirectory(path);
        screenElement.Save(Path.Combine(path, $"{Name}.xml"));

        // hard coded !! alert alert very bad !!
        string runtimePath = Path.Combine(content.RootDirectory, "screens", $"{Name}.xml");
        screenElement.Save(runtimePath);
    }

    public void LoadFromFile(ContentManager content, string path)
    {
        GameObjects.Clear();
        string filePath = Path.IsPathRooted(path) ? path : Path.Combine(content.RootDirectory, path + ".xml");
        
        XElement screenElement = XElement.Load(filePath);
        Name = screenElement.Attribute("name")?.Value;

        foreach (var gameObjectElement in screenElement.Elements("GameObject"))
        {
            string name = gameObjectElement.Attribute("name")?.Value;
            
            GameObject gameObject = CreateGameObject(name);
            
            XElement transformElement = gameObjectElement.Element("Transform");
            string positionValue = transformElement?.Attribute("position")?.Value;
            string rotationValue = transformElement?.Attribute("rotation")?.Value;
            string scaleValue = transformElement?.Attribute("scale")?.Value;

            gameObject.Transform.Position = positionValue.Parse2();
            gameObject.Transform.Rotation = float.Parse(rotationValue);
            gameObject.Transform.Scale = scaleValue.Parse2();

            XElement scriptsElement = gameObjectElement.Element("Scripts");
            foreach (var scriptElement in scriptsElement.Elements("Script"))
            {
                string typeValue = scriptElement.Attribute("type")?.Value;

                Type scriptType = Type.GetType(typeValue);
                if (scriptType == null) continue;
                
                gameObject.AddScript(scriptType);
            }

            // hacky workaround
            if (gameObject.HasScript<CameraScript>())
            {
                CameraGameObject = gameObject;
                CameraScript = gameObject.GetScript<CameraScript>();
            }
        }
    }
    
    public GameObject CreateGameObject(string name, IEnumerable<Type> scripts)
    {
        var gameObject = new GameObject(this, name, scripts);
        GameObjects.Add(gameObject);
        return gameObject;
    }
    
    public GameObject CreateGameObject(string name, Type script) => CreateGameObject(name, new []{ script });
    
    public GameObject CreateGameObject(string name) => CreateGameObject(name, Array.Empty<Type>());
    
    public GameObject FindGameObjectByName(string name) 
        => GameObjects.FirstOrDefault(go => go.Name == name && !go.IsDestroyed);
    
    public IEnumerable<GameObject> FindGameObjectsByScript<T>() where T : Script 
        => GameObjects.Where(gameObject => !gameObject.IsDestroyed && gameObject.HasScript<T>());
    
    public GameObject FindGameObjectByScript<T>() where T : Script
        => FindGameObjectsByScript<T>().FirstOrDefault();
}