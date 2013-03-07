using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace X2Game
{
    enum ParticleValues
    {
        INITIAL_SIZE,
        SIZE_ADD,
        INITIAL_ROTATION,
        ROTATION_ADD,
        LIFE_TIME,
        TEXTURE,
        VELOCITY_ADD,
        INITIAL_ALPHA,
        ALPHA_ADD
    }

    class ParticleTemplate : GenericDataStructure
    {
        public ParticleTemplate(string filePath)
            : base(filePath, typeof(ParticleValues))
        {
            setDefaultValue(ParticleValues.INITIAL_SIZE, 1.0f);
            setDefaultValue(ParticleValues.SIZE_ADD, 0.0f);
            setDefaultValue(ParticleValues.INITIAL_ROTATION, 0.0f);
            setDefaultValue(ParticleValues.ROTATION_ADD, 0.0f);
            setDefaultValue(ParticleValues.VELOCITY_ADD, 0.0f);
            setDefaultValue(ParticleValues.INITIAL_ALPHA, 0.0f);
            setDefaultValue(ParticleValues.ALPHA_ADD, 0.0f);
            setDefaultValue(ParticleValues.LIFE_TIME, TimeSpan.TicksPerSecond * 3L);
            setDefaultValue(ParticleValues.TEXTURE, ResourceManager.getTexture("INVALID_TEXTURE"));
        }
    }

}
