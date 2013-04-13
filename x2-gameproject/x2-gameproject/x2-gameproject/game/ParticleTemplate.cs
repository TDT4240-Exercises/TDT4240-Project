using System.ComponentModel;

namespace X2Game
{
    public enum ParticleValues
    {
        InitialSize,                    //Starting size (float)
        SizeAdd,                        //Size change every frame
        InitialRotation,                //Starting rotation (-1 for random)
        RotationAdd,                    //Rotation add every frame
        LifeTime,                       //Seconds before it dies
        Texture,                        //Which texture to use (string)
        InitialAlpha,                   //Staring transperancy
        AlphaAdd,                       //Transperancy change every frame
        SpawnParticleOnEnd,             //Which particle to spawn when this ends
        CanCollide,                     //Can collide with enemy entities?
        RotationIndependentVelocity,    //Does not rotate in velocity direction
        SoundEffectOnSpawn,             //Play this sound when particle spawns
        AttackCooldown,                 //Seconds between each attack
        Damage,                         //How much damage it deals
        Speed,                          //Maximum speed
        CameraShake,                    //Cause camera shake?
        AreaOfEffect                    //Can hit multiple enemies?
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
            SetDefaultValue(ParticleValues.AttackCooldown, 1.0f);
            SetDefaultValue(ParticleValues.Speed, 10.0f);
            SetDefaultValue(ParticleValues.Texture, "MISSING_TEXTURE");
        }
    }

}
