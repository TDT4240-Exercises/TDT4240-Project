using System;
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
        private ParticleTemplate template;
        private float secondsRemaining;
        private float size;
        private float alpha;
        public float Speed;

        public bool isDestroyed;

        //These two are cached for performance reasons
        private Vector2 centre;

        public Particle(Vector2 initialPosition, ParticleTemplate template)
        {
            this.template = template;
            Position = initialPosition;
            Rotation = template.GetValue<float>(ParticleValues.InitialRotation);
            size = template.GetValue<float>(ParticleValues.InitialSize);
            secondsRemaining = template.GetValue<float>(ParticleValues.LifeTime);
            Texture = template.GetValue<Texture2D>(ParticleValues.Texture);
            centre = new Vector2(Texture.Width / 2, Texture.Height / 2);
            alpha = 1.0f - template.GetValue<float>(ParticleValues.InitialAlpha);
        }

        public void Destroy()
        {
            if (isDestroyed) return;
            isDestroyed = true;

            //Spawn other particles on death?
            ParticleTemplate spawn = template.GetValue<ParticleTemplate>(ParticleValues.SpawnParticleOnEnd);
            if (spawn != null)
            {
                ParticleEngine.SpawnParticle(Position, spawn);
            }
        }

        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {
            float timeUnit = delta.ElapsedGameTime.Ticks/(float)TimeSpan.TicksPerSecond;

            //Has this particle expired and needs to be removed?
            if (!float.IsInfinity(secondsRemaining))
            {
                secondsRemaining -= (delta.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond);
                if (secondsRemaining <= 0)
                {
                    Destroy();
                    return;
                }
            }

            //Update alpha
            alpha -= template.GetValue<float>(ParticleValues.AlphaAdd) * timeUnit;
            if (alpha <= 0)
            {
                Destroy();
                return;
            }

            //Update size
            size += template.GetValue<float>(ParticleValues.SizeAdd) * timeUnit;
            if (size <= 0)
            {
                Destroy();
                return;
            }
            Width = (int) (Texture.Width * size);
            Height = (int) (Texture.Height * size);

            //Update rotation
            Rotation += template.GetValue<float>(ParticleValues.RotationAdd) * timeUnit;

            //Update velocity depending on rotation and speed
            if (!template.GetValue<bool>(ParticleValues.RotationIndependentVelocity))
            {
                Velocity.X = (float)Math.Cos(Rotation) * Speed;
                Velocity.Y = (float)Math.Sin(Rotation) * Speed;
            }
            else
            {
               // Velocity.X = Speed;
               // Velocity.Y = Speed;
                //TODO: not implemented
            }

            //Update position
            Position += Velocity * timeUnit;
        }
    }
}
