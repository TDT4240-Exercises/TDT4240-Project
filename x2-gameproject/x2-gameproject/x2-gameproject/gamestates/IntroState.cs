using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class IntroState : GameState
    {
        protected override void Draw(RenderEngine renderEngine)
        {
            renderEngine.Draw(ResourceManager.GetTexture("splash.jpg"), 0, 0, renderEngine.GetScreenWidth(), renderEngine.GetScreenHeight());
        }

        protected override void Update(GameTime delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            if ( keyboard.GetPressedKeys().Length != 0)
            {
                if (keyboard.IsKeyDown(Keys.Escape)) NextGameState = null;
                else                                 NextGameState = new MainMenuState();
            }
        }

    }
}