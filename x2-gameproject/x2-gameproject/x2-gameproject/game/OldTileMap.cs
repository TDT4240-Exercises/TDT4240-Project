using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    class OldTileMap
    {
        private readonly TileType[,] _tileMap;
        public readonly UInt16 Width;
        public readonly UInt16 Height;
        public readonly int RealWidth;
        public readonly int RealHeight;

        public OldTileMap(UInt16 width, UInt16 height)
        {
            Width = width;
            Height = height;
            _tileMap = new TileType[width, height];

            Random rand = new Random();

            //Fill tilemap with grass
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (rand.Next(100) > 20)
                        _tileMap[x, y] = ResourceManager.GetTile("grass.xml");
                    else
                        _tileMap[x, y] = ResourceManager.GetTile("brickwall.xml");
                }
            }

            //Calculate real world bounds
            RealHeight = height * TileType.TILE_HEIGHT;
            RealWidth = width * TileType.TILE_WIDTH;
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

        public TileType this[int x, int y]
        {
            get { return _tileMap[x, y]; }
        }
    }
}
