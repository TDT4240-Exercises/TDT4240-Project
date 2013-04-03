using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    enum TileValues
    {
        Texture,
        BlocksMovement,
        BlocksSight,
        Destructible,
        XIndex,
        YIndex
    }

    class TileType : GenericDataStructure
    {
        public readonly static int TILE_WIDTH = 64;
        public readonly static int TILE_HEIGHT = 64;

        private readonly Texture2D _texture;
        private readonly Rectangle _tileIndex;
        public readonly bool BlocksMovement;

        public TileType(string xmlPath) : base(xmlPath, typeof(TileValues))
        {
            SetDefaultValue(TileValues.Texture, ResourceManager.InvalidTexture);

            _tileIndex = new Rectangle(GetValue<int>(TileValues.XIndex) * TILE_WIDTH, GetValue<int>(TileValues.YIndex) * TILE_HEIGHT, TILE_WIDTH, TILE_HEIGHT);
            _texture = GetValue<Texture2D>(TileValues.Texture);
            BlocksMovement = GetValue<bool>(TileValues.BlocksMovement);
        }

        public void Draw(int x, int y, int width, int height, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(x, y, width, height), _tileIndex, Color.White);
        }

    }
}
