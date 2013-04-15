using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class Label : GUIComponent
    {
        public string Text;

        public Label(string text, int x, int y)
        {
            Text = text;
            Bounds.X = x;
            Bounds.Y = y;
        }


        public override void Draw(RenderEngine renderEngine)
        {
            renderEngine.DrawString(Text, Bounds.X, Bounds.Y, Color.White);  
        }

        public override void Update(KeyboardState keyboard, MouseState mouse, GameTime delta)
        {
            //Nothing
        }
    }
}
