using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game {
	class SandboxAndersState : GameState {
		private bool exitState = false;

		private TankPrototype minTank;
		
		public SandboxAndersState() : base(false) // Set isOverlay to false
		{
			minTank = new TankPrototype();
			minTank.SetController (TankPrototype.Controllers.Forward, 	Keys.W);
			minTank.SetController (TankPrototype.Controllers.Back, 		Keys.S);
			minTank.SetController (TankPrototype.Controllers.Left, 		Keys.A);
			minTank.SetController (TankPrototype.Controllers.Right, 	Keys.D);
		}
		
		protected override bool Update()
		{
			minTank.Update (new TimeSpan ());
			return exitState;
		}
		
		protected override void Draw(SpriteBatch spriteBatch)
		{
			minTank.Render (spriteBatch);
			ParticleEngine.Render (spriteBatch);
			spriteBatch.DrawString (
				ResourceManager.GetDebugFont (), 
				"Position: " + minTank.GetX () + ", " + minTank.GetY (), 
				new Vector2 (200, 200), 
				Color.White
			);
		}
		
		public override void Input(KeyboardState keyboard)
		{
			if (keyboard.IsKeyDown(Keys.Escape)) {
				exitState = true;
			}
			minTank.Input (keyboard);
		}
		
		public override GameState getNextState ()
		{
			if (exitState) {
				exitState = false;
			}
			return null;
		}
	}
}