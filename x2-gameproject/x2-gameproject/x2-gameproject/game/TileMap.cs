using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    class TileMap
    {
        #region Declarations
        private readonly TileType[,] _mapSquares;
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
            _mapSquares = new TileType[MapWidth, MapHeight];

            //Fill tilemap with grass
            for (int x = 0; x < mapWidth; ++x)
            {
                for (int y = 0; y < mapHeight; ++y)
                {
                    if (rand.Next(100) > 20)
                        _mapSquares[x, y] = ResourceManager.GetTile("grass.xml");
                    else
                        _mapSquares[x, y] = ResourceManager.GetTile("brickwall.xml");
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

        public Point GetSquareAtPixel(Vector2 pixelLocation)
        {
            return new Point(
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
                return _mapSquares[tileX, tileY];
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
                _mapSquares[tileX, tileY] = tile;
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

            //Clip to valid ranges
            startX = Math.Min(Math.Max(0, startX), MapWidth - 1);
            startY = Math.Min(Math.Max(0, startY), MapHeight - 1);
            endX = Math.Min(Math.Max(0, endX), MapWidth - 1);
            endY = Math.Min(Math.Max(0, endY), MapHeight - 1);

            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    _mapSquares[x, y].Draw(SquareScreenRectangle(x, y), spriteBatch);
                }
        }
        #endregion

        #region Collision Detection

        /// <summary>
        /// Detects and handles collision between GameObjects and the World.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>true if there occured an collision</returns>
        public bool WorldCollision(GameObject gameObject)
        {
            bool hasCollided = false;

            Point entityInTile = GetSquareAtPixel(gameObject.Position);

            // Collision detect in a 5x5 grid to where you are
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    hasCollided |= HandleTileCollision(gameObject, new Vector2(entityInTile.X + (i - 2), entityInTile.Y + (j - 2)));
                }
            }

            return hasCollided;
        }

        private bool HandleTileCollision(GameObject gameObject, Vector2 tile)
        {
            //Prevent array out of bounds
            if (tile.X < 0 || tile.Y < 0 || tile.X >= MapWidth || tile.Y >= MapHeight) return false;

            //Is the tile collidable?
            if (!_mapSquares[(int)tile.X, (int)tile.Y].BlocksMovement) return false;

            Vector2 collisionTileCenter = GetSquareCenter(tile);
            int collisionTileRadius = TileWidth / 2;

            Vector2 entityCenter = gameObject.Position;
            int entityRadius = (gameObject.Height + gameObject.Width) / 4;// Average of height and width divided by 2 => 4
            entityRadius -= 8; // Make it possible to pass between tiles

            Vector2 diffVector = entityCenter - collisionTileCenter; // Tile to Entity
            int distance = (int)diffVector.Length();

            //Collision?
            if (distance < collisionTileRadius + entityRadius)
            { 
                int moveRadius = (collisionTileRadius + entityRadius) - distance; // how long to move to not collide any more
                diffVector /= diffVector.Length();
                Vector2 moveVector = diffVector * moveRadius;

                gameObject.Position += moveVector;
                return true;
            }

            return false;
        }
        #endregion

        public bool IsWalkable(int tileX, int tileY)
        {
            //Everything outside the map is impassable
            if (tileX < 0 || tileY < 0 || tileX >= MapWidth || tileY >= MapWidth) return false;

            return !_mapSquares[tileX, tileY].BlocksMovement;
        }
    }
}
