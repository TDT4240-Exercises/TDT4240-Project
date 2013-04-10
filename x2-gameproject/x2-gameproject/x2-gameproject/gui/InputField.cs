
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class InputField : GUIComponent
    {
        public string Text;

        private HashSet<Keys> keyDown = new HashSet<Keys>();
        private bool isTyping;
        private double delay;
        private char token = '_';
        private long tokenShift;

        public InputField(string text, int x, int y, int width, int height)
        {
            Text = text;
            Bounds.X = x;
            Bounds.Y = y;
            Bounds.Width = width;
            Bounds.Height = height;
        }

        public override void Draw(RenderEngine renderEngine)
        {
            renderEngine.DrawFilledRectangle(Bounds, Color.Coral);
            renderEngine.DrawString(Text + (isTyping ? token : ' '), Bounds.X, Bounds.Y, Color.White);   
        }

        public override void Update(KeyboardState keyboard, MouseState mouse, GameTime delta)
        {
            //Make the cursor blinking
            if (delta.TotalGameTime.Ticks > tokenShift)
            {
                tokenShift = delta.TotalGameTime.Ticks + TimeSpan.TicksPerSecond/2;
                token = token == ' ' ? '_' : ' ';
            }

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                isTyping = Bounds.Contains(mouse.X, mouse.Y);
            }

            if (keyboard.IsKeyDown(Keys.Enter)) isTyping = false;

            if (keyboard.GetPressedKeys().Count() == 0) keyDown.Clear();

            //Not typing in this box?
            if (!isTyping || delta.TotalGameTime.TotalSeconds < delay) return;
            

            //Backspace to erase
            if (keyboard.IsKeyDown(Keys.Back) && Text.Length > 0)
            {
                Text = Text.Substring(0, Text.Length - 1);
                delay = delta.TotalGameTime.TotalSeconds + 0.1;
                return;
            }

            //Limit text with to input box width
            if (ResourceManager.GetDebugFont().MeasureString(Text).X + 16 > Bounds.Width) return;

            //Empty space
            if (keyboard.IsKeyDown(Keys.Space))
            {
                Text += " ";
                delay = delta.TotalGameTime.TotalSeconds + 0.2;
                return;
            }

            //Check if SHIFT key is held down
            bool upperCase = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            
            //Loop through all pressed buttons
            foreach (var key in keyboard.GetPressedKeys().Where(key => key.ToString().Length == 1 && !keyDown.Contains(key)))
            {
                Text += upperCase ? key.ToString() : key.ToString().ToLower();
                keyDown.Add(key);
                break;
            }
        }
    }
}
