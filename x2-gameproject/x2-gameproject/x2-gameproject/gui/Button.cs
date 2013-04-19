using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class Button : GUIComponent
    {
        public delegate void OnButtonClick();

        public string Text = null;
        public Color TextColor = Color.WhiteSmoke;
        public Color ButtonColor = Color.DarkRed;
        private bool _mouseIsOver, _hotkeyDown, _mouseIsDown;
        private OnButtonClick _onClickFunction;
        private Keys _hotKey;
        private Texture2D btnTexture;

        public Button(string buttonText, int x, int y, int width = 150, int height = 50, Keys hotKey = Keys.None)
        {
            Text = buttonText;
            Bounds = new Rectangle(x, y, width, height);
            _hotKey = hotKey;
            btnTexture = ResourceManager.GetTexture("button.png");
        }

      
        public void SetOnClickFunction(OnButtonClick onClick)
        {
            _onClickFunction = onClick;
        }

        public override void Draw(RenderEngine renderEngine)
        {
            renderEngine.Draw(btnTexture, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, _mouseIsOver ? ButtonColor * 1.5f : ButtonColor);
            if (!string.IsNullOrWhiteSpace(Text))
            {
                renderEngine.DrawString(Text, Bounds.Center.X, Bounds.Center.Y, TextColor, true);
            }
        }

        public override void Update(KeyboardState keyboard, MouseState mouse, GameTime delta)
        {
            //Mouse
            _mouseIsOver = Bounds.Contains(mouse.X, mouse.Y);

            //Hotkey down?
            if(!_hotkeyDown && _hotKey != Keys.None) _hotkeyDown = keyboard.IsKeyDown(_hotKey);

            //Can it be clicked?
            if (_onClickFunction == null) return;
            
            //Mouse click?
            if (_mouseIsOver)
            {
                if (mouse.LeftButton == ButtonState.Pressed) _mouseIsDown = true;
                else if (_mouseIsDown && mouse.LeftButton == ButtonState.Released)
                {
                    _mouseIsDown = false;
                    _onClickFunction();
                }
            }
            else if (!_mouseIsOver)
            {
                _mouseIsDown = false;
            }
            
            //Trigger hotkey?
            else if (_hotkeyDown && keyboard.IsKeyUp(_hotKey))
            {
                _hotkeyDown = false;
                _onClickFunction();
            }

            //Key released?
            else if (keyboard.IsKeyUp(_hotKey))
            {
                _hotkeyDown = false;
            }
        }

        public void setText(string text)
        {
            Text = text;
        }
    }
}
