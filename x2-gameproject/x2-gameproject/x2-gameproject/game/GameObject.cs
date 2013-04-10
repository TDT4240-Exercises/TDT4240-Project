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
        #region Declarations

        public Texture2D Texture { get; protected set; }
        private Vector2 position;
        private Vector2 velocity;
        private float rotation;
        private int width;
        private int height;

        #endregion

        #region Public Methods

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float X {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float VelX
        {
            get { return velocity.X; }
            set { velocity.X = value; }
        }

        public float VelY
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public Vector2 RelativeCenter
        {
            get { return new Vector2(Width / 2.0f, Height / 2.0f); }
        }

        public Rectangle hitBox
        {
            get
            {
                return new Rectangle((int)X, (int)Y, Width, Height); //XNA does not support float rectangles natively
            }
        }

        /// <summary>
        /// Abstract update function that is called every update frame
        /// </summary>
        /// <param name="delta">Time since last update</param>
        /// <param name="keyboard"></param>
        /// <param name="mouse"></param>
        public abstract void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse);

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

        #endregion

        protected GameObject()
        {
            Velocity = new Vector2();
            Position = new Vector2();
            Texture = ResourceManager.InvalidTexture;
        }
    }
}
