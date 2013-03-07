using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace X2Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameInstance : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		Stack<GameState> stateStack;

        public GameInstance()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ResourceManager.initialize(GraphicsDevice, "");

			stateStack = new Stack<GameState> ();
			stateStack.Push (new IntroState ());

            //PARTICLE ENGINE TEST
            ParticleTemplate test = new ParticleTemplate("blueEnergyBall.xml");
            test.writeToFile("test.xml");
            ParticleEngine.spawnParticle(new Vector2(100, 100), test);
            //PARTICLE ENGINE TEST END

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			ResourceManager.LoadDebugFont (Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

			stateStack.Peek ().Input (Keyboard.GetState ());

			if (stateStack.Peek ().Update ()) {
				// TODO: get next state from this state and push it
				// 		 if it is null, then pop current
				GameState newState = stateStack.Peek().getNextState();

				if (newState == null){
					if (stateStack.Count == 1) this.Exit();
					stateStack.Pop();
				}
				else stateStack.Push(newState);
			}

            // TODO: Add your update logic here
            ParticleEngine.update(gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Start new frame
            GraphicsDevice.Clear(Color.Black);

            //Render current state
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			if (stateStack.Peek().isOverlay) ParticleEngine.render(spriteBatch);
            stateStack.Peek().Draw(spriteBatch);
            if (!stateStack.Peek().isOverlay) ParticleEngine.render(spriteBatch);
			spriteBatch.End();

            //Finished this frame
            base.Draw(gameTime);
        }
    }
}
