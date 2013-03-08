using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using x2_gameproject;

namespace X2Game {
	abstract class GameState {
		/** Is this state supposed to be an overlay for other states? */
		public readonly bool isOverlay = false;
	    protected readonly List<GUIComponent> components;

		protected GameState(bool overlay)
		{
			isOverlay = overlay;
            components = new List<GUIComponent>();
		}

        /// <summary>
        /// Updates all components in this GameState
        /// </summary>
        /// <returns>Returns true if it is finished and ready for a call to getNextState()</returns>
		public virtual bool UpdateAll()
		{
		    foreach (GUIComponent guiComponent in components)
		    {
                guiComponent.Update();
		    }

		    return Update();
		}

        /**
         *	Update the current GameState
         *
         *	Returns true if it is finished and ready for a call to getNextState()
         */
	    protected abstract bool Update();

		/** Draw the current GameState and all components */
		public void DrawAll(SpriteBatch spriteBatch)
		{
            //Draw the GameState itself
		    Draw(spriteBatch);

            //Draw components last
            foreach (GUIComponent guiComponent in components) guiComponent.Draw(spriteBatch);
		}

        /** Draw the current GameState */
        protected abstract void Draw(SpriteBatch spriteBatch);

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