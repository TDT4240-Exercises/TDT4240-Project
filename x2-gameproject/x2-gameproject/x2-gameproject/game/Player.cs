using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game 
{
    class Player : Entity
    {
        public enum Controllers 
        {
            Forward,
            Back,
            Left,
            Right,
            Shoot
        }

        private readonly Dictionary<Keys, Controllers> _controlMap;
        private readonly string _playerName;

        public Player(string playername, UnitType type, Dictionary<Keys, Controllers> input)
            : base(type, EntityController.Player)
        {
            _playerName     = playername;
            Position 		= new Vector2 (100, 100);
            _controlMap     = input;
        }

        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {
            //Handle key inputs
            if (GetController() == EntityController.Player && keyboard.HasValue)
            {
                foreach (var key in _controlMap.Where(key => keyboard.Value.IsKeyDown(key.Key)))
                {
                    switch (key.Value)
                    {
                        case Controllers.Left:
                            Rotation -= turnRate;   // Rotate according to keys and turnRate
                            break;

                        case Controllers.Right:
                            Rotation += turnRate;
                            break;

                        case Controllers.Forward:
                            Velocity.X = (float) Math.Cos(Rotation) * speed;
                            Velocity.Y = (float)Math.Sin(Rotation) * speed;
                            break;

                        case Controllers.Back:
                            Velocity.X = (float)-Math.Cos(Rotation) * speed;
                            Velocity.Y = (float)-Math.Sin(Rotation) * speed;
                            break;

                        case Controllers.Shoot:
                            FireProjectile();
                            break;

                    }
                }
            }

            base.Update (delta, keyboard, mouse);
        }

        /// <summary>
        /// Sets individual controllers
        /// </summary>
        /// <param name="controller">The controller to set</param>
        /// <param name="key">The key to associate with the goven control</param>
        public void SetController(Controllers controller, Keys key)
        {
            _controlMap[key] = controller;
        }

        public void SetPosition(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }
    }
}