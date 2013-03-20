using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class PlayingState : GameState
    {
        private Vector4 _camera;
        private TileMap _tileMap;

        public PlayingState()
        {
            _camera = new Vector4(0, 0, 800, 640);
            _tileMap = new TileMap(100, 100);
        }

        protected override void Update(KeyboardState keyboard, MouseState mouse)
        {
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                NextGameState = null; //Go back to main menu
            }
        }


        protected override void Draw(SpriteBatch spriteBatch)
        {
            _tileMap.Draw(spriteBatch, _camera);
        }
    }
}
