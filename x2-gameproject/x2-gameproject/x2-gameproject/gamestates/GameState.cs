using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

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
        public GameState UpdateAll(GameTime delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            //Update GUI components
            foreach (GUIComponent guiComponent in components)
            {
                guiComponent.Update(keyboard, mouse);
            }

            Update(delta, keyboard, mouse, renderEngine);

            return NextGameState;
        }

        protected virtual void Update(GameTime delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            //Default no implementation
        }

        /** Draw the current GameState and all components */
        public void DrawAll(RenderEngine renderEngine)
        {
            //Draw the GameState itself
            Draw(renderEngine);

            //Draw components last
            foreach (GUIComponent guiComponent in components) guiComponent.Draw(renderEngine);
        }

        /** Draw the current GameState */
        protected abstract void Draw(RenderEngine renderEngine);
    }
}