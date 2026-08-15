using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Screens;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Engine;

public class Script
{
    public GameObject GameObject { get; internal set; }
    public Transform2 Transform => GameObject.Transform;
    public bool Enabled { get; set; } = true;
    
    public virtual void Initialize() { }
    public virtual void Start() { }
    public virtual void Update(GameTime gameTime) { }
    public virtual void LateUpdate(GameTime gameTime) { }
    public virtual void Draw(GameTime gameTime) { }
    public virtual void OnDestroy() { }
    
    public T GetScript<T>() where T : Script => GameObject.GetScript<T>();

    public GameObject FindGameObjectByName(string name) 
        => GameObject.ParentScreen.FindGameObjectByName(name);

    public IEnumerable<GameObject> FindGameObjectsByScript<T>() where T : Script 
        => GameObject.ParentScreen.FindGameObjectsByScript<T>();

    public GameObject FindGameObjectByScript<T>() where T : Script 
        => GameObject.ParentScreen.FindGameObjectByScript<T>();

}