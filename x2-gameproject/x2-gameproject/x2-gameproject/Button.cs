using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class Button : GUIComponent
    {
        public string Text = null;
        public Color TextColor = Color.GhostWhite;
        public Color ButtonColor = Color.Brown;
        private bool _mouseIsOver;

        public Button(string buttonText, int x, int y, int width = 150, int height = 50)
        {
            Text = buttonText;
            Bounds = new Rectangle(x, y, width, height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(ResourceManager.GetDebugTexture(), Bounds, _mouseIsOver ? ButtonColor * 1.5f : ButtonColor);
            if (!string.IsNullOrWhiteSpace(Text))
            {
                SpriteFont font = ResourceManager.GetDebugFont();
                Vector2 textMeasure = font.MeasureString(Text) / 2;
                spriteBatch.DrawString(font, Text, new Vector2(Bounds.Center.X - textMeasure.X, Bounds.Center.Y - textMeasure.Y), TextColor);
            }
        }

        public override void Update(KeyboardState keyboard, MouseState mouse)
        {
            _mouseIsOver = Bounds.Contains(mouse.X, mouse.Y);
        }
    }
}
