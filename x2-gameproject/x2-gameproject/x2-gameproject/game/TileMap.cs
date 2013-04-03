using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    class TileMap
    {
        private readonly TileType[,] _tileMap;
        public readonly UInt16 Width;
        public readonly UInt16 Height;
        public readonly int RealWidth;
        public readonly int RealHeight;

        public TileMap(UInt16 width, UInt16 height)
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
                    if(rand.Next(100) > 20)
                        _tileMap[x, y] = ResourceManager.GetTile("grass.xml");
                    else
                        _tileMap[x, y] = ResourceManager.GetTile("brickwall.xml");
                }
            }

            //Calculate real world bounds
            RealHeight = height * TileType.TILE_HEIGHT;
            RealWidth  = width * TileType.TILE_WIDTH;
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

        public void Draw(SpriteBatch spriteBatch, Rectangle camera)
        {
            //Figure out the visible bounds of our map
            int sttX = Math.Max(0, camera.X / TileType.TILE_WIDTH - 2);
            int sttY = Math.Max(0, camera.Y / TileType.TILE_HEIGHT);
            int endX = Math.Min(Width, sttX + camera.Width / TileType.TILE_WIDTH);
            int endY = Math.Min(Height, sttY + camera.Height / TileType.TILE_HEIGHT);

            //Scale each tile to our prefered resolution (how many can we fit inside the camera?)
            int width = (camera.Width / (endX - sttX - 3));
            int height = (camera.Height / (endY - sttY));
            
            //Draw the entire visible map
            for (int x = sttX; x < endX; ++x)
            {
                for (int y = sttY; y < endY; ++y)
                {
                    _tileMap[x, y].Draw(x * width - camera.X, y * height - camera.Y, width, height, spriteBatch);
                }
            }

            //Debug info
            spriteBatch.DrawString(ResourceManager.GetDebugFont(), "Camera: " + camera, new Vector2(), Color.White);
            spriteBatch.DrawString(ResourceManager.GetDebugFont(), "sttX: " + sttX + ", " + "sttY: " + sttY + ", " + "endX: " + endX + ", " + "endY: " + endY, new Vector2(0, 16), Color.White);
        }

        public TileType this[int x, int y]
        {
            get { return _tileMap[x, y]; }
        }
    }
}
