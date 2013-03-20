using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game {
	class SandboxAndersState : GameState {
		private TankPrototype minTank;
		
		public SandboxAndersState()
		{
			minTank = new TankPrototype();
			minTank.SetController (TankPrototype.Controllers.Forward, 	Keys.W);
			minTank.SetController (TankPrototype.Controllers.Back, 		Keys.S);
			minTank.SetController (TankPrototype.Controllers.Left, 		Keys.A);
			minTank.SetController (TankPrototype.Controllers.Right, 	Keys.D);
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
		
		protected override void Update(KeyboardState keyboard, MouseState mouse)
		{
			if (keyboard.IsKeyDown(Keys.Escape)) {
				NextGameState = null; //Go back to main menu
			}
			minTank.Input(keyboard);
            minTank.Update(new TimeSpan());
        }
		
	}
}