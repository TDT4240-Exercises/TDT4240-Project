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
            Button changeScreenSize = new Button("Max Particles: " + ParticleEngine.MaxParticles, 300, 200, 250, 50, Keys.S);
            changeScreenSize.SetOnClickFunction(() => {
                ParticleEngine.MaxParticles += 128;
                if (ParticleEngine.MaxParticles > 2512) ParticleEngine.MaxParticles = 256;
                changeScreenSize.setText("Max Particles: " + ParticleEngine.MaxParticles);
            });
            components.Add(changeScreenSize);

            Button exitButtion = new Button("Exit", 300, 270, 250, 50, Keys.Escape);
            exitButtion.SetOnClickFunction(() => NextGameState = null);
            components.Add(exitButtion);

            Label stateLabel = new Label("==OPTIONS==", 360, 100);
            components.Add(stateLabel);
        }


        protected override void Draw(RenderEngine renderEngine)
        {
        }
    }
}
