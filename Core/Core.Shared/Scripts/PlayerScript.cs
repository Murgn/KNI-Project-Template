using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Engine;
using Engine.Debugging;
using Engine.Input;
using Engine.Scripts.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

namespace Core.Scripts
{
    public class PlayerScript : Script
    {
        // Movement
        public int speed = 16;
        
        // Jumping
        public int jumpForce = 16;
        public bool isGrounded => groundContacts > 0;
        private int groundContacts;
        
        // Gravity
        [Inspect] private float idleGravityScale = 1.0f;
        [Inspect] private float fallingGravityScale = 2.0f;
        
        // Scripts
        private PhysicsBody2D physicsBody2D;

        public override void Start()
        {
            base.Start();
            physicsBody2D = GetScript<PhysicsBody2D>();
            physicsBody2D.OnCollisionEnter += OnCollisionEnter;
            physicsBody2D.OnCollisionExit += OnCollisionExit;
            physicsBody2D.body.FixedRotation = true;
            physicsBody2D.collider.Friction = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            Movement();
            Gravity();
        }

        private void Movement()
        {
            KeyboardInfo keyboard = Runtime.Input.Keyboard;
            Vector2 dir = Vector2.Zero;
        
            if (keyboard.IsKeyPressed(Keys.Space) && isGrounded) 
                physicsBody2D.body.ApplyLinearImpulse(-Vector2.UnitY * jumpForce);
            if (keyboard.IsKeyReleased(Keys.Space) && physicsBody2D.body.LinearVelocity.Y < 0) // variable jump height
                physicsBody2D.body.LinearVelocity = new Vector2(physicsBody2D.body.LinearVelocity.X, physicsBody2D.body.LinearVelocity.Y * 0.25f);
            if (keyboard.IsKeyDown(Keys.A)) dir.X--;
            if (keyboard.IsKeyDown(Keys.D)) dir.X++;
            if(dir != Vector2.Zero) dir.Normalize();
            
            physicsBody2D.body.LinearVelocity = new Vector2(dir.X * speed, physicsBody2D.body.LinearVelocity.Y);
        }

        private void Gravity()
        {
            physicsBody2D.GravityScale = physicsBody2D.body.LinearVelocity.Y >= 0.2f ? fallingGravityScale : idleGravityScale;
        }
        
        private void OnCollisionEnter(Fixture self, Fixture other, Contact contact)
        {
            if (IsGroundBelow(self, contact))
                groundContacts++;
        }

        private void OnCollisionExit(Fixture self, Fixture other, Contact contact)
        {
            groundContacts = Math.Max(groundContacts - 1, 0);
        }
        
        private static bool IsGroundBelow(Fixture self, Contact contact)
        {
            contact.GetWorldManifold(out Vector2 normal, out FixedArray2<Vector2> points);
            
            if (contact.FixtureB == self)
                normal = -normal;

            Vector2 down = Vector2.UnitY;

            float alignment = Vector2.Dot(normal, down);

            const float threshold = 0.6f;
            return alignment > threshold;
        }
    }
}