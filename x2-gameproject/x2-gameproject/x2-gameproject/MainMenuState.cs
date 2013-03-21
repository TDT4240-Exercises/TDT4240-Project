using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game {

    class MainMenuState : GameState {
        private bool spawnParticle;
        private Random rand;
        
        public MainMenuState()
        {
            rand = new Random ();

            Button newGameButton = new Button("New Game", 200, 200, 150, 50, Keys.N);
            newGameButton.SetOnClickFunction(() => NextGameState = new PlayingState());
            components.Add(newGameButton);

            components.Add(new Button("Options", 200, 270));

            Button exitButton = new Button("Exit Game", 200, 340, 150, 50, Keys.Escape);
            exitButton.SetOnClickFunction(() => NextGameState = null);
            components.Add(exitButton);

            Button sandboxButton = new Button ("Sandbox", 200, 410, 150, 50, Keys.U);
            sandboxButton.SetOnClickFunction (() => {
                NextGameState = new SandboxAndersState();
            });
            components.Add (sandboxButton);
        }

        protected override void Update(TimeSpan delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            if (keyboard.IsKeyDown(Keys.P) && !spawnParticle)
            {
                ParticleEngine.SpawnParticle(new Vector2(rand.Next(renderEngine.GetScreenWidth()), rand.Next(renderEngine.GetScreenHeight())), ResourceManager.GetParticleTemplate("fireball.xml"));
                spawnParticle = true;
            }
            else if (keyboard.IsKeyUp(Keys.P) && spawnParticle)
                spawnParticle = false;
            else if (keyboard.IsKeyDown(Keys.S))
                ParticleEngine.SpawnParticle(new Vector2(rand.Next(renderEngine.GetScreenWidth()), rand.Next(renderEngine.GetScreenHeight())), ResourceManager.GetParticleTemplate("fireball.xml"));
            if (keyboard.IsKeyDown(Keys.L))
                ParticleEngine.SpawnParticle(new Vector2(rand.Next(renderEngine.GetScreenWidth()), rand.Next(renderEngine.GetScreenHeight())), ResourceManager.GetParticleTemplate("blueEnergyBall.xml"));
        }

        protected override void Draw(RenderEngine renderEngine)
        {
            renderEngine.DrawString("Main menu", 100, 100, Color.White);
            renderEngine.DrawString("Particles: " + ParticleEngine.Count(), 10, 10, Color.White);
        }
        
    }
}