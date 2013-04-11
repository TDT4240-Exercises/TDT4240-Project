using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace X2Game
{
    class SelectPlayerState : GameState
    {
        private readonly Dictionary<Keys, Player.Controllers> _player1Input = new Dictionary<Keys, Player.Controllers>();
        private readonly Dictionary<Keys, Player.Controllers> _player2Input = new Dictionary<Keys, Player.Controllers>();
        private UnitType player1;
        private UnitType player2;
        private Button _getInputKey;
        private readonly List<UnitType> _playableUnits = new List<UnitType>(); 

        public SelectPlayerState()
        {
            //Load all playable tanks
            _playableUnits.AddRange(ResourceManager.GetAllUnitTypes().Where(unit => unit.GetValue<bool>(UnitValues.IsPlayable)));

            //PLAYER 1 Stuff
            int x = 50;
            int y = 50;
            Label player1Label = new Label("=PLAYER 1=", x, y);            
            components.Add(player1Label);
            y += 32;

            components.Add(new Label("Name: ", x, y));
            InputField player1Name = new InputField("Player 1", x+100, y, 150, 32);
            components.Add(player1Name);

            player1 = _playableUnits[0];
            player2 = _playableUnits[0];

            //Load default controls
            _player1Input[Keys.Space] = Player.Controllers.Shoot;
            _player1Input[Keys.Right] = Player.Controllers.Right;
            _player1Input[Keys.Up] = Player.Controllers.Forward;
            _player1Input[Keys.Down] = Player.Controllers.Back;
            _player1Input[Keys.Left] = Player.Controllers.Left;

            foreach (var input in _player1Input)
            {
                y += 32;
                Label inputMap = new Label(input.Value.ToString() + ":", x, y);
                components.Add(inputMap);

                Button keyMap = new Button(input.Key.ToString(), x + 100, y, 150, 32);
                keyMap.SetOnClickFunction(() =>
                    {
                        keyMap.setText("...");
                        _getInputKey = keyMap;
                    });
                components.Add(keyMap);
            }

            //PLAYER 2 Stuff
            x = 350;
            y = 50;
            Label player2Label = new Label("=PLAYER 2=", x, y);
            components.Add(player2Label);

            y += 32;

            components.Add(new Label("Name: ", x, y));
            components.Add(new InputField("Player 2", x + 100, y, 150, 32));

            //Load default controls
            _player2Input[Keys.LeftControl] = Player.Controllers.Shoot;
            _player2Input[Keys.D] = Player.Controllers.Right;
            _player2Input[Keys.W] = Player.Controllers.Forward;
            _player2Input[Keys.S] = Player.Controllers.Back;
            _player2Input[Keys.A] = Player.Controllers.Left;

            foreach (var input in _player2Input)
            {
                y += 32;
                Label inputMap = new Label(input.Value.ToString() + ":", x, y);
                components.Add(inputMap);

                Button keyMap = new Button(input.Key.ToString(), x + 100, y, 150, 32);
                keyMap.SetOnClickFunction(() =>
                {
                    keyMap.setText("...");
                    _getInputKey = keyMap;
                });
                components.Add(keyMap);
            }


            //Buttons to start game or go back to main menu
            Button newGameButton = new Button("Start!", 550, 400);
            newGameButton.SetOnClickFunction(() =>
                {
                    List<Player> playerList = new List<Player>();
                    if (player1 != null) playerList.Add(new Player("Player 1", player1, _player1Input));
                    if (player2 != null) playerList.Add(new Player("Player 2", player2, _player2Input));
					playerList[0].Position = new Vector2(150, 150);

                    NextGameState = new PlayingState(playerList);
                });
            components.Add(newGameButton);

            Button backButton = new Button("Back", 50, 400, 150, 50, Keys.Escape);
            backButton.SetOnClickFunction(() => NextGameState = null);
            components.Add(backButton);
        }

        protected override void Draw(RenderEngine renderEngine)
        {
             //Draw player 1 Tank
            if (player1 != null)
                renderEngine.Draw(player1.Texture, 50, 300, player1.Texture.Width, player1.Texture.Height);

            //Draw player 2 Tank   
            if(player2 != null)
                renderEngine.Draw(player2.Texture, 350, 300, player2.Texture.Width, player2.Texture.Height);
        }

        public override GameState UpdateAll(Microsoft.Xna.Framework.GameTime delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            if (_getInputKey == null)
            {
                return base.UpdateAll(delta, keyboard, mouse, renderEngine);
            }

            foreach (var key in keyboard.GetPressedKeys())
            {
                _getInputKey.setText(key.ToString());
                _getInputKey = null;
                break;
            }

            return this;
        }
    }
}
