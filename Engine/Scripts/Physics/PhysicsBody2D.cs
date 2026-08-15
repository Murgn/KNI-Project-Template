using System;
using Engine.Debugging;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

namespace Engine.Scripts.Physics;

public class PhysicsBody2D : Script
{
    public BodyType bodyType
    {
        get => _bodyType;
        set { _bodyType = value; if(body != null) body.BodyType = _bodyType; }
    }

    private BodyType _bodyType = BodyType.Dynamic;

    public Body body { get; private set; }
    public Fixture collider;

    public Vector2 Gravity { get; set; } = new Vector2(0.0f, 9.8f);
    public float GravityScale { get; set; } = 1.0f;
    
    public event Action<Fixture, Fixture, Contact> OnCollisionEnter;
    public event Action<Fixture, Fixture, Contact> OnCollisionExit;
    
    private Vector2 _lastSyncedPosition;
    private float _lastSyncedRotation;
    
    public override void Initialize()
    {
        base.Initialize();
        body = Runtime.PhysicsWorld.CreateBody(Transform.Position, Transform.Rotation, bodyType);
        body.IgnoreGravity = true;
        
        collider = body.CreateRectangle(Transform.Scale.X, Transform.Scale.Y, 1.0f, Vector2.Zero);

        SubscribeColliderEvents();
        
        _lastSyncedPosition = Transform.Position;
        _lastSyncedRotation = Transform.Rotation;
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (body.BodyType == BodyType.Dynamic)
        {
            if (Transform.Position != _lastSyncedPosition || Transform.Rotation != _lastSyncedRotation)
            {
                body.SetTransform(Transform.Position, Transform.Rotation);
                body.Awake = true;
            }
            
            body.ApplyForce(Gravity * GravityScale * body.Mass);
        }
    }

    public override void LateUpdate(GameTime gameTime)
    {
        base.LateUpdate(gameTime);
        
        if (body.BodyType == BodyType.Dynamic)
        {
            Transform.Position = body.Position;
            Transform.Rotation = body.Rotation;
        }
        else if (Transform.Position != _lastSyncedPosition || Transform.Rotation != _lastSyncedRotation)
        {
            body.SetTransform(Transform.Position, Transform.Rotation);
            body.Awake = true;
        }
        
        _lastSyncedPosition = Transform.Position;
        _lastSyncedRotation = Transform.Rotation;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        if (body != null)
            Runtime.PhysicsWorld.Remove(body);
    }
    
    private void SubscribeColliderEvents()
    {
        collider.OnCollision += (sender, other, contact) =>
        {
            OnCollisionEnter?.Invoke(sender, other, contact);
            return true;
        };

        collider.OnSeparation += (sender, other, contact) =>
        {
            OnCollisionExit?.Invoke(sender, other, contact);
        };
    }
    
    public void RebuildCollider()
    {
        if (collider != null)
            body.Remove(collider);

        collider = body.CreateRectangle(Transform.Scale.X, Transform.Scale.Y, 1.0f, Vector2.Zero);
        SubscribeColliderEvents();
        body.SetTransform(Transform.Position, Transform.Rotation);
    }
}