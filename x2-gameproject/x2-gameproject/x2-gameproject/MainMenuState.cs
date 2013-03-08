using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace X2Game {
	class MainMenuState : GameState {
		private bool buttonPressed = false;
		private bool spawnParticle = false;
		private Random rand;
		
		public MainMenuState() : base(true) // Set isOverlay to true
		{
			rand = new Random ();
		}
		
		public override bool Update()
		{
			return buttonPressed;
		}
		
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (200, 200, 150, 50), Color.White);
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (200, 270, 150, 50), Color.White);
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (200, 340, 150, 50), Color.White);
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (200, 410, 150, 50), Color.White);
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
			else if (keyboard.IsKeyDown(Keys.L))
				ParticleEngine.SpawnParticle (new Vector2 (rand.Next (800), rand.Next (600)), ResourceManager.getParticleTemplate ("blueEnergyBall.xml"));
		}
		
		public override GameState getNextState ()
		{
			return null;
		}
	}
}