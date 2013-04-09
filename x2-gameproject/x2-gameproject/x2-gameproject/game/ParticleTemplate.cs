using System.ComponentModel;

namespace X2Game
{
    public enum ParticleValues
    {
        InitialSize,
        SizeAdd,
        InitialRotation,
        RotationAdd,
        LifeTime,
        Texture,
        VelocityAdd,
        InitialAlpha,
        AlphaAdd,
        SpawnParticleOnEnd
    }

    [ImmutableObject(true)]
    class ParticleTemplate : GenericDataStructure
    {
        public readonly string particleID;

        public ParticleTemplate(string filePath)
            : base(filePath, typeof(ParticleValues))
        {
            ParticleValues.LifeTime.GetType();
            particleID = filePath;
            SetDefaultValue(ParticleValues.InitialSize, 1.0f);
            SetDefaultValue(ParticleValues.LifeTime, 3.0f);
            SetDefaultValue(ParticleValues.Texture, ResourceManager.InvalidTexture);
        }
    }

}
