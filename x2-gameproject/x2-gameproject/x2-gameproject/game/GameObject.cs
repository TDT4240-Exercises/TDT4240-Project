using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    /// <summary>
    /// A high-level abstraction for all interactive objects in the game (players, enemies, projectiles and whatnot).
    /// This utilizes the Composite pattern.
    /// </summary>
    abstract class GameObject
    {
        public Texture2D Texture { get; protected set; }

        public Vector2 Velocity;
        public Vector2 Position;
        public float Rotation { get; protected set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Rectangle hitBox
        {
            get
            {
                return new Rectangle(GetX(), GetY(), Width, Height); //XNA does not support float rectangles natively
            }
        }

        protected GameObject()
        {
            Velocity = new Vector2();
            Position = new Vector2();
            Texture = ResourceManager.InvalidTexture;
        }

        /// <summary>
        /// Abstract update function that is called every update frame
        /// </summary>
        /// <param name="delta">Time since last update</param>
        /// <param name="keyboard"></param>
        /// <param name="mouse"></param>
        public abstract void Update(TimeSpan delta, KeyboardState? keyboard, MouseState? mouse);

        public int GetX()
        {
            return (int) Position.X;
        }

        public int GetY()
        {
            return (int) Position.Y;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(Position.X, Position.Y); //return new instance so that they dont have a reference to our private variable
        }

        public Vector2 GetVelocity()
        {
            return new Vector2(Velocity.X, Velocity.Y); //return new instance so that they dont have a reference to our private variable
        }

        /// <summary>
        /// Returns true if this object has collided with the specified GameObject. A object cannot collide with itself
        /// </summary>
        /// <param name="other">Which object to check the collision with</param>
        /// <returns>true if it has collided with the GameObject, false otherwise</returns>
        public virtual bool CollidesWith(GameObject other)
        {
            //Never collide with ourselves
            if (other.Equals(this)) return false;

            return other.hitBox.Intersects(hitBox);
        }

    }
}
