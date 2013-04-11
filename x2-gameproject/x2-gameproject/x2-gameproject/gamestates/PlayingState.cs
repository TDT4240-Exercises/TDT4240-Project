using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    internal class PlayingState : GameState
    {
        private readonly TileMap _tileMap;
        private readonly List<Entity> _entities;
        private List<Player> _playerList;

        public PlayingState(List<Player> playerList)
        {
            //Initialize level
            _entities = new List<Entity>(64);
            _tileMap = new TileMap(100, 100);

            //Initialize players TODO: as parameter in constructor
            _entities.AddRange(playerList);
            _playerList = playerList;

            //Reset particles
            ParticleEngine.Clear();


            Camera.WorldRectangle = new Rectangle(0, 0, _tileMap.RealWidth, _tileMap.RealHeight);
            Camera.ViewPortWidth = 800;
            Camera.ViewPortHeight = 480;
        }

        protected override void Update(GameTime delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            //TODO: this should open a small ingame menu?
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                NextGameState = null; //Go back to main menu
                return;
            }


            //Update Camera position
            Camera.Position = _playerList[0].Position - new Vector2(Camera.ViewPortWidth, Camera.ViewPortHeight)/2; //IKKE FERDIG!

            //Update particle effects
            ParticleEngine.Update(delta);

            //Update all entities (TODO: this could be done in parallel?)
            foreach (Entity entity in _entities)
            {
                entity.Update(delta, keyboard, mouse);

                //Collision with map edges
                if (entity.Position.X < 0)                                      entity.X = 0;
                if (entity.Position.Y < 0)                                      entity.Y = 0;
                if (entity.Position.X + entity.Width > _tileMap.RealWidth)      entity.X = _tileMap.RealWidth;
                if (entity.Position.Y + entity.Height > _tileMap.RealHeight)    entity.Y = _tileMap.RealHeight;

				EntityWorldCollision(entity);

                //Collision with world
                /*int tileX = entity.GetX() / TileType.TILE_WIDTH;
                int tileY = entity.GetY() / TileType.TILE_HEIGHT;
                if (_tileMap[tileX, tileY].BlocksMovement)
                {
                    entity.Velocity.X = -entity.Velocity.X * 2;
                    entity.Velocity.Y = -entity.Velocity.Y * 2;
                }*/
            }

			for (int i = 0; i < _entities.Count; ++i){
				for (int j = i + 1; j < _entities.Count; ++j){
					EntityEntityCollision(_entities[i], _entities[j]);
				}
			}
        }

		private void EntityEntityCollision(Entity en1, Entity en2){
			Vector2 en1Center = en1.Position;
			int en1Radius = (en1.Height + en1.Width) / 4; // Average of height and width divided by 2 => 4
			
			Vector2 en2Center = en2.Position;
			int en2Radius = (en2.Height + en2.Width) / 4; // Average of height and width divided by 2 => 4

			Vector2 diffVector = en1Center - en2Center; // From en2 to en1
			int distance = (int)diffVector.Length ();

			if (distance < en1Radius + en2Radius){ // Collision
				int moveDistance = (en1Radius + en2Radius) - distance;
				diffVector /= distance;

				diffVector *= moveDistance / 2;

				en1.Position += diffVector;
				en2.Position -= diffVector;
			}
		}

		private void EntityWorldCollision(Entity entity){
			Vector2 entityInTile = new Vector2();
			entityInTile.X = entity.X / TileType.TILE_WIDTH;
			entityInTile.Y = entity.Y / TileType.TILE_HEIGHT;

			entityInTile = _tileMap.GetSquareAtPixel (entity.Position);

			// Collision detect in a 5x5 grid to where you are
			for (int i = 0; i < 5; ++i) {
				for (int j = 0; j < 5; ++j){
					EntityTileCollision (entity, new Vector2 (entityInTile.X + (i-2), entityInTile.Y + (j-2)));
				}
			}

		}

		private void EntityTileCollision(Entity entity, Vector2 tile){
			if (tile.X < 0 || tile.Y < 0)
				return;

			if (_tileMap.GetTileAtSquare((int)tile.X, (int)tile.Y).BlocksMovement){
				Vector2 collisionTileCenter = _tileMap.GetSquareCenter(tile);
				int collisionTileRadius = _tileMap.TileWidth / 2;

				Vector2 entityCenter = entity.Position;
				int entityRadius = (entity.Height + entity.Width) / 4;// Average of height and width divided by 2 => 4
				entityRadius -= 8; // Make it possible to pass between tiles

				Vector2 diffVector = entityCenter - collisionTileCenter; // Tile to Entity
				int distance = (int)diffVector.Length();
				
				if (distance < collisionTileRadius + entityRadius){ // Collision
					int moveRadius = (collisionTileRadius + entityRadius) - distance; // how long to move to not collide any more
					diffVector /= diffVector.Length();
					Vector2 moveVector = diffVector * moveRadius;
					
					entity.Position += moveVector;
				}
			}
		}


        protected override void Draw(RenderEngine renderEngine)
        {
            //Draw the level
            renderEngine.Render(_tileMap);
            
            //Draw each entity visible on the screen
            foreach (Entity entity in _entities.Where(entity => Camera.ObjectIsVisible(entity.hitBox)))
            {
                renderEngine.Render(entity);
            }

            //Draw particle effects
            ParticleEngine.Render(renderEngine);
        }
    }
}
