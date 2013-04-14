namespace X2Game
{
    class SelectGameModeScreen : GameState
    {
        public SelectGameModeScreen()
        {
            int x = 300;
            int y = 50;

            Label screenLabel = new Label("==SELECT GAME-MODE==", x+20, y);
            components.Add(screenLabel);
            y += 50;

            Button singlePlayerButton = new Button("Single Player", x, y, 250);
            singlePlayerButton.SetOnClickFunction(() => NextGameState = new SelectPlayerState(1, false));
            components.Add(singlePlayerButton);
            y += 100;

            Button multiplayerCoopButton = new Button("Two Player (Co-op)", x, y, 250);
            multiplayerCoopButton.SetOnClickFunction(() => NextGameState = new SelectPlayerState(2, false));
            components.Add(multiplayerCoopButton);
            y += 100;

            Button multiplayerVersusButton = new Button("Two Player (Versus)", x, y, 250);
            multiplayerVersusButton.SetOnClickFunction(() => NextGameState = new SelectPlayerState(2, true));
            components.Add(multiplayerVersusButton);
            y += 100;

            Button backButton = new Button("Back", x, y, 250);
            backButton.SetOnClickFunction(() => NextGameState = null);
            components.Add(backButton);
            y += 100;

        }

        protected override void Draw(RenderEngine renderEngine)
        {
            
        }
    }
}
