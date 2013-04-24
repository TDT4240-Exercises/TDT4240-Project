using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    public enum Difficulity
    {
        SISSY,
        REGULAR,
        IRONMAN
    }

    class OptionsState : GameState
    {
        private static Difficulity difficulity = Difficulity.REGULAR;

        public static float GetDifficulity()
        {
            switch(difficulity)
            {
                case Difficulity.SISSY:
                    return 2;
                case Difficulity.REGULAR:
                    return 1.5f;
                case Difficulity.IRONMAN:
                    return 1;
            }

            return 1;
        }

        public OptionsState()
        {
            Button changeScreenSize = new Button("Max Particles: " + ParticleEngine.MaxParticles, 300, 200, 250, 50, Keys.S);
            changeScreenSize.SetOnClickFunction(() => {
                ParticleEngine.MaxParticles += 128;
                if (ParticleEngine.MaxParticles > 2512) ParticleEngine.MaxParticles = 256;
                changeScreenSize.setText("Max Particles: " + ParticleEngine.MaxParticles);
            });
            components.Add(changeScreenSize);

            Button difficulityButton = new Button("Difficulity: " + difficulity.ToString(), 300, 270, 250, 50, Keys.S);
            difficulityButton.SetOnClickFunction(() =>
            {
                switch (difficulity)
                {
                    case Difficulity.IRONMAN: difficulity = Difficulity.SISSY; break;
                    case Difficulity.SISSY:   difficulity = Difficulity.REGULAR; break;
                    case Difficulity.REGULAR: difficulity = Difficulity.IRONMAN; break;
                }
                difficulityButton.setText("Difficulity: " + difficulity.ToString());
            });
            components.Add(difficulityButton);

            Button exitButtion = new Button("Exit", 300, 340, 250, 50, Keys.Escape);
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
