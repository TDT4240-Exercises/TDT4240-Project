using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace X2Game
{

    class SelectPlayerState : GameState
    {
        private readonly List<UnitType> _playableUnits = new List<UnitType>(); 
        private readonly Dictionary<Keys, Player.Controllers> _playerInput = new Dictionary<Keys, Player.Controllers>();
        private UnitType _unitType;
        private Button _getInputKey;

        public SelectPlayerState(int numberOfplayers, bool versus, List<Player> playerList = null)
        {
            //First player?
            if (playerList == null) playerList = new List<Player>();

            //Load all playable tanks
            _playableUnits.AddRange(ResourceManager.GetAllUnitTypes().Where(unit => unit.GetValue<bool>(UnitValues.IsPlayable)));
            _unitType = _playableUnits[0];

            int x = 50;
            int y = 50;
            Label player1Label = new Label("=PLAYER " + (playerList.Count + 1) + "=", x, y);            
            components.Add(player1Label);

            x = 400;
            y += 32;

            components.Add(new Label("Name: ", x, y));

            //Player name
            InputField player1Name = new InputField("Player " + (playerList.Count + 1), x + 100, y, 150, 32);
            components.Add(player1Name);
            y += 100;

            //Unit Type
            Label unitType = new Label("Unit Type", x, y);
            components.Add(unitType);
            y += 150;
            x += 50;

            Button previousType = new Button("<-", x, y, 32, 32);
            previousType.SetOnClickFunction(() =>
                {
                    int index = _playableUnits.IndexOf(_unitType) - 1;
                    if (index < 0) index = _playableUnits.Count-1;
                    _unitType = _playableUnits[index];
                });
            components.Add(previousType);

            Button nextType = new Button("->", x + 50, y, 32, 32);
            nextType.SetOnClickFunction(() =>
            {
                int index = _playableUnits.IndexOf(_unitType) + 1;
                if (index >= _playableUnits.Count) index = 0;
                _unitType = _playableUnits[index];
            });
            components.Add(nextType);

            //Load default controls
            x = 50;
            y = 82;
            _playerInput[Keys.RightShift] = Player.Controllers.Secondary;
            _playerInput[Keys.RightControl] = Player.Controllers.Primary;
            _playerInput[Keys.Right] = Player.Controllers.Right;
            _playerInput[Keys.Up] = Player.Controllers.Forward;
            _playerInput[Keys.Down] = Player.Controllers.Back;
            _playerInput[Keys.Left] = Player.Controllers.Left;

            foreach (var input in _playerInput)
            {
                y += 32;
                Label inputMap = new Label(input.Value.ToString() + ":", x, y);
                components.Add(inputMap);

                Button keyMap = new Button(input.Key.ToString(), x + 150, y, 150, 32);
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
                    playerList.Add(new Player(player1Name.Text, _unitType, _playerInput));

                    if (playerList.Count < numberOfplayers)
                    {
                        NextGameState = new SelectPlayerState(numberOfplayers, versus, playerList);
                    }
                    else
                    {
                        NextGameState = new PlayingState(playerList);
                        DestroyCurrentState();
                    }

                });
            components.Add(newGameButton);

            Button backButton = new Button("Back", 50, 400, 150, 50, Keys.Escape);
            backButton.SetOnClickFunction(() =>
                {
                    NextGameState = null;
                });
            components.Add(backButton);
        }

        protected override void Draw(RenderEngine renderEngine)
        {
             //Draw player Tank
            if (_unitType != null)
                renderEngine.Draw(_unitType.Texture, 450, 250, _unitType.Texture.Width, _unitType.Texture.Height);
        }

        public override GameState UpdateAll(GameTime delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
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
