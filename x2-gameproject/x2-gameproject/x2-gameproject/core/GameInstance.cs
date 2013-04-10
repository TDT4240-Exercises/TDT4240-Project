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
        private SpriteBatch _spriteBatch;
        private Stack<GameState> _stateStack;
        private RenderEngine _renderEngine;
        private GraphicsDeviceManager _deviceManager;

        private GameState CurrentGameState { get { return _stateStack.Peek(); } }

        public GameInstance()
        {
            _deviceManager = new GraphicsDeviceManager(this);
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
            //Initialization logic here
            ResourceManager.Initialize(GraphicsDevice, _deviceManager,  "Content/");

            //Enable full logging
            Logger.SetLogLevel(LogLevel.Debug);

            _stateStack = new Stack<GameState> ();
            _stateStack.Push (new IntroState ());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _renderEngine = new RenderEngine(_spriteBatch);

            ResourceManager.LoadDebugFont(Content);
            Components.Add(new FPSCounter(this, _spriteBatch));
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
            GameState nextState = CurrentGameState.UpdateAll(gameTime, Keyboard.GetState(), Mouse.GetState(), _renderEngine);

            //Go to previous?
            if (nextState == null)
            {
                _stateStack.Pop();
                if(_stateStack.Count == 0) Exit(); //Last state in stack?
                CurrentGameState.NextGameState = CurrentGameState;
            }

            //New state?
            else if (nextState != CurrentGameState)
            {
                _stateStack.Push(nextState);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Main render loop
            _renderEngine.BeginFrame();
            CurrentGameState.DrawAll(_renderEngine);
            _renderEngine.EndFrame();

            //Finished this frame
            base.Draw(gameTime);
        }
    }
}
