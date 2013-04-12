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
        private Vector2 _position;
        private Vector2 _velocity;
        private float _rotation;
        private int _width;
        private int _height;
        protected internal bool IsCollidable;
        protected internal char Team;

        protected GameObject()
        {
            Velocity = new Vector2();
            Position = new Vector2();
            Texture = ResourceManager.InvalidTexture;
        }

        #region Public Methods

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float X {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float Y {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public float VelX
        {
            get { return _velocity.X; }
            set { _velocity.X = value; }
        }

        public float VelY
        {
            get { return _velocity.Y; }
            set { _velocity.Y = value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public Vector2 RelativeCenter
        {
            get { return new Vector2(Width / 2.0f, Height / 2.0f); }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)X, (int)Y, Width, Height); //XNA does not support float rectangles natively
            }
        }

        #endregion

        /// <summary>
        /// Abstract update function that is called every update frame
        /// </summary>
        /// <param name="delta">Time since last update</param>
        /// <param name="keyboard"></param>
        /// <param name="mouse"></param>
        public abstract void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse);

        #region Collision detection

        public bool HandleCollision(GameObject other, bool applyForce = true)
        {
            Vector2 en1Center = Position;
            int en1Radius = (Height + Width) / 4; // Average of height and width divided by 2 => 4

            Vector2 en2Center = other.Position;
            int en2Radius = (other.Height + other.Width) / 4; // Average of height and width divided by 2 => 4

            Vector2 diffVector = en1Center - en2Center; // From en2 to en1
            int distance = (int)diffVector.Length();

            //Collision?
            if (distance >= en1Radius + en2Radius) return false;

            if (applyForce)
            {
                int moveDistance = (en1Radius + en2Radius) - distance;
                diffVector /= distance;
                diffVector *= moveDistance / 2.0f;
                Position += diffVector;
                other.Position -= diffVector;
            }

            return true;
        }

        #endregion
    }
}
