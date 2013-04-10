using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class OptionsState : GameState
    {
        public OptionsState()
        {
            Button changeScreenSize = new Button("Change screen res", 200, 200, 250, 50, Keys.S);
            changeScreenSize.SetOnClickFunction(() => ResourceManager.SetScreenSize());
            components.Add(changeScreenSize);

            Button exitButtion = new Button("Exit", 200, 270, 250, 50, Keys.Escape);
            exitButtion.SetOnClickFunction(() => NextGameState = null);
            components.Add(exitButtion);
        }


        protected override void Draw(RenderEngine renderEngine)
        {
            renderEngine.DrawString("Options", 100, 100, Color.White);
            renderEngine.DrawString("Particles: " + ParticleEngine.Count(), 10, 10, Color.White);
        }
    }
}
