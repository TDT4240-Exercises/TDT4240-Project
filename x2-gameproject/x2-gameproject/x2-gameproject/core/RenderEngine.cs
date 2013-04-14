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

        public void Render(Entity entity)
        {
            if (Camera.ObjectIsVisible(entity.Bounds))
            {
                Vector2 drawPosition = entity.Position - Camera.Position;

                //the unit itself
                _spriteBatch.Draw(
                    entity.Texture,
                    drawPosition,
                    null,
                    Color.White,
                    entity.Rotation,
                    entity.RelativeCenter,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);

                //Max health
                DrawFilledRectangle((int)drawPosition.X - entity.Width / 2, (int)drawPosition.Y - entity.Height / 2 - 24, 
                    entity.Width, 4, Color.Black);

                //remaining health
                DrawFilledRectangle((int)drawPosition.X - entity.Width / 2, (int)drawPosition.Y - entity.Height / 2 - 24,
                    (int)(entity.Width * (1.0 / entity.MaxHealth * entity.Health)), 4, entity is Player ? Color.Green : Color.Red);
            }
        }

        public void Render(Particle particle)
        {
            Rectangle target = particle.Bounds;

            if (!Camera.ObjectIsVisible(target)) return;

            //Calculate screen position
            target.X -= (int) Camera.Position.X;
            target.Y -= (int) Camera.Position.Y;

            //Draw it there
            _spriteBatch.Draw(particle.Texture, target, null, Color.White * particle.Alpha, 
                particle.Rotation, new Vector2(particle.Texture.Width / 2.0f, particle.Texture.Height / 2.0f), SpriteEffects.None, 0);
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
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        }

        public void EndFrame()
        {
            //Draw makeshift mouse pointer (last so it is on top)
            _spriteBatch.Draw(_mouseCursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32), Color.White);

            _spriteBatch.End();
        }
    }
}
