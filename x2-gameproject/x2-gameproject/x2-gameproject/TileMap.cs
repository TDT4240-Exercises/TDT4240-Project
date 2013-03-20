using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    class TileMap
    {
        private readonly TileType[,] _tileMap;
        private readonly UInt16 _width;
        private readonly UInt16 _height;

        public TileMap(UInt16 width, UInt16 height)
        {
            _width = width;
            _height = height;
            _tileMap = new TileType[width, height];

            //Fill tilemap with grass
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    _tileMap[x, y] = ResourceManager.GetTile("grass.xml");
                }
            }
        }

        /// <summary>
        /// Gets the Tile from the specified real world coordinate position
        /// </summary>
        /// <param name="x">float x position</param>
        /// <param name="y">float y position</param>
        /// <returns>The Tile object at the specified positon</returns>
        public TileType GetTile(float x, float y)
        {
            int xIndex = (int)(x / TileType.TILE_WIDTH);
            int yIndex = (int)(y / TileType.TILE_HEIGHT);
            return _tileMap[xIndex, yIndex];
        }

        public void Draw(SpriteBatch spriteBatch, Vector4 camera)
        {
            //Figure out the visible bounds of our map
            int sttX = (int)Math.Max(0, camera.X / TileType.TILE_WIDTH);
            int sttY = (int)Math.Max(0, camera.Y / TileType.TILE_HEIGHT);
            int endX = (int)Math.Min(_width, camera.Z / TileType.TILE_WIDTH);
            int endY = (int)Math.Max(_height, camera.W / TileType.TILE_HEIGHT);

            //Scale each tile to our prefered resolution
            int width = (int)(camera.Z/(endX - sttX));
            int height = (int)(camera.Z / (endX - sttX));
            
            //Draw the entire visible map
            for (int x = sttX; x < endX; ++x)
            {
                for (int y = sttY; y < endY; ++y)
                {
                    _tileMap[x, y].Draw(x*width, y*height, width, height, spriteBatch);
                }
            }
        }
    }
}
