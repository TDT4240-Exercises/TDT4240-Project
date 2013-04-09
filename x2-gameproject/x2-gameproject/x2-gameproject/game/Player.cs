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

        public Player(string playername) : base("basic_tank.png", EntityController.Player)
        {
            _playerName     = playername;
            TurnRate 		= 0.025f;
            Position 		= new Vector2 (100, 100);
            _controlMap     = new Dictionary<Keys, Controllers>();
        }

        public override void Update (TimeSpan delta, KeyboardState? keyboard, MouseState? mouse)
        {
            //Handle key inputs
            if (GetController() == EntityController.Player && keyboard.HasValue)
            {
                foreach (var key in _controlMap.Where(key => keyboard.Value.IsKeyDown(key.Key)))
                {
                    switch (key.Value)
                    {
                        case Controllers.Left:
                            Rotation -= TurnRate;   // Rotate according to keys and turnRate
                            break;

                        case Controllers.Right:
                            Rotation += TurnRate;
                            break;

                        case Controllers.Forward:
                            Velocity.X = (float) Math.Cos(Rotation);
                            Velocity.Y = (float) Math.Sin(Rotation);
                            break;

                        case Controllers.Back:
                            Velocity.X = (float) -Math.Cos(Rotation);
                            Velocity.Y = (float) -Math.Sin(Rotation);
                            break;

                        case Controllers.Shoot:
                            ParticleEngine.SpawnParticle(GetPosition(), ResourceManager.GetParticleTemplate("fireball.xml"));
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