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
        SpawnParticleOnEnd,
        CanCollide,
        RotationIndependentVelocity,
        SoundEffectOnSpawn
    }

    [ImmutableObject(true)]
    class ParticleTemplate : GenericDataStructure
    {
        public readonly string particleID;

        public ParticleTemplate(string filePath)
            : base(filePath, typeof(ParticleValues))
        {
            particleID = filePath;
            SetDefaultValue(ParticleValues.InitialSize, 1.0f);
            SetDefaultValue(ParticleValues.LifeTime, 3.0f);
            SetDefaultValue(ParticleValues.Texture, "MISSING_TEXTURE");
        }
    }

}
