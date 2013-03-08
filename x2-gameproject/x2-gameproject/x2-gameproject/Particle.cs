using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private float rotation;
        private float size;
        private float alpha;

        public bool isDestroyed;

        private Rectangle renderArea
        {
            get
            {
                Texture2D texture = template.getValue<Texture2D>(ParticleValues.TEXTURE);
                return new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * size), (int)(texture.Height * size)); //XNA does not support float rectangles natively
            }
        }

        //These two are cached for performance reasons
        private Vector2 centre;
        private Texture2D texture;

        public Particle(Vector2 initialPosition, ParticleTemplate template)
        {
            this.template = template;
            position = initialPosition;
            rotation = template.getValue<float>(ParticleValues.INITIAL_ROTATION);
            size = template.getValue<float>(ParticleValues.INITIAL_SIZE);
            secondsRemaining = template.getValue<float>(ParticleValues.LIFETIME);
            texture = texture = template.getValue<Texture2D>(ParticleValues.TEXTURE);
            centre = new Vector2(texture.Width / 2, texture.Height / 2);
            alpha = 1.0f - template.getValue<float>(ParticleValues.INITIAL_ALPHA);
        }

        public void destroy()
        {
            if (isDestroyed) return;
            isDestroyed = true;

            //Spawn other particles on death?
            ParticleTemplate spawn = template.getValue<ParticleTemplate>(ParticleValues.SPAWN_PARTICLE_ON_END);
            if (spawn != null)
            {
                ParticleEngine.spawnParticle(position, spawn);
            }
        }

        public override void update(TimeSpan delta)
        {
            //Do we still exist?
            if (isDestroyed) return;

            float timeUnit = delta.Ticks/(float)TimeSpan.TicksPerSecond;

            //Update rotation
            rotation += template.getValue<float>(ParticleValues.ROTATION_ADD) * timeUnit;

            //Update position
            position += velocity;

            //Update velocity
            velocity.X += template.getValue<float>(ParticleValues.VELOCITY_ADD) * timeUnit;     //TODO: add velocity based on rotation?
            velocity.Y += template.getValue<float>(ParticleValues.VELOCITY_ADD) * timeUnit;

            //Update alpha
            alpha -= template.getValue<float>(ParticleValues.ALPHA_ADD) * timeUnit;
            if (alpha <= 0) destroy();

            //Update size
            size += template.getValue<float>(ParticleValues.SIZE_ADD) * timeUnit;
            if (size <= 0) destroy();

            //Has this particle expired and needs to be removed?
            if (!float.IsInfinity(secondsRemaining))
            {
                secondsRemaining -= (delta.Ticks/(float)TimeSpan.TicksPerSecond);
                if (secondsRemaining <= 0) destroy();
            }

        }

        public override void render(SpriteBatch spriteBatch)
        {
            //Do we still exist?
            if (isDestroyed) return;

            spriteBatch.Draw(texture, renderArea, null, Color.White * alpha, rotation, centre, SpriteEffects.None, 0);
        }
    }
}
