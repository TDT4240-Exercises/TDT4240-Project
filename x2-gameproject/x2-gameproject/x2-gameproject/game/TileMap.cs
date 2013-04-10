using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    class TileMap
    {
        #region Declarations
        private readonly TileType[,] mapSquares;
        public readonly int TileWidth = TileType.TILE_WIDTH;
        public readonly int TileHeight = TileType.TILE_HEIGHT;
        public readonly int MapWidth;
        public readonly int MapHeight;
        public readonly int RealWidth;
        public readonly int RealHeight;

        private List<Rectangle> tiles = new List<Rectangle>();

        private Random rand = new Random();

        #endregion

        #region Constructor
        public TileMap(int mapWidth, int mapHeight)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            mapSquares = new TileType[MapWidth, MapHeight];

            //Fill tilemap with grass
            for (int x = 0; x < mapWidth; ++x)
            {
                for (int y = 0; y < mapHeight; ++y)
                {
                    if (rand.Next(100) > 20)
                        mapSquares[x, y] = ResourceManager.GetTile("grass.xml");
                    else
                        mapSquares[x, y] = ResourceManager.GetTile("brickwall.xml");
                }
            }

            
            RealWidth = mapWidth * TileType.TILE_WIDTH;
            RealHeight = mapHeight * TileType.TILE_HEIGHT;
        }

        #endregion

        #region Information about Map Squares

        public int GetSquareByPixelX(int pixelX)
        {
            return pixelX / TileWidth;
        }

        public int GetSquareByPixelY(int pixelY)
        {
            return pixelY / TileHeight;
        }

        public Vector2 GetSquareAtPixel(Vector2 pixelLocation)
        {
            return new Vector2(
                GetSquareByPixelX((int)pixelLocation.X),
                GetSquareByPixelY((int)pixelLocation.Y));
        }

        public Vector2 GetSquareCenter(int squareX, int squareY)
        {
            return new Vector2(
                (squareX * TileWidth) + (TileWidth / 2),
                (squareY * TileHeight) + (TileHeight / 2));
        }

        public Vector2 GetSquareCenter(Vector2 square)
        {
            return GetSquareCenter(
                (int)square.X,
                (int)square.Y);
        }

        public Rectangle SquareWorldRectangle(int x, int y)
        {
            return new Rectangle(
                x * TileWidth,
                y * TileHeight,
                TileWidth,
                TileHeight);
        }

        public Rectangle SquareWorldRectangle(Vector2 square)
        {
            return SquareWorldRectangle(
                (int)square.X,
                (int)square.Y);
        }

        public Rectangle SquareScreenRectangle(int x, int y)
        {
            return Camera.WorldToScreen(SquareWorldRectangle(x, y));
        }

        public Rectangle SquareSreenRectangle(Vector2 square)
        {
            return SquareScreenRectangle((int)square.X, (int)square.Y);
        }
        #endregion

        #region Information about Map Tiles

        public TileType GetTileAtSquare(int tileX, int tileY)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                return mapSquares[tileX, tileY];
            }
            else
            {
                return null;
            }
        }

        public void SetTileAtSquare(int tileX, int tileY, TileType tile)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                mapSquares[tileX, tileY] = tile;
            }
        }

        public TileType GetTileAtPixel(int pixelX, int pixelY)
        {
            return GetTileAtSquare(
                GetSquareByPixelX(pixelX),
                GetSquareByPixelY(pixelY));
        }

        public TileType GetTileAtPixel(Vector2 pixelLocation)
        {
            return GetTileAtPixel(
                (int)pixelLocation.X,
                (int)pixelLocation.Y);
        }
        #endregion

        #region Drawing
        public void Draw(SpriteBatch spriteBatch)
        {
            int startX = GetSquareByPixelX((int)Camera.Position.X);
            int endX = GetSquareByPixelX((int)Camera.Position.X +
                  Camera.ViewPortWidth);

            int startY = GetSquareByPixelY((int)Camera.Position.Y);
            int endY = GetSquareByPixelY((int)Camera.Position.Y +
                      Camera.ViewPortHeight);

            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    if ((x >= 0) && (y >= 0) &&
                        (x < MapWidth) && (y < MapHeight))
                    {
                        GetTileAtSquare(x, y).Draw(SquareScreenRectangle(x, y), spriteBatch);
                    }
                }
        }
        #endregion
    }
}
