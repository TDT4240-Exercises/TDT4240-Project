using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game {
	class IntroState : GameState {
		private bool buttonPressed;

		public IntroState() : base(false) // Set isOverlay to true
		{
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
                ParticleEngine.SpawnParticle(new Vector2(rand.Next(800), rand.Next(640)), ResourceManager.getParticleTemplate("fireball.xml"));
		}
		
		protected override bool Update()
		{
			return buttonPressed;
		}

        protected override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ResourceManager.getTexture("splash.jpg"), new Rectangle(0, 0, 800, 640), null, Color.White);
		}

		public override void Input(KeyboardState keyboard)
		{
			if (keyboard.GetPressedKeys ().Length > 0 && !keyboard.IsKeyDown(Keys.Escape)) {
				buttonPressed = true;
			}
		}

		public override GameState getNextState ()
		{
			if (buttonPressed) {
				buttonPressed = false;
				return new MainMenuState ();
			}
			return null;
		}
	}
}