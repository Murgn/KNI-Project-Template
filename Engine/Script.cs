using System;
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
    public virtual void Draw(GameTime gameTime) { }
    public virtual void OnDestroy() { }
    
    public T GetScript<T>() where T : Script => GameObject.GetScript<T>();
}