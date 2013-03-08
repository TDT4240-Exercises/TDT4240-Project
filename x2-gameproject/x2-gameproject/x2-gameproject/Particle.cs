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

        //These two are cached for performance reasons
        private Vector2 centre;
        private Texture2D texture;
        private Rectangle renderArea;

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

            renderArea = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * size), (int)(texture.Height * size));
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
            float timeUnit = delta.Ticks/(float)TimeSpan.TicksPerSecond;

            //Has this particle expired and needs to be removed?
            if (!float.IsInfinity(secondsRemaining))
            {
                secondsRemaining -= (delta.Ticks / (float)TimeSpan.TicksPerSecond);
                if (secondsRemaining <= 0)
                {
                    destroy();
                    return;
                }
            }

            //Update alpha
            alpha -= template.getValue<float>(ParticleValues.ALPHA_ADD) * timeUnit;
            if (alpha <= 0)
            {
                destroy();
                return;
            }

            //Update size
            size += template.getValue<float>(ParticleValues.SIZE_ADD) * timeUnit;
            if (size <= 0)
            {
                destroy();
                return;
            }

            //Update rotation
            rotation += template.getValue<float>(ParticleValues.ROTATION_ADD) * timeUnit;

            //Update position
            position += velocity;

            //Update velocity
            velocity.X += template.getValue<float>(ParticleValues.VELOCITY_ADD) * timeUnit;     //TODO: add velocity based on rotation?
            velocity.Y += template.getValue<float>(ParticleValues.VELOCITY_ADD) * timeUnit;

            //Update render area
            renderArea.X = (int)position.X;
            renderArea.Y = (int)position.Y;
            renderArea.Width = (int)(texture.Width*size);
            renderArea.Height = (int)(texture.Height*size);
        }

        public override void render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, renderArea, null, Color.White * alpha, rotation, centre, SpriteEffects.None, 0);
        }
    }
}
