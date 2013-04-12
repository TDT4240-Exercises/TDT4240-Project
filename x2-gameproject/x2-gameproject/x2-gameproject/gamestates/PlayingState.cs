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
        private readonly List<Player> _playerList;

        public PlayingState(List<Player> playerList)
        {
            //Initialize level
            _entities = new List<Entity>(64);
            _tileMap = new TileMap(24, 24);

            //Initialize players
            _entities.AddRange(playerList);
            _playerList = playerList;

            //Spawn some random opponents
            Random rand = new Random();
            for (int i = 0; i < 5; ++i) {
                _entities.Add(new AIControlledEntity(rand.NextDouble() > 0.5 ? ResourceManager.GetUnitType("soldier.xml") : ResourceManager.GetUnitType("small_tank.xml"), _entities, _tileMap));
                _entities.Last().Position = new Vector2(rand.Next(_tileMap.RealWidth), rand.Next(_tileMap.RealHeight));
            }

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
            ParticleEngine.Update(delta, _entities, _tileMap);

            //Update all entities (TODO: this could be done in parallel?)
            Queue<Entity> destroyedEntities = new Queue<Entity>();
            foreach (Entity entity in _entities)
            {
                //Update the entity itself (run AI, check input, etc.)
                entity.Update(delta, keyboard, mouse);

                //Collision with map edges
                if (entity.Position.X < 0)                                          entity.X = 0;
                if (entity.Position.Y < 0)                                          entity.Y = 0;
                if (entity.Position.X + entity.Width / 2.0f > _tileMap.RealWidth)   entity.X = _tileMap.RealWidth - entity.Width / 2.0f;
                if (entity.Position.Y + entity.Height / 2.0f > _tileMap.RealHeight) entity.Y = _tileMap.RealHeight - entity.Height / 2.0f;

                //Collision with world
                if(entity.IsCollidable) _tileMap.WorldCollision(entity);

                if (entity.IsDestroyed)
                {
                    destroyedEntities.Enqueue(entity);
                }
            }

            foreach (var destroyedEntity in destroyedEntities) {
                _entities.Remove(destroyedEntity);
            }
     

            //Entity to entity collision detection
			for (int i = 0; i < _entities.Count; ++i)
			{
			    if (!_entities[i].IsCollidable) continue;

				for (int j = i + 1; j < _entities.Count; ++j){
                    if (!_entities[j].IsCollidable) continue;
                    _entities[i].HandleCollision(_entities[j]);
				}
			}
        }

        protected override void Draw(RenderEngine renderEngine)
        {
            //Draw the level
            renderEngine.Render(_tileMap);
            
            //Draw each entity visible on the screen
            foreach (Entity entity in _entities.Where(entity => Camera.ObjectIsVisible(entity.Bounds)))
            {
                renderEngine.Render(entity);
            }

            //Draw particle effects
            ParticleEngine.Render(renderEngine);
        }
    }
}
