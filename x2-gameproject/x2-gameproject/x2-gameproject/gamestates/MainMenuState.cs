using Microsoft.Xna.Framework.Input;

namespace X2Game
{

    class MainMenuState : GameState
    {
        public MainMenuState()
        {
            Button newGameButton = new Button("Start Game", 200, 200, 150, 50, Keys.N);
            newGameButton.SetOnClickFunction(() => NextGameState = new SelectGameModeScreen());

            Button optionsButton = new Button("Options", 200, 270, 150, 50, Keys.O);
            optionsButton.SetOnClickFunction(() => NextGameState = new OptionsState());
           
            Button exitButton = new Button("Exit Game", 200, 340, 150, 50, Keys.Escape);
            exitButton.SetOnClickFunction(() => NextGameState = null);
            
            Button sandboxButton = new Button("Sandbox", 200, 410, 150, 50, Keys.U);
            sandboxButton.SetOnClickFunction(() => NextGameState = new SandboxAndersState());

            Label stateLabel = new Label("==MAIN MENU==", 200, 100);
            
            components.Add(newGameButton);
            components.Add(optionsButton);
            components.Add(exitButton);
            components.Add(sandboxButton);
            components.Add(stateLabel);
        }

        protected override void Draw(RenderEngine renderEngine)
        {
            //nothing
        }

    }
}