using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        
        public bool isDestroyed;

        //These two are cached for performance reasons
        private Vector2 centre;
        private Texture2D texture;

        public Particle(Vector2 initialPosition, ParticleTemplate template)
        {
            this.template = template;
            position = initialPosition;
            rotation = template.GetValue<float>(ParticleValues.InitialRotation);
            size = template.GetValue<float>(ParticleValues.InitialSize);
            secondsRemaining = template.GetValue<float>(ParticleValues.LifeTime);
            texture = template.GetValue<Texture2D>(ParticleValues.Texture);
            centre = new Vector2(texture.Width / 2, texture.Height / 2);
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
                ParticleEngine.SpawnParticle(position, spawn);
            }
        }

        public override void Update(TimeSpan delta)
        {
            float timeUnit = delta.Ticks/(float)TimeSpan.TicksPerSecond;

            //Has this particle expired and needs to be removed?
            if (!float.IsInfinity(secondsRemaining))
            {
                secondsRemaining -= (delta.Ticks / (float)TimeSpan.TicksPerSecond);
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

            //Update rotation
            rotation += template.GetValue<float>(ParticleValues.RotationAdd) * timeUnit;

            //Update position
            position += velocity;

            //Update velocity
            velocity.X += template.GetValue<float>(ParticleValues.VelocityAdd) * timeUnit;     //TODO: add velocity based on rotation?
            velocity.Y += template.GetValue<float>(ParticleValues.VelocityAdd) * timeUnit;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White * alpha, rotation, centre, size, SpriteEffects.None, 0);
        }
    }
}
