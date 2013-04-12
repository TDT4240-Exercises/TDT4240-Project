using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    /// <summary>
    /// A Particle is a lightweight rendering sprite. Use ParticleEngine to create and manage Particle effects.
    /// </summary>
    class Particle : GameObject
    {
        private static readonly Random _rand = new Random();
        private readonly ParticleTemplate _template;
        private float _secondsRemaining;
        internal float Size;
        internal float Alpha;
        public float Speed;

        public bool IsDestroyed;

        public Particle(Vector2 initialPosition, ParticleTemplate template)
        {
            _template = template;

            //Rotation
            Rotation = template.GetValue<float>(ParticleValues.InitialRotation);
            if ((int)Rotation == -1) Rotation = (float)_rand.NextDouble();

            Size = template.GetValue<float>(ParticleValues.InitialSize);
            _secondsRemaining = template.GetValue<float>(ParticleValues.LifeTime);
            Texture = ResourceManager.GetTexture("particles/" + template.GetValue<string>(ParticleValues.Texture));
            Alpha = 1.0f - template.GetValue<float>(ParticleValues.InitialAlpha);
            IsCollidable = template.GetValue<bool>(ParticleValues.CanCollide);
            Position = initialPosition;

            //Sound effect
            string spawnSound = template.GetValue<string>(ParticleValues.SoundEffectOnSpawn);
            if (spawnSound != null) ResourceManager.PlaySoundEffect(spawnSound);
        }

        public void Destroy()
        {
            if (IsDestroyed) return;
            IsDestroyed = true;

            //Spawn other particles on death?
            ParticleTemplate spawn = _template.GetValue<ParticleTemplate>(ParticleValues.SpawnParticleOnEnd);
            if (spawn != null)
            {
                ParticleEngine.SpawnParticle(Position, spawn);
            }
        }

        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {

            //Has this particle expired and needs to be removed?
            if (!float.IsInfinity(_secondsRemaining))
            {
                _secondsRemaining -= (delta.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond);
                if (_secondsRemaining <= 0)
                {
                    Destroy();
                    return;
                }
            }

            //Update alpha
            Alpha -= _template.GetValue<float>(ParticleValues.AlphaAdd);
            if (Alpha <= 0)
            {
                Destroy();
                return;
            }
            if (Alpha > 1.0f) Alpha = 1;

            //Update size
            Size += _template.GetValue<float>(ParticleValues.SizeAdd);
            if (Size <= 0)
            {
                Destroy();
                return;
            }
            Width = (int) (Texture.Width * Size);
            Height = (int) (Texture.Height * Size);

            //Update rotation
            Rotation += _template.GetValue<float>(ParticleValues.RotationAdd);

            //Update velocity depending on rotation and speed
            if (!_template.GetValue<bool>(ParticleValues.RotationIndependentVelocity))
            {
                VelX = (float)Math.Cos(Rotation) * Speed;
                VelY = (float)Math.Sin(Rotation) * Speed;
            }
            else
            {
               // Velocity.X = Speed;
               // Velocity.Y = Speed;
                //TODO: not implemented
            }

            //Update position
            Position += Velocity;
        }

        public void Update(GameTime delta, List<Entity> entities, TileMap tileMap)
        {
            if (IsCollidable)
            {
                //Did we hit a wall?
                if (tileMap.WorldCollision(this))
                {
                    Destroy();
                    return;
                }

                //Do we hit an gameObject?
                if (entities.Where(entity => entity.IsCollidable).Any(entity => entity.Team != Team && entity.HandleCollision(this, false)))
                {
                    Destroy();
                    return;
                }
            }


            Update(delta, (KeyboardState?)null, null);
        }
    }
}
