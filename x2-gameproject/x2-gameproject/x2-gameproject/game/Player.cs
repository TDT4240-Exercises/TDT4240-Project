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
            Primary,
            Secondary
        }

        private readonly Dictionary<Keys, Controllers> _controlMap;
        public readonly string Name;

        public Player(string playername, UnitType type, Dictionary<Keys, Controllers> input)
            : base(type)
        {
            Name = playername;
            Position 		= new Vector2 (100, 100);
            _controlMap     = input;
            Team = 'P';     //Team Player
        }


        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {
            //Handle key inputs
            if (keyboard.HasValue)
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
                            VelX = (float) Math.Cos(Rotation) * Speed;
                            VelY = (float) Math.Sin(Rotation) * Speed;
                            break;

                        case Controllers.Back:
                            VelX = (float)-Math.Cos(Rotation) * Speed;
                            VelY = (float)-Math.Sin(Rotation) * Speed;
                            break;

                        case Controllers.Secondary:
                        case Controllers.Primary:
                            FireProjectile(key.Value == Controllers.Primary);
                            break;

                    }
                }
            }

            //Regenerate
            Health += 0.05f;
            if (Health > MaxHealth) Health = MaxHealth;

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
            X = x;
            Y = y;
        }
    }
}