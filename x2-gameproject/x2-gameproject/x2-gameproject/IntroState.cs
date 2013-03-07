using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace X2Game {
	class IntroState : GameState {
		private bool buttonPressed = false;

		public IntroState() : base(true) // Set isOverlay to true
		{

		}
		
		public override bool Update()
		{
			return buttonPressed;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (100, 200, 150, 250), Color.White);
			spriteBatch.DrawString (ResourceManager.GetDebugFont (), "AwesomeGame", new Vector2 (100, 100), Color.Black);
		}

		public override void Input(KeyboardState keyboard)
		{
			if (keyboard.GetPressedKeys ().Length > 0 && !keyboard.IsKeyDown(Keys.Escape)) {
				buttonPressed = true;
			}
		}

		public override GameState getNextState ()
		{
			if (buttonPressed) {
				buttonPressed = false;
				return new MainMenuState ();
			}
			return null;
		}
	}
}