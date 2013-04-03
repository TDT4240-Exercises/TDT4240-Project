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
        
        public bool isDestroyed;

        //These two are cached for performance reasons
        private Vector2 centre;
        private Texture2D texture;

        public Particle(Vector2 initialPosition, ParticleTemplate template)
        {
            this.template = template;
            Position = initialPosition;
            Rotation = template.GetValue<float>(ParticleValues.InitialRotation);
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
                ParticleEngine.SpawnParticle(Position, spawn);
            }
        }

        public override void Update(TimeSpan delta, KeyboardState? keyboard, MouseState? mouse)
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
            Rotation += template.GetValue<float>(ParticleValues.RotationAdd) * timeUnit;

            //Update position
            Position += Velocity;

            //Update velocity
            Velocity.X += template.GetValue<float>(ParticleValues.VelocityAdd) * timeUnit;     //TODO: add velocity based on rotation?
            Velocity.Y += template.GetValue<float>(ParticleValues.VelocityAdd) * timeUnit;
        }

        /*public override void Render(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(texture, Position, null, Color.White * alpha, Rotation, centre, size, SpriteEffects.None, 0);
        }*/
    }
}
