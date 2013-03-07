using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace X2Game {
	class IntroState : GameState {

		public IntroState() : base(true)
		{

		}
		
		public override void Update()
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (ResourceManager.GetDebugTexture (), new Rectangle (100, 200, 150, 250), Color.White);
		}

		public override void Input(KeyboardState keyboard)
		{

		}
	}
}