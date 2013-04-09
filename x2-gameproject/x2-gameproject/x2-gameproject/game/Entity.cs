using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{

    public enum EntityController
    {
        None,              //No controller
        DefaultAI,         //Default AI
        KamikazeAI,        //Rush towards opponent
        SmartAI,           //Sneaky AI hides behind walls, etc.
        Player             //This entity is controlled by a player
    }

    /// <summary>
    /// An Entity is a unit that is either controlled by an AI or a player
    /// </summary>
    class Entity : GameObject
    {
        public const float VELOCITY_FALLOFF = 0.95f;
        private EntityController controller;
        protected UnitType type;
        protected float turnRate;
        protected float health;
        protected ParticleTemplate currentWeapon;

        /// <summary>
        /// Constructor for an Entity
        /// </summary>
        /// <param name="textureID">The name of the texture file to use</param>
        /// <param name="setController">Is this entity controlled by an AI or a player?</param>
        public Entity(UnitType type, EntityController setController)
        {
            controller = setController;
            Texture = type.Texture;
            Width = Texture.Width;
            Height = Texture.Height;
            turnRate = type.GetValue<float>(UnitValues.TurnRate);
            health = type.GetValue<float>(UnitValues.Health);
            currentWeapon = ResourceManager.GetParticleTemplate("missile.xml");
        }

        public override void Update(TimeSpan delta, KeyboardState? keyboard, MouseState? mouse)
        {
            Position += Velocity;
            Velocity *= VELOCITY_FALLOFF;
        }

        public EntityController GetController()
        {
            return controller;
        }

    }
}
