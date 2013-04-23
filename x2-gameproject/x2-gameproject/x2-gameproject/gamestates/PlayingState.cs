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
        private double _enemySpawnRate;
        private readonly Random rand = new Random();
        private readonly bool _versusMode;
        private readonly Label _player1Score;
        private readonly Label _player2Score;

        public PlayingState(List<Player> playerList, bool versusMode)
        {
            //Initialize level
            _entities = new List<Entity>(64);
            _tileMap = new TileMap(24, 24);
            _versusMode = versusMode;

            //Initialize players
            _entities.AddRange(playerList);
            _playerList = playerList;

            //Place players somewher safe
            foreach (Player player in playerList)
            {
                player.Position = GetRandomSpawnPosition();
            }

            //Reset particles
            ParticleEngine.Clear();

            Camera.WorldRectangle = new Rectangle(0, 0, _tileMap.RealWidth, _tileMap.RealHeight);

            //Player 1 GUI
            Label player1Name = new Label(_playerList[0].Name, 200, 5);
            components.Add(player1Name);

            _player1Score = new Label("SCORE: 0", 200, 32);
            components.Add(_player1Score);

            //Player 2 GUI
            if (_playerList.Count > 1)
            {
                Label player2Name = new Label(_playerList[1].Name, 500, 5);
                components.Add(player2Name);

                _player2Score = new Label("SCORE: 0", 500, 32);
                components.Add(_player2Score);
            }
        }

        private void SpawnEnemies(GameTime delta)
        {
            if (delta.TotalGameTime.TotalSeconds < _enemySpawnRate) return;
            _enemySpawnRate = delta.TotalGameTime.TotalSeconds + 20.0; //spawn next wave in 20 seconds

            List<UnitType> unitTypes = ResourceManager.GetAllUnitTypes();
            

            //Spawn some random opponents
            for (int i = 0; i < 3; ++i)
            {
                _entities.Add(new AIControlledEntity(unitTypes[rand.Next(unitTypes.Count - 1)], _entities, _tileMap));
                _entities.Last().Position = GetRandomSpawnPosition();
            }

            //Spawn some squads
            for (int i = 0; i < 3; ++i)
            {
                Entity leader = new AIControlledEntity(ResourceManager.GetUnitType("soldier_captain.xml"), _entities, _tileMap);
                _entities.Add(leader);
                leader.Position = GetRandomSpawnPosition();
                for (int j = 0; j < 5; ++j)
                {
                    _entities.Add(new AIControlledEntity(ResourceManager.GetUnitType("soldier.xml"), _entities, _tileMap, leader));
                    _entities.Last().Position = leader.Position;
                }
            }
        }

        private Vector2 GetRandomSpawnPosition()
        {
            switch (rand.Next(1, 4))
            {
                case 1:
                     return new Vector2(0, rand.Next(_tileMap.RealHeight));
       
                case 2:
                     return new Vector2(rand.Next(_tileMap.RealWidth), 0);

                case 3:
                    return new Vector2(_tileMap.RealWidth, rand.Next(_tileMap.RealHeight));

                default:
                    return new Vector2(rand.Next(_tileMap.RealWidth), _tileMap.RealHeight);
            }
        }

        protected override void Update(GameTime delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            //TODO: this should open a small ingame menu?
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                NextGameState = null; //Go back to main menu
                return;
            }

            //15 seconds until first wave spawns
            if ((int)_enemySpawnRate == 0)
            {
                _enemySpawnRate = delta.TotalGameTime.TotalSeconds + 15;
            }

            //Update highscore
            _player1Score.Text = "SCORE: " + _playerList[0].Score;
            if(_playerList.Count > 1) _player2Score.Text = "SCORE: " + _playerList[1].Score;

            //Update Camera shake
            if (Camera.shake > 0) Camera.shake -= delta.ElapsedGameTime.Milliseconds;

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

            //Spawn new waves of enemies!
            if(!_versusMode) SpawnEnemies(delta);
        }

        protected override void Draw(RenderEngine renderEngine)
        {
            int cameraOffset = 0;

            //Splitscreen camera?
            if (_playerList.Count == 2 && (_playerList[0].Position - _playerList[1].Position).LengthSquared() >= 800*480 / 2)
            {
                foreach (Player player in _playerList)
                {
                    Camera.ViewPortWidth = 800 / _playerList.Count;
                    Camera.ViewPortHeight = 480;

                    Camera.Position = player.Position - new Vector2(Camera.ViewPortWidth, Camera.ViewPortHeight) / 2;

                    //Draw the level
                    renderEngine.Render(_tileMap, cameraOffset);

                    //Draw each entity visible on the screen
                    foreach (Entity entity in _entities.Where(entity => Camera.ObjectIsVisible(entity.Bounds)))
                    {
                        renderEngine.Render(entity, cameraOffset);
                    }

                    //Draw particle effects
                    ParticleEngine.Render(renderEngine, cameraOffset);

                    cameraOffset += Camera.ViewPortWidth + 32;
                }

                //Draw splitscreen bar
                if (_playerList.Count > 1)
                {
                    renderEngine.Draw(ResourceManager.GetTexture("split.png"), Camera.ViewPortWidth - 32, 0, 64, Camera.ViewPortHeight, Color.White);
                }
            }

            //Normal Camera
            else
            {
                Camera.ViewPortWidth = 800;
                Camera.ViewPortHeight = 480;

                if (_playerList.Count == 1)
                    Camera.Position = _playerList[0].Position - new Vector2(Camera.ViewPortWidth, Camera.ViewPortHeight) / 2;
                else
                {
                    Camera.Position = (_playerList[0].Position + _playerList[1].Position)/2 - new Vector2(Camera.ViewPortWidth, Camera.ViewPortHeight) / 2;
                }

                //Draw the level
                renderEngine.Render(_tileMap, cameraOffset);

                //Draw each entity visible on the screen
                foreach (Entity entity in _entities.Where(entity => Camera.ObjectIsVisible(entity.Bounds)))
                {
                    renderEngine.Render(entity, cameraOffset);
                }

                //Draw particle effects
                ParticleEngine.Render(renderEngine, cameraOffset);
            }

        }
    }
}
