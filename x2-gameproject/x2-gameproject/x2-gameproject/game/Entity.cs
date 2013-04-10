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
        protected float attackCooldown;
        protected float speed;
        protected ParticleTemplate currentWeapon;

        /// <summary>
        /// Constructor for an Entity
        /// </summary>
        /// <param name="textureID">The name of the texture file to use</param>
        /// <param name="setController">Is this entity controlled by an AI or a player?</param>
        public Entity(UnitType type, EntityController setController)
        {
            this.type = type;
            controller = setController;
            Texture = type.Texture;
            Width = Texture.Width;
            Height = Texture.Height;
            turnRate = type.GetValue<float>(UnitValues.TurnRate);
            health = type.GetValue<float>(UnitValues.Health);
            speed = type.GetValue<float>(UnitValues.Speed);
            currentWeapon = ResourceManager.GetParticleTemplate("missile.xml");
        }

        public float Speed { get { return speed; } }

        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {
            Position += Velocity;
            Velocity *= VELOCITY_FALLOFF;
            if (attackCooldown > 0) attackCooldown -= (float)delta.ElapsedGameTime.TotalSeconds;
        }

        public EntityController GetController()
        {
            return controller;
        }

        public void FireProjectile()
        {
            if (attackCooldown > 0) return;
            attackCooldown = 0.5f;
            ParticleEngine.SpawnProjectile(this, currentWeapon);
        }

    }
}
