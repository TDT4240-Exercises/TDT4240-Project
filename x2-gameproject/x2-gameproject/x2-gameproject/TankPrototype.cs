using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game {
	class TankPrototype : Entity {
		public enum Controllers {
			Forward,
			Back,
			Left,
			Right
		}

		private Keys[] controls;
		private bool[] controlStates;

		private float turnRate;

		public TankPrototype() : base("fireball.png", EntityController.Player)
		{
			controls 		= new Keys[Enum.GetNames(typeof(Controllers)).Length]; // Size = number of items in Controllers enum
			controlStates 	= new bool[Enum.GetNames(typeof(Controllers)).Length]; // Size = number of items in Controllers enum
			turnRate 		= 0.05f;
			position 		= new Vector2 (100, 100);
		}

		public override void Update (TimeSpan delta)
		{
			// Rotate according to keys and turnRate
			if (controlStates [(int)Controllers.Left])
				rotation -= turnRate;
			else if (controlStates [(int)Controllers.Right])
				rotation += turnRate;

			// Direction vector times (Forward, Back, Neither):(1, -1, 0) with inline if
			velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 
				(controlStates [(int)Controllers.Forward] ? 1.0f : (controlStates [(int)Controllers.Back] ? -1.0f : 0.0f));

			base.Update (delta);
		}

		public override void Render (SpriteBatch spriteBatch)
		{
			base.Render (spriteBatch);
		}

		public void Input(KeyboardState keyboard)
		{
			// Iterate through all keys in enum and set statuses
			// PS: Does not need to be modified to add keys
			for (int i = 0; i < Enum.GetNames(typeof(Controllers)).Length; ++i)
				controlStates [i] = keyboard.IsKeyDown (controls [i]);
		}

		/// <summary>
		/// Sets the controllers. (deprecated)
		/// </summary>
		/// <param name="forward">Forward.</param>
		/// <param name="back">Back.</param>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		public void SetControllers(Keys forward, Keys back, Keys left, Keys right)
		{
			// Set all controllers (modify method to add more controllers, or call SetController)
			controls [(int)Controllers.Forward] = forward;
			controls [(int)Controllers.Back] 	= back;
			controls [(int)Controllers.Left] 	= left;
			controls [(int)Controllers.Right] 	= right;
		}

		/// <summary>
		/// Sets individual controllers
		/// </summary>
		/// <param name="controller">The controller to set</param>
		/// <param name="key">The key to associate with the goven control</param>
		public void SetController(Controllers controller, Keys key){
			controls [(int)controller] = key;
		}
	}
}