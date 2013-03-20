using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game {
    class IntroState : GameState {

        public IntroState()
        {
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
                ParticleEngine.SpawnParticle(new Vector2(rand.Next(800), rand.Next(640)), ResourceManager.GetParticleTemplate("fireball.xml"));
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ResourceManager.GetTexture("splash.jpg"), new Rectangle(0, 0, 800, 640), null, Color.White);
        }


        protected override void Update(KeyboardState keyboard, MouseState mouse)
        {
            if (keyboard.GetPressedKeys().Length > 0 && !keyboard.IsKeyDown(Keys.Escape)) {
                NextGameState = new MainMenuState();
            }
        }

    }
}