using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    class TileMap
    {
        private readonly TileType[,] _mapSquares;
        private readonly float[,]    _tileDamage;
        public readonly int TileWidth = TileType.TILE_WIDTH;
        public readonly int TileHeight = TileType.TILE_HEIGHT;
        public readonly int MapWidth;
        public readonly int MapHeight;
        public readonly int RealWidth;
        public readonly int RealHeight;

        private readonly Random rand = new Random();
        private readonly Texture2D _damageOverlay = ResourceManager.GetTexture("tiledamage.png");


        #region Constructor
        public TileMap(int mapWidth, int mapHeight)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            _mapSquares = new TileType[MapWidth, MapHeight];
            _tileDamage = new float[mapWidth,mapHeight];

            RealWidth = mapWidth * TileType.TILE_WIDTH;
            RealHeight = mapHeight * TileType.TILE_HEIGHT;

            GenerateRandomLevel();
        }

        #endregion

        private void GenerateRandomLevel()
        {

            //Fill tilemap with grass
            for (int x = 0; x < MapWidth; ++x)
            {
                for (int y = 0; y < MapHeight; ++y)
                {
                    _mapSquares[x, y] = ResourceManager.GetTile("grass.xml");
                }
            }

            //Place a sign
            _mapSquares[rand.Next(0, MapWidth - 1), rand.Next(0, MapWidth - 1)] = ResourceManager.GetTile("sign.xml");

            //Place some trees
            int nrOfTrees = rand.Next(10, 40);
            while (nrOfTrees-- >= 0)
            {
                int x = rand.Next(0, MapWidth-1);
                int y = rand.Next(0, MapHeight-1);
                if(rand.NextDouble() > 0.5)
                    _mapSquares[x, y] = ResourceManager.GetTile("pinetree.xml");
                else
                    _mapSquares[x, y] = ResourceManager.GetTile("oaktree.xml");
            }

            //Place a couple of houses
            int nrOfHouses = rand.Next(2, 4);
            while (nrOfHouses-- >= 0)
            {
                int width = rand.Next(3, 8);
                int height = rand.Next(3, 8);
                int x = rand.Next(MapWidth - width - 2);
                int y = rand.Next(MapHeight - height - 2);

                TileType wallType;

                if (rand.NextDouble() > 0.5) wallType = ResourceManager.GetTile("brickwall.xml");
                else wallType = ResourceManager.GetTile("ironwall.xml");

                //Place the house itself
                for (int dx = 0; dx <= width; ++dx)
                {
                    for (int dy = 0; dy <= height; ++dy)
                    {
                        if (dy == 0 || dx == 0 || dx == width || dy == height)
                        {
                            _mapSquares[x + dx, y + dy] = wallType;
                        }
                        else
                        {
                            _mapSquares[x + dx, y + dy] = ResourceManager.GetTile("hewnfloor.xml");
                        }
                    }
                }

                //Place doors
                int nrOfDoors = rand.Next(1, 2);
                while (nrOfDoors-- >= 0)
                {
                    int doorX = 1;
                    int doorY = 1;

                    switch (rand.Next(1, 4))
                    {
                        case 1:
                            doorX = rand.Next(1, width - 1);
                            doorY = 0;
                            break;

                        case 2:
                            doorX = width;
                            doorY = rand.Next(1, height - 1);
                            break;

                        case 3:
                            doorX = rand.Next(1, width - 1);
                            doorY = height;
                            break;

                        case 4:
                            doorX = 0;
                            doorY = rand.Next(1, height - 1);
                            break;
                    }


                    _mapSquares[doorX+x, doorY+y] = ResourceManager.GetTile("hewnfloor.xml"); 
                }

                //Furniture
                int nrOfFeatures = rand.Next(0, 2);
                while (nrOfFeatures-- >= 0)
                {
                    int dx = rand.Next(1, width - 1);
                    int dy = rand.Next(1, height - 1);
                    _mapSquares[x+dx, y+dy] = ResourceManager.GetTile("supplies.xml"); 
                }
            }
        }

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
                    Rectangle target = SquareScreenRectangle(x, y);
                    _mapSquares[x, y].Draw(target, spriteBatch);
                    
                    //Damaged tile overlay?
                    if (_tileDamage[x, y] > 0)
                    {
                        float max = _mapSquares[x, y].GetValue<float>(TileValues.Health);

                        //determine damage level
                        int level = (int)Math.Round((6.0/max) *_tileDamage[x, y]);

                        spriteBatch.Draw(_damageOverlay, target, new Rectangle(64 * level, 0, 64, 64), Color.White);
                    }
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

        public Point? GetCollidedTile(GameObject gameObject)
        {
            Point entityInTile = GetSquareAtPixel(gameObject.Position);
            // Collision detect in a 5x5 grid to where you are
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    if (HandleTileCollision(gameObject, new Vector2(entityInTile.X + (i - 2), entityInTile.Y + (j - 2))))
                    {
                        return new Point(entityInTile.X + (i - 2), entityInTile.Y + (j - 2));
                    }
                }
            }

            return null;
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

        public void DestroyTile(int x, int y, float damage)
        {
            if (!_mapSquares[x, y].GetValue<bool>(TileValues.Destructible)) return;

            //Deal some love
            _tileDamage[x, y] += damage;

            //Enough damage to completely destroy?
            if (_tileDamage[x, y] >= _mapSquares[x, y].GetValue<float>(TileValues.Health))
            {
                string newTile = _mapSquares[x, y].GetValue<string>(TileValues.DestroyedTile);
                if (newTile != null) _mapSquares[x, y] = ResourceManager.GetTile(newTile);
                else _mapSquares[x, y] = ResourceManager.GetTile("grass.xml");
                _tileDamage[x, y] = 0;
            }

        }
    }
}
