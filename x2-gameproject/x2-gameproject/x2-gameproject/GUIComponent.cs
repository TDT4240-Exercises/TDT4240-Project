using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    abstract class GUIComponent
    {
        public Rectangle Bounds;

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(KeyboardState keyboard, MouseState mouse);
    }
}
