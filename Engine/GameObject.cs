using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Screens;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Engine;

public class GameObject
{
    public string Name { get; set; }
    public Transform2 Transform { get; private set; } = new(Vector2.Zero, 0.0f, Vector2.One);
    
    public bool IsDestroyed { get; private set; }
    
    public readonly List<Script> scripts = new();
    
    public GameObjectScreen ParentScreen { get; private set; }
    
    public GameObject(GameObjectScreen parentScene, string name, IEnumerable<Type> scripts)
    {
        ParentScreen = parentScene;
        Name = name;
        
        var types = scripts.ToList();

        foreach (var type in types)
            AddScript(type);
        
        foreach (var script in this.scripts)
            script.Start();
    }


    public void Initialize()
    {
        if (IsDestroyed) return;

        foreach (var script in scripts.Where(script => script.Enabled))
        {
            script.Initialize();
            script.Start();
        }

        //LoadContent();
    }
    // Not too sure whether to include these rn, i thiiinkk... i will though
    // public void LoadContent() { }
    // public void UnloadContent() { }

    public void Update(GameTime gameTime)
    {
        if (IsDestroyed) return;

        foreach (var script in scripts.Where(script => script.Enabled))
            script.Update(gameTime);
    }
    
    public void LateUpdate(GameTime gameTime)
    {
        if (IsDestroyed) return;

        foreach (var script in scripts.Where(script => script.Enabled))
            script.LateUpdate(gameTime);
    }


    public void Draw(GameTime gameTime)
    {
        if (IsDestroyed) return;
        
        foreach (var script in scripts.Where(script => script.Enabled))
            script.Draw(gameTime);
    }

    public void Destroy()
    {
        if (IsDestroyed) return;
        IsDestroyed = true;

        foreach (var script in scripts)
            script.OnDestroy();
        
        scripts.Clear();
    }
    
    public T AddScript<T>() where T : Script, new()
    {
        if (IsDestroyed)
            throw new InvalidOperationException("Cannot add script, GameObject is destroyed.");

        Script script = new T { GameObject = this };
        scripts.Add(script);
        script.Initialize();
        script.Start();
        return (T)script;
    }

    public Script AddScript(Type scriptType)
    {
        if (IsDestroyed)
            throw new InvalidOperationException("Cannot add script, GameObject is destroyed.");
        
        if (!typeof(Script).IsAssignableFrom(scriptType))
            throw new ArgumentException("Cannot add script, Type must inherit Script");

        Script script = (Script)Activator.CreateInstance(scriptType);
        script.GameObject = this;
        scripts.Add(script);
        script.Initialize();
        return script;
    }
    
    public T GetScript<T>() where T : Script => scripts.OfType<T>().FirstOrDefault();
    
    public bool HasScript<T>() where T : Script => scripts.Any(s => s is T);

    public void RemoveScript(Script script)
    {
        script.OnDestroy();
        script.GameObject = null;
        scripts.Remove(script);
    }

}