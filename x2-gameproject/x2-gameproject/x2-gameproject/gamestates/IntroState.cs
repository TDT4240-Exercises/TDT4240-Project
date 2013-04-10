using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class IntroState : GameState
    {

        private bool start = false;

        public IntroState()
        {

            Random rand = new Random();
            for (int i = 0; i < 100; i++)
                ParticleEngine.SpawnParticle(new Vector2(rand.Next(800), rand.Next(640)), ResourceManager.GetParticleTemplate("fireball.xml"));
        }

        protected override void Draw(RenderEngine renderEngine)
        {
            renderEngine.Draw(ResourceManager.GetTexture("splash.jpg"), 0, 0, renderEngine.GetScreenWidth(), renderEngine.GetScreenHeight());
        }


        protected override void Update(TimeSpan delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            if (keyboard.GetPressedKeys().Length > 0 && !keyboard.IsKeyDown(Keys.Escape)) {
                start = true;
            }

            if (start && keyboard.GetPressedKeys().Length == 0)
            {
                NextGameState = new MainMenuState();
                start = false;
            }
        }

    }
}