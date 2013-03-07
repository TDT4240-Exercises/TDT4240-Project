using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    class Particle : GameObject
    {
        private ParticleTemplate template;
        private TimeSpan timeRemaining;
        private float rotation;
        private float size;

        public bool isDestroyed;

        private Color tint { get; set; }

        private Rectangle renderArea
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)(template.texture.Width * size), (int)(template.texture.Height * size)); //XNA does not support float rectangles natively
            }
        }

        public Particle(Vector2 initialPosition, ParticleTemplate template)
        {
            this.template = template;
            position = initialPosition;
            rotation = template.initialRotation;
            size = template.initialSize;
            timeRemaining = template.lifeTime;
        }

        public override void update(TimeSpan delta)
        {
            //Do we still exist?
            if (isDestroyed) return;

            //Update rotation
            rotation += template.rotationAdd;

            //Update position
            position += velocity;

            //Update velocity
            velocity.X += template.velocityAdd;     //TODO: add velocity based on rotation?
            velocity.Y += template.velocityAdd;

            //Update size
            size += template.sizeAdd;
            if (size <= 0) isDestroyed = true;

            //Has this particle expired and needs to be removed?
            if (timeRemaining != TimeSpan.MaxValue)
            {
                timeRemaining -= delta;
                if (timeRemaining.Ticks <= 0) isDestroyed = true;
            }

        }

        public override void render(SpriteBatch spriteBatch)
        {
            //Do we still exist?
            if (isDestroyed) return;

            spriteBatch.Draw(template.texture, renderArea, null, tint, rotation, new Vector2(), SpriteEffects.None, 0);
        }
    }
}
