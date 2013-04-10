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

                //Collision with world
                /*int tileX = entity.GetX() / TileType.TILE_WIDTH;
                int tileY = entity.GetY() / TileType.TILE_HEIGHT;
                if (_tileMap[tileX, tileY].BlocksMovement)
                {
                    entity.Velocity.X = -entity.Velocity.X * 2;
                    entity.Velocity.Y = -entity.Velocity.Y * 2;
                }*/
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
