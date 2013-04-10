using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X2Game;

namespace X2Game
{
    class RenderEngine
    {
        private readonly Texture2D _mouseCursor;
        private readonly SpriteBatch _spriteBatch;

        public RenderEngine(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _mouseCursor = ResourceManager.GetTexture("cursor.png");
        }

        public void Render(GameObject gameObject)
        {
            if (Camera.ObjectIsVisible(gameObject.hitBox))
            {
                _spriteBatch.Draw(
                    gameObject.Texture,
                    gameObject.Position - Camera.Position,
                    null,
                    Color.White,
                    gameObject.Rotation,
                    gameObject.RelativeCenter,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }

        public int GetScreenWidth()
        {
            return _spriteBatch.GraphicsDevice.Viewport.Width;
        }

        public int GetScreenHeight()
        {
            return _spriteBatch.GraphicsDevice.Viewport.Height;
        }

        public void Render(TileMap tileMap)
        {
            tileMap.Draw(_spriteBatch);
        }

        public void DrawString(string text, float x, float y, Color color, bool center = false)
        {
            //Center text?
            if (center)
            {
                Vector2 offset = ResourceManager.GetDebugFont().MeasureString(text) / 2;
                x -= offset.X;
                y -= offset.Y;
            }

            //Draw the string!
            _spriteBatch.DrawString(ResourceManager.GetDebugFont(), text, new Vector2(x, y), color);
        }

        public void Draw(Texture2D texture, float x, float y, float width, float height)
        {
            _spriteBatch.Draw(texture, new Rectangle((int)x, (int)y, (int)width, (int)height), Color.White);
        }

        public void DrawFilledRectangle(Rectangle rectangle, Color color)
        {
            _spriteBatch.Draw(ResourceManager.InvalidTexture, rectangle, color);
        }

        public void DrawFilledRectangle(int x, int y, int width, int height, Color color)
        {
            DrawFilledRectangle(new Rectangle(x, y, width, height), color);
        }

        public void BeginFrame()
        {
            //Start new frame
            _spriteBatch.GraphicsDevice.Clear(Color.Black);

            //Render current state
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        }

        public void EndFrame()
        {
            //Draw makeshift mouse pointer (last so it is on top)
            _spriteBatch.Draw(_mouseCursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32), Color.White);

            _spriteBatch.End();
        }
    }
}
