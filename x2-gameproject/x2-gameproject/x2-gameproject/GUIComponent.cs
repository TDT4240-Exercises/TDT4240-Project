using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace x2_gameproject
{
    abstract class GUIComponent
    {
        public Rectangle Bounds;

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update();
    }
}
