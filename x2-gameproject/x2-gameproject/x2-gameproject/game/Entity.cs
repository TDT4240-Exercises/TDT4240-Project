using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{

    /// <summary>
    /// An Entity is a unit that is either controlled by an AI or a player
    /// </summary>
    abstract class Entity : GameObject
    {
        public const float VelocityFalloff = 0.95f;
        protected UnitType type;
        protected float TurnRate;
        protected float Health;
        protected float AttackCooldown;
        protected ParticleTemplate CurrentWeapon;
        public float Speed;

        /// <summary>
        /// Constructor for an Entity
        /// </summary>
        /// <param name="textureID">The name of the texture file to use</param>
        public Entity(UnitType type)
        {
            IsCollidable = true;
            this.type = type;
            Texture = type.Texture;
            Width = Texture.Width;
            Height = Texture.Height;
            TurnRate = type.GetValue<float>(UnitValues.TurnRate);
            Health = type.GetValue<float>(UnitValues.Health);
            Speed = type.GetValue<float>(UnitValues.Speed);
            CurrentWeapon = ResourceManager.GetParticleTemplate(type.GetValue<string>(UnitValues.PrimaryWeapon));
        }

        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {
            Position += Velocity;
            Velocity *= VelocityFalloff;
            if (AttackCooldown > 0) AttackCooldown -= (float)delta.ElapsedGameTime.TotalSeconds;
        }

        public void FireProjectile()
        {
            if (AttackCooldown > 0) return;
            AttackCooldown = 0.5f;
            ParticleEngine.SpawnProjectile(this, CurrentWeapon);
        }

    }
}
