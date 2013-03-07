using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    class ParticleTemplate
    {
        public Texture2D texture { get; private set; }

        public float velocityAdd { get; private set; }

        public TimeSpan lifeTime { get; private set; }

        public float initialRotation { get; private set; }
        public float rotationAdd { get; private set; }

        public float initialSize { get; private set; }
        public float sizeAdd { get; private set; }

        //TODO: Load these from file and get from resource manager?
        public ParticleTemplate(string textureID, float velocityAdd, TimeSpan lifeTime, float initialRotation, float rotationAdd, float initialSize, float sizeAdd)
        {
            texture = ResourceManager.getInstance().getTexture(textureID);
            this.velocityAdd = velocityAdd;
            this.lifeTime = lifeTime;
            this.initialRotation = initialRotation;
            this.rotationAdd = rotationAdd;
            this.initialSize = initialSize;
            this.sizeAdd = sizeAdd;
        }
    }
}
