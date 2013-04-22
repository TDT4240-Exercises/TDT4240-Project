using Microsoft.Xna.Framework.Input;

namespace X2Game
{

    class MainMenuState : GameState
    {
        public MainMenuState()
        {
            Button newGameButton = new Button("Start Game", 300, 200, 150, 50, Keys.N);
            newGameButton.SetOnClickFunction(() => NextGameState = new SelectGameModeScreen());

            Button optionsButton = new Button("Options", 300, 270, 150, 50, Keys.O);
            optionsButton.SetOnClickFunction(() => NextGameState = new OptionsState());
           
            Button exitButton = new Button("Exit Game", 300, 340, 150, 50, Keys.Escape);
            exitButton.SetOnClickFunction(() => NextGameState = null);
            
            Label stateLabel = new Label("==MAIN MENU==", 300, 100);
            
            components.Add(newGameButton);
            components.Add(optionsButton);
            components.Add(exitButton);
            components.Add(stateLabel);
        }

        protected override void Draw(RenderEngine renderEngine)
        {
            renderEngine.Draw(ResourceManager.GetTexture("splash.jpg"), 0, 0, renderEngine.GetScreenWidth(), renderEngine.GetScreenHeight());
        }

    }
}