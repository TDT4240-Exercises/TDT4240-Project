using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class IntroState : GameState
    {
        Color opacity = new Color(255, 255, 255);

        protected override void Draw(RenderEngine renderEngine)
        {
            renderEngine.Draw(ResourceManager.GetTexture("splash.jpg"), 0, 0, renderEngine.GetScreenWidth(), renderEngine.GetScreenHeight(), opacity);
        }

        protected override void Update(GameTime delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            if (keyboard.GetPressedKeys().Length != 0)
            {
                if (keyboard.IsKeyDown(Keys.Escape)) NextGameState = null;
                else Fade();
            }
            if (opacity.A != 255) Fade();

        }

        private void Fade()
        {
            opacity.A -= 15;
            if (opacity.A == 0)
            {
                opacity.A = 255;
                NextGameState = new MainMenuState();
            }
        }

    }
}