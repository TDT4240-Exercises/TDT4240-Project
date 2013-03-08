using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace X2Game {
	abstract class GameState {
		/** Is this state supposed to be an overlay for other states? */
		public readonly bool isOverlay = false;

		protected GameState(bool overlay)
		{
			isOverlay = overlay;
		}

		/**
		 *	Update the current GameState
		 *
		 *	Returns true if it is finished and ready for a call to getNextState()
		 */
		public abstract bool Update();

		/** Draw the current GameState */
		public abstract void Draw(SpriteBatch spriteBatch);

		/** Send input to the GameState */
		public abstract void Input(KeyboardState keyboard);

		/**
		 * Get new state which this state spawned.
		 * 
		 * Returns null if this state is finished
		 */
		public abstract GameState getNextState();
	}
}