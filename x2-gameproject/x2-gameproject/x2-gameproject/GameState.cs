using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace X2Game {
	abstract class GameState {
		public readonly bool isOverlay = false;

		protected GameState(bool overlay)
		{
			isOverlay = overlay;
		}

		public abstract bool Update();
		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void Input(KeyboardState keyboard);
		public abstract GameState getNextState();
	}
}