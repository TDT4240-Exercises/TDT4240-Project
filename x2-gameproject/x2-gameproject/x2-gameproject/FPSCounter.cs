using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using X2Game;

namespace x2_gameproject
{
    public class FPSCounter : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;

        private int frameRate;
        private int frameCounter;
        private TimeSpan elapsedTime = TimeSpan.Zero;


        public FPSCounter(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
        }


        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);

            spriteBatch.Begin();

            spriteBatch.DrawString(ResourceManager.GetDebugFont(), fps, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(ResourceManager.GetDebugFont(), fps, new Vector2(32, 32), Color.White);

            spriteBatch.End();
        }
    }
}
