using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    /// <summary>
    /// A high-level abstraction for all interactive objects in the game (players, enemies, projectiles and whatnot).
    /// This utilizes the Composite pattern.
    /// </summary>
    class GameObject
    {
        protected Vector2 velocity;

        public Vector2 position 
        { 
            public get{ return new Vector2(position.X, position.Y); } 
            public set; 
        }
        public int width { get; private set; }
        public int height { get; private set; }

        public Rectangle hitBox
        {
            public get
            {
                return new Rectangle((int)position.X, (int)position.Y, width, height); //XNA does not support float rectangles natively
            }
        }

        public GameObject()
        {
            velocity = new Vector2();
        }

        /// <summary>
        /// Abstract update function that is called every update frame
        /// </summary>
        /// <param name="delta">Time since last update</param>
        abstract void update(GameTime delta);

        /// <summary>
        /// Abstract function for rendering this GameObject on the screen
        /// </summary>
        abstract void render(SpriteBatch spriteBatch);

        public float getX()
        {
            return hitBox.X;
        }

        public float getY()
        {
            return hitBox.Y;
        }

        public Vector2 getPosition()
        {
            return new Vector2(hitBox.X, hitBox.Y); //return new instance so that they dont have a reference to our private variable
        }

        public Vector2 getVelocity()
        {
            return new Vector2(velocity.X, velocity.Y); //return new instance so that they dont have a reference to our private variable
        }

        public void addVelocity(float x, float y)
        {
            velocity.X += x;
            velocity.Y += y;
        }

        /// <summary>
        /// Returns true if this object has collided with the specified GameObject. A object cannot collide with itself
        /// </summary>
        /// <param name="other">Which object to check the collision with</param>
        /// <returns>true if it has collided with the GameObject, false otherwise</returns>
        public virtual Boolean collidesWith(GameObject other)
        {
            //Never collide with ourselves
            if (other.Equals(this)) return false;

            return other.hitBox.Intersects(this.hitBox);
        }
    }
}
