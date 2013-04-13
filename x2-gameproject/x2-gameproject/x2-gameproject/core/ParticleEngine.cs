using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace X2Game
{
    static class ParticleEngine
    {
        public static uint MaxParticles = 7000;
        private static readonly LinkedList<Particle> ParticleList = new LinkedList<Particle>();
        private static readonly LinkedList<Particle> SpawnList = new LinkedList<Particle>();

        public static void Update(GameTime delta, List<Entity> entities, TileMap tileMap)
        {
            //Add new particles to the current list
            foreach (Particle particle in SpawnList)
            {
                ParticleList.AddLast(particle);
            }
            SpawnList.Clear();

            //Single threaded update
            if (ParticleList.Count < 1000)
            {
                LinkedListNode<Particle> node = ParticleList.First;
                while (node != null)
                {
                    //Update particle
                    node.Value.Update(delta, entities, tileMap);

                    //Has this particle been destroyed?
                    if (node.Value.IsDestroyed)
                    {
                        LinkedListNode<Particle> remove = node;
                        node = node.Next;
                        ParticleList.Remove(remove);
                        continue;
                    }

                    //Get next particle to update
                    node = node.Next;
                }
            }

            //Parallel update
            else
            {
                Parallel.ForEach(ParticleList, particle => particle.Update(delta, null, null));

                //Remove destroyed particles
                for (LinkedListNode<Particle> node = ParticleList.First; node != null; )
                {
                    LinkedListNode<Particle> next = node.Next;
                    if (node.Value.IsDestroyed) ParticleList.Remove(node);
                    node = next;
                }
            }

        }

        public static void Render(RenderEngine renderEngine)
        {
            foreach (Particle particle in ParticleList)
            {
                renderEngine.Render(particle);
            }
        }

        public static void SpawnParticle(Vector2 position, ParticleTemplate template, bool centre = false)
        {
            //Don't spawn more particles than the set limit
            if (Count() >= MaxParticles || template == null) return;

            //Spawn it and add it last in our list
            SpawnList.AddLast(new Particle(position, template));
        }

        public static void SpawnProjectile(Entity shooter, ParticleTemplate template)
        {
            //Don't spawn more particles than the set limit
            if (Count() >= MaxParticles || template == null) return;

            Particle projectile = new Particle(shooter.Position, template);
            projectile.Rotation = shooter.Rotation;
            projectile.Speed = template.GetValue<float>(ParticleValues.Speed);
            projectile.Team = shooter.Team;

            //Spawn projectile from gun barrel and not unit origin
            projectile.X += (float)Math.Cos(shooter.Rotation) * shooter.Width/2;
            projectile.Y += (float)Math.Sin(shooter.Rotation) * shooter.Width/2;

            //Spawn it and add it last in our list
            SpawnList.AddLast(projectile);
        }

        public static int Count()
        {
            return ParticleList.Count + SpawnList.Count;
        }

        public static void Clear()
        {
            ParticleList.Clear();
        }

    }
}
