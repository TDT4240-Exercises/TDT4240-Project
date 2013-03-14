using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    /// <summary>
    /// A high-level abstraction for all interactive objects in the game (players, enemies, projectiles and whatnot).
    /// This utilizes the Composite pattern.
    /// </summary>
    abstract class GameObject
    {
        protected Vector2 velocity;
        protected Vector2 position;

        protected float rotation;

        public int width { get; protected set; }
        public int height { get; protected set; }

        public Rectangle hitBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, width, height); //XNA does not support float rectangles natively
            }
        }

        protected GameObject()
        {
            velocity = new Vector2();
            position = new Vector2();
        }

        /// <summary>
        /// Abstract update function that is called every update frame
        /// </summary>
        /// <param name="delta">Time since last update</param>
        public abstract void Update(TimeSpan delta);

        /// <summary>
        /// Abstract function for rendering this GameObject on the screen
        /// </summary>
        public abstract void Render(SpriteBatch spriteBatch);

        public float GetX()
        {
            return position.X;
        }

        public float GetY()
        {
            return position.Y;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(position.X, position.Y); //return new instance so that they dont have a reference to our private variable
        }

        public Vector2 GetVelocity()
        {
            return new Vector2(velocity.X, velocity.Y); //return new instance so that they dont have a reference to our private variable
        }

        public void AddVelocity(float x, float y)
        {
            velocity.X += x;
            velocity.Y += y;
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

            return other.hitBox.Intersects(this.hitBox);
        }
    }
}
