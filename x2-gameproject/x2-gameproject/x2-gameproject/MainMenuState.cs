using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using X2Game;

namespace X2Game {

	class MainMenuState : GameState {
		private bool buttonPressed;
		private bool spawnParticle;
		private Random rand;
		
		public MainMenuState() : base(true) // Set isOverlay to true
		{
			rand = new Random ();

            components.Add(new Button("New Game", 200, 200));
            components.Add(new Button("Options", 200, 270));

		    Button exitButton = new Button("Exit Game", 200, 340);
            exitButton.SetOnClickFunction(() => Environment.Exit(0));
            components.Add(exitButton);

            components.Add(new Button("Secret Button", 200, 410));
		}
		
		protected override bool Update()
		{
			return buttonPressed;
		}

        protected override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString (ResourceManager.GetDebugFont (), "Main menu", new Vector2 (100, 100), Color.White);
			spriteBatch.DrawString (ResourceManager.GetDebugFont (), "Particles: " + ParticleEngine.Count (), new Vector2 (10, 10), Color.White);
		}
		
		public override void Input(KeyboardState keyboard)
		{
			if (keyboard.IsKeyDown (Keys.Escape)) {
				buttonPressed = true;
			} else if (keyboard.IsKeyDown (Keys.P) && !spawnParticle) {
				ParticleEngine.SpawnParticle (new Vector2 (rand.Next (800), rand.Next (600)), ResourceManager.getParticleTemplate ("fireball.xml"));
				spawnParticle = true;
			} else if (keyboard.IsKeyUp (Keys.P) && spawnParticle)
				spawnParticle = false;
			else if (keyboard.IsKeyDown (Keys.S))
				ParticleEngine.SpawnParticle (new Vector2 (rand.Next (800), rand.Next (600)), ResourceManager.getParticleTemplate ("fireball.xml"));
			if (keyboard.IsKeyDown(Keys.L))
				ParticleEngine.SpawnParticle (new Vector2 (rand.Next (800), rand.Next (600)), ResourceManager.getParticleTemplate ("blueEnergyBall.xml"));
		}
		
		public override GameState getNextState ()
		{
			return null;
		}
	}
}