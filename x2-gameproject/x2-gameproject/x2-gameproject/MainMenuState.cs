using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace X2Game {
	class MainMenuState : GameState {
		private bool buttonPressed = false;
		
		public MainMenuState() : base(true) // Set isOverlay to true
		{
			
		}
		
		public override bool Update()
		{
			return buttonPressed;
		}
		
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (200, 200, 150, 50), Color.White);
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (200, 270, 150, 50), Color.White);
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (200, 340, 150, 50), Color.White);
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (200, 410, 150, 50), Color.White);
			spriteBatch.DrawString (ResourceManager.GetDebugFont (), "Main menu", new Vector2 (100, 100), Color.Black);
		}
		
		public override void Input(KeyboardState keyboard)
		{
			if (keyboard.IsKeyDown (Keys.Escape)) {
				buttonPressed = true;
			}
		}
		
		public override GameState getNextState ()
		{
			return null;
		}
	}
}