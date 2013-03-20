using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class Button : GUIComponent
    {
        public delegate void OnButtonClick();

        public string Text = null;
        public Color TextColor = Color.GhostWhite;
        public Color ButtonColor = Color.Brown;
        private bool _mouseIsOver, _hotkeyDown;
        private OnButtonClick _onClickFunction;
        private Keys _hotKey;

        public Button(string buttonText, int x, int y, int width = 150, int height = 50, Keys hotKey = Keys.None)
        {
            Text = buttonText;
            Bounds = new Rectangle(x, y, width, height);
            _hotKey = hotKey;
        }

        public void SetOnClickFunction(OnButtonClick onClick)
        {
            _onClickFunction = onClick;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(ResourceManager.InvalidTexture, Bounds, _mouseIsOver ? ButtonColor * 1.5f : ButtonColor);
            if (!string.IsNullOrWhiteSpace(Text))
            {
                SpriteFont font = ResourceManager.GetDebugFont();
                Vector2 textMeasure = font.MeasureString(Text) / 2;
                spriteBatch.DrawString(font, Text, new Vector2(Bounds.Center.X - textMeasure.X, Bounds.Center.Y - textMeasure.Y), TextColor);
            }
        }

        public override void Update(KeyboardState keyboard, MouseState mouse)
        {
            //Mouse
            _mouseIsOver = Bounds.Contains(mouse.X, mouse.Y);

            //Hotkey down?
            if(!_hotkeyDown && _hotKey != Keys.None) _hotkeyDown = keyboard.IsKeyDown(_hotKey);

            //Can it be clicked?
            if (_onClickFunction == null) return;
            
            //Mouse click?
            if (_mouseIsOver && mouse.LeftButton == ButtonState.Pressed) _onClickFunction();
            
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
    }
}
