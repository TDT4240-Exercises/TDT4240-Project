using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace X2Game
{
    public static class Camera
    {
        #region Declarations
        private static Vector2 position = Vector2.Zero;
        private static Vector2 viewPortSize = Vector2.Zero;
        private static Rectangle worldRectangle = new Rectangle(0, 0, 0, 0);
        public static long shake;
        private static readonly Random rand = new Random();
        #endregion

        #region Properties
        public static Vector2 Position
        {
            get
            {
                if (shake > 0)
                {
                    return position + new Vector2(rand.Next(128) - 64, rand.Next(129) - 64);
                }
                else
                {
                    return position;
                }
            }
            set
            {
                position = new Vector2(
                    MathHelper.Clamp(value.X,
                        worldRectangle.X,
                        worldRectangle.Width - ViewPortWidth),
                    MathHelper.Clamp(value.Y,
                        worldRectangle.Y,
                        worldRectangle.Height - ViewPortHeight));
            }
        }

        public static Rectangle WorldRectangle
        {
            get { return worldRectangle; }
            set { worldRectangle = value; }
        }

        public static int ViewPortWidth
        {
            get { return (int)viewPortSize.X; }
            set { viewPortSize.X = value; }
        }

        public static int ViewPortHeight
        {
            get { return (int)viewPortSize.Y; }
            set { viewPortSize.Y = value; }
        }

        public static Rectangle ViewPort
        {
            get
            {
                return new Rectangle(
                    (int)Position.X, (int)Position.Y,
                    ViewPortWidth, ViewPortHeight);
            }
        }
        #endregion

        #region Public Methods
        public static void Move(Vector2 offset)
        {
            Position += offset;
        }

        public static bool ObjectIsVisible(Rectangle bounds)
        {
            return (ViewPort.Intersects(bounds));
        }

        public static Vector2 WorldToScreen(Vector2 point)
        {
            return point - position;
        }

        public static Rectangle WorldToScreen(Rectangle rectangle)
        {
            return new Rectangle(
                rectangle.Left - (int)position.X,
                rectangle.Top - (int)position.Y,
                rectangle.Width,
                rectangle.Height);
        }
        #endregion
    }
}
