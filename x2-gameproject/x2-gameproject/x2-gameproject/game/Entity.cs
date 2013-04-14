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
        protected internal float Health;
        protected internal float MaxHealth;
        protected float PrimaryAttackCooldown;
        protected float SecondaryAttackCooldown;
        protected ParticleTemplate PrimaryWeapon;
        protected ParticleTemplate SecondaryWeapon;
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
            MaxHealth = Health = type.GetValue<float>(UnitValues.Health);
            Speed = type.GetValue<float>(UnitValues.Speed);
            PrimaryWeapon = ResourceManager.GetParticleTemplate(type.GetValue<string>(UnitValues.PrimaryWeapon));
            SecondaryWeapon = ResourceManager.GetParticleTemplate(type.GetValue<string>(UnitValues.SecondaryWeapon));
            DestroyOnCollsion = type.GetValue<bool>(UnitValues.DestroyOnCollision);
        }

        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {
            Position += Velocity;
            Velocity *= VelocityFalloff;
            if (PrimaryAttackCooldown > 0) PrimaryAttackCooldown -= (float)delta.ElapsedGameTime.TotalSeconds;
            if (SecondaryAttackCooldown > 0) SecondaryAttackCooldown -= (float)delta.ElapsedGameTime.TotalSeconds;
        }

        public void FireProjectile(bool primary = true)
        {
            if (primary)
            {
                if (PrimaryAttackCooldown > 0 || PrimaryWeapon == null) return;
                PrimaryAttackCooldown = PrimaryWeapon.GetValue<float>(ParticleValues.AttackCooldown);
                ParticleEngine.SpawnProjectile(this, PrimaryWeapon);
            }
            else
            {
                if (SecondaryAttackCooldown > 0 || SecondaryWeapon == null) return;
                SecondaryAttackCooldown = SecondaryWeapon.GetValue<float>(ParticleValues.AttackCooldown);
                ParticleEngine.SpawnProjectile(this, SecondaryWeapon);
            }

        }

        public void Damage(float amount)
        {
            Health -= amount;
            if(Health < 0) Destroy();
        }

        public override void Destroy()
        {
            if (IsDestroyed) return;
            string deathParticle = type.GetValue<string>(UnitValues.DeathParticle);
            if(deathParticle != null) ParticleEngine.SpawnParticle(Position, ResourceManager.GetParticleTemplate(deathParticle), true);
            base.Destroy();
        }

    }
}
