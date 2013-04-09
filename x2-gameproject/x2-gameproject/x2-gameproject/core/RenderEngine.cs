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
        public Rectangle Camera;

        public RenderEngine(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            Camera = new Rectangle(0, 0, 800, 600);
            _mouseCursor = ResourceManager.GetTexture("cursor.png");
        }

        public bool IsVisible(Rectangle rectangle)
        {
            return Camera.Intersects(rectangle);
        }

        public void Render(GameObject gameObject)
        {
            Vector2 scale = new Vector2();

            scale.X = 1.0f / gameObject.Texture.Width * gameObject.Width;
            scale.Y = 1.0f / gameObject.Texture.Height * gameObject.Height;

            //Calculate draw position on screen
            Vector2 realPosition = gameObject.GetPosition();
            realPosition.X -= Camera.X;
            realPosition.Y -= Camera.Y;

            _spriteBatch.Draw(
                gameObject.Texture,
                realPosition,
                null,
                Color.White,
                gameObject.Rotation,
                new Vector2(gameObject.Width / 2.0f, gameObject.Height / 2.0f),
                scale,
                SpriteEffects.None,
                1.0f
            );
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
            //Figure out the visible bounds of our map
            int sttX = Math.Max(0, Camera.X / TileType.TILE_WIDTH - 2);
            int sttY = Math.Max(0, Camera.Y / TileType.TILE_HEIGHT);
            int endX = Math.Min(tileMap.Width, sttX + Camera.Width / TileType.TILE_WIDTH);
            int endY = Math.Min(tileMap.Height, sttY + Camera.Height / TileType.TILE_HEIGHT);

            //Scale each tile to our prefered resolution (how many can we fit inside the camera?)
            int width = (Camera.Width / (endX - sttX - 3));
            int height = (Camera.Height / (endY - sttY));

            //Draw the entire visible map
            for (int x = sttX; x < endX; ++x)
            {
                for (int y = sttY; y < endY; ++y)
                {
                    tileMap[x, y].Draw(x * width - Camera.X, y * height - Camera.Y, width, height, _spriteBatch);
                }
            }

            //Debug info
            _spriteBatch.DrawString(ResourceManager.GetDebugFont(), "Camera: " + Camera, new Vector2(), Color.White);
            _spriteBatch.DrawString(ResourceManager.GetDebugFont(), "sttX: " + sttX + ", " + "sttY: " + sttY + ", " + "endX: " + endX + ", " + "endY: " + endY, new Vector2(0, 16), Color.White);            
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
