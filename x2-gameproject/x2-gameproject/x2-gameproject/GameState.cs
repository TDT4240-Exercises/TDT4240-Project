using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game {
    abstract class GameState {
        protected readonly List<GUIComponent> components;
        public GameState NextGameState;

        protected GameState()
        {
            components = new List<GUIComponent>();
            NextGameState = this;
        }

        /// <summary>
        /// Updates all components in this GameState and figures out what should be the next GameState
        /// </summary>
        /// <returns>Returns what should be the current GameState. If null is returned then it will go back to the previous state</returns>
        public GameState UpdateAll(KeyboardState keyboard, MouseState mouse)
        {
            //Update GUI components
            foreach (GUIComponent guiComponent in components)
            {
                guiComponent.Update(keyboard, mouse);
            }

            Update(keyboard, mouse);

            return NextGameState;
        }

        protected virtual void Update(KeyboardState keyboard, MouseState mouse)
        {
            //Default no implementation
        }

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
    }
}