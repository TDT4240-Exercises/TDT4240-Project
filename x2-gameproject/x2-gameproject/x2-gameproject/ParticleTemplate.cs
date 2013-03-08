
namespace X2Game
{
    enum ParticleValues
    {
        InitialSize,
        SizeAdd,
        InitialRotation,
        RotationAdd,
        Lifetime,
        Texture,
        VelocityAdd,
        InitialAlpha,
        AlphaAdd,
        SpawnParticleOnEnd
    }

    class ParticleTemplate : GenericDataStructure
    {
        public readonly string particleID;

        public ParticleTemplate(string filePath)
            : base(filePath, typeof(ParticleValues))
        {
            particleID = filePath;
            SetDefaultValue(ParticleValues.InitialSize, 1.0f);
            SetDefaultValue(ParticleValues.Lifetime, 3.0f);
            SetDefaultValue(ParticleValues.Texture, ResourceManager.getTexture("INVALID_TEXTURE"));
        }
    }

}
