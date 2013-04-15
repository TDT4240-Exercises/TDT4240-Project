using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly Random _rand = new Random();
        private readonly ParticleTemplate _template;
        private float _secondsRemaining;
        internal float Size;
        internal float Alpha;
        public float Speed;
        private readonly HashSet<Entity> _areaOfEffect;
        private bool _aoeFinished;
        private readonly Entity _owner;

        public Particle(Vector2 initialPosition, ParticleTemplate template, Entity owner)
        {
            _template = template;
            _owner = owner;

            //Rotation
            Rotation = template.GetValue<float>(ParticleValues.InitialRotation);
            if ((int)Rotation == -1) Rotation = (float)_rand.NextDouble();

            Size = template.GetValue<float>(ParticleValues.InitialSize);
            _secondsRemaining = template.GetValue<float>(ParticleValues.LifeTime);
            Texture = ResourceManager.GetTexture("particles/" + template.GetValue<string>(ParticleValues.Texture));
            Alpha = 1.0f - template.GetValue<float>(ParticleValues.InitialAlpha);
            IsCollidable = template.GetValue<bool>(ParticleValues.CanCollide);
            Position = initialPosition;

            //Sound effect
            string spawnSound = template.GetValue<string>(ParticleValues.SoundEffectOnSpawn);
            if (spawnSound != null) ResourceManager.PlaySoundEffect(spawnSound);

            //camera shake effect
            Camera.shake += template.GetValue<int>(ParticleValues.CameraShake);

            if (template.GetValue<bool>(ParticleValues.AreaOfEffect))
            {
                _areaOfEffect = new HashSet<Entity>();
            }
        }

        public override void Destroy()
        {
            if (IsDestroyed) return;
            IsDestroyed = true;

            //Spawn other particles on death?
            ParticleTemplate spawn = _template.GetValue<ParticleTemplate>(ParticleValues.SpawnParticleOnEnd);
            if (spawn != null)
            {
                ParticleEngine.SpawnParticle(Position, spawn, false, _owner);
            }
        }

        public override void Update(GameTime delta, KeyboardState? keyboard, MouseState? mouse)
        {

            //Has this particle expired and needs to be removed?
            if (!float.IsInfinity(_secondsRemaining))
            {
                _secondsRemaining -= (delta.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond);
                if (_secondsRemaining <= 0)
                {
                    Destroy();
                    return;
                }
            }

            //Update alpha
            Alpha -= _template.GetValue<float>(ParticleValues.AlphaAdd);
            if (Alpha <= 0)
            {
                Destroy();
                return;
            }
            if (Alpha > 1.0f) Alpha = 1;

            //Update size
            Size += _template.GetValue<float>(ParticleValues.SizeAdd);
            if (Size <= 0)
            {
                Destroy();
                return;
            }
            Width = (int) (Texture.Width * Size);
            Height = (int) (Texture.Height * Size);

            //Update rotation
            Rotation += _template.GetValue<float>(ParticleValues.RotationAdd);

            //Update velocity depending on rotation and speed
            if (!_template.GetValue<bool>(ParticleValues.RotationIndependentVelocity))
            {
                VelX = (float)Math.Cos(Rotation) * Speed;
                VelY = (float)Math.Sin(Rotation) * Speed;
            }
            else
            {
               // Velocity.X = Speed;
               // Velocity.Y = Speed;
                //TODO: not implemented
            }

            //Update position
            Position += Velocity;
        }

        public void Update(GameTime delta, List<Entity> entities, TileMap tileMap)
        {
            if (IsCollidable)
            {
                //Area of effect!
                if (_areaOfEffect != null)
                {
                    //Do we hit an gameObject?
                    foreach (Entity entity in entities.Where(entity => entity.IsCollidable && entity.HandleCollision(this, false) && entity.Team != Team && !_areaOfEffect.Contains(entity)))
                    {
                        entity.Damage(_template.GetValue<float>(ParticleValues.Damage), _owner);
                        _areaOfEffect.Add(entity);
                    }
                }

                //Single target only
                else
                {
                    //Did we hit a wall?
                    Point? tileCollision = tileMap.GetCollidedTile(this);
                    if (tileCollision.HasValue)
                    {
                        tileMap.DestroyTile(tileCollision.Value.X, tileCollision.Value.Y, _template.GetValue<float>(ParticleValues.Damage));
                        Destroy();
                        return;
                    }

                    //Do we hit an gameObject?
                    foreach (Entity entity in entities.Where(entity => entity.IsCollidable && entity.HandleCollision(this, false) && entity.Team != Team))
                    {
                        entity.Damage(_template.GetValue<float>(ParticleValues.Damage), _owner);
                        Destroy();
                        return;
                    }
                }

            }


            Update(delta, (KeyboardState?)null, null);

            //Make AOE apply damage to tiles (but only once)
            if (IsCollidable && _areaOfEffect != null && !_aoeFinished)
            {
                _aoeFinished = true;
                Vector2 bounds = new Vector2(Width, Height);
                Point start = tileMap.GetSquareAtPixel(Position - bounds/2);
                Point end = tileMap.GetSquareAtPixel(Position + bounds/2);
                for (int x = start.X; x <= end.X; ++x)
                {
                    for (int y = start.Y; y <= end.Y; ++y)
                    {
                        tileMap.DestroyTile(x, y, _template.GetValue<float>(ParticleValues.Damage));
                    }
                }
            }
        }
    }
}
