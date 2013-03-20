using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameInstance : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Stack<GameState> stateStack;
        private Texture2D _mouseCursor;

        private GameState CurrentGameState { get { return stateStack.Peek(); } }

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
            ResourceManager.Initialize(GraphicsDevice, "Content/");

			stateStack = new Stack<GameState> ();
			stateStack.Push (new IntroState ());

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

            ResourceManager.LoadDebugFont(Content);
            Components.Add(new FPSCounter(this, spriteBatch));
            _mouseCursor = ResourceManager.GetTexture("cursor.png");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) Exit();

            //Update the current game state and all it's sub-components
            GameState nextState = CurrentGameState.UpdateAll(Keyboard.GetState(), Mouse.GetState());

            //Go to previous?
            if (nextState == null)
            {
                stateStack.Pop();
                if(stateStack.Count == 0) Exit(); //Last state in stack?
                CurrentGameState.NextGameState = CurrentGameState;
            }

            //New state?
            else if (nextState != CurrentGameState)
            {
                stateStack.Push(nextState);
            }

            ParticleEngine.Update(gameTime.ElapsedGameTime);

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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			
            stateStack.Peek().DrawAll(spriteBatch);
            ParticleEngine.Render(spriteBatch);

            //Draw makeshift mouse pointer
            spriteBatch.Draw(_mouseCursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32), Color.White);

            spriteBatch.End();

            //Finished this frame
            base.Draw(gameTime);
        }
    }
}
