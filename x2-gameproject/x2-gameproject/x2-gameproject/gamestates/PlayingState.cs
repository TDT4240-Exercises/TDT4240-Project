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
        private Player player1;

        public PlayingState()
        {
            _entities = new List<Entity>(64);
            _tileMap = new TileMap(100, 100);

            //Initialize player TODO: as parameter in constructor
            player1 = new Player("Player 1");
            _entities.Add(player1);
            player1.SetController(Player.Controllers.Forward, Keys.W);
            player1.SetController(Player.Controllers.Back, Keys.S);
            player1.SetController(Player.Controllers.Left, Keys.A);
            player1.SetController(Player.Controllers.Right, Keys.D);
            player1.SetController(Player.Controllers.Shoot, Keys.Space);
            player1.SetPosition(350, 350);
        }

        protected override void Update(TimeSpan delta, KeyboardState keyboard, MouseState mouse, RenderEngine renderEngine)
        {
            //TODO: this should open a small ingame menu?
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                NextGameState = null; //Go back to main menu
                return;
            }

            //Update camera position
            renderEngine.Camera.X = player1.GetX() - renderEngine.Camera.Width / 2;
            renderEngine.Camera.Y = player1.GetY() - renderEngine.Camera.Height / 2;

            //Update particle effects
            ParticleEngine.Update(delta);

            //Update all entities (TODO: this could be done in parallel?)
            foreach (Entity entity in _entities)
            {
                entity.Update(delta, keyboard, mouse);

                //Collision with map edges
                if (entity.Position.X < 0)                                      entity.Position.X = 0;
                if (entity.Position.Y < 0)                                      entity.Position.Y = 0;
                if (entity.Position.X + entity.Width > _tileMap.RealWidth)      entity.Position.X = _tileMap.RealWidth;
                if (entity.Position.Y + entity.Height > _tileMap.RealHeight)    entity.Position.Y = _tileMap.RealHeight;

                //Collision with world
                int tileX = entity.GetX() / TileType.TILE_WIDTH;
                int tileY = entity.GetY() / TileType.TILE_HEIGHT;
                if (_tileMap[tileX, tileY].BlocksMovement)
                {
                    entity.Velocity.X = -entity.Velocity.X * 2;
                    entity.Velocity.Y = -entity.Velocity.Y * 2;
                }
            }
        }


        protected override void Draw(RenderEngine renderEngine)
        {
            //Draw the level
            renderEngine.Render(_tileMap);
            
            //Draw each entity visible on the screen
            foreach (Entity entity in _entities.Where(entity => renderEngine.IsVisible(entity.hitBox)))
            {
                renderEngine.Render(entity);
            }

            //Draw particle effects
            ParticleEngine.Render(renderEngine);
        }
    }
}
