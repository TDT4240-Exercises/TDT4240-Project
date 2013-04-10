using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class Label : GUIComponent
    {
        private string _text;

        public Label(string text, int x, int y)
        {
            _text = text;
            Bounds.X = x;
            Bounds.Y = y;
        }

        public override void Draw(RenderEngine renderEngine)
        {
            renderEngine.DrawString(_text, Bounds.X, Bounds.Y, Color.White);  
        }

        public override void Update(KeyboardState keyboard, MouseState mouse)
        {

        }
    }
}
