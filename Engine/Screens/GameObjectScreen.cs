using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Screens;

namespace Engine.Screens;

public abstract class GameObjectScreen : GameScreen
{
    public readonly List<GameObject> GameObjects = new();
    public string Name { get; private set; }

    protected GameObjectScreen(Game game, string name) : base(game)
    {
        Name = name;
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
        string filePath = Path.Combine(content.RootDirectory, path);
        
        XElement screenElement = XElement.Load(filePath);

        foreach (var gameObjectElement in screenElement.Elements("GameObject"))
        {
            string name = gameObjectElement.Attribute("name")?.Value;
            GameObject gameObject = new GameObject(name);

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

            GameObjects.Add(gameObject);
        }
    }
}