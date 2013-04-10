﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace X2Game
{
    static class ParticleEngine
    {
        public static uint maxParticles = 10000;
        private static readonly LinkedList<Particle> particleList = new LinkedList<Particle>();
        private static readonly LinkedList<Particle> spawnList = new LinkedList<Particle>();

        public static void Update(GameTime delta)
        {
            //Add new particles to the current list
            foreach (Particle particle in spawnList)
            {
                particleList.AddLast(particle);
            }
            spawnList.Clear();

            //Single threaded update
            if (particleList.Count < 100)
            {
                LinkedListNode<Particle> node = particleList.First;
                while (node != null)
                {
                    //Update particle
                    node.Value.Update(delta, null, null);

                    //Has this particle been destroyed?
                    if (node.Value.isDestroyed)
                    {
                        LinkedListNode<Particle> remove = node;
                        node = node.Next;
                        particleList.Remove(remove);
                        continue;
                    }

                    //Get next particle to update
                    node = node.Next;
                }
            }

            //Parallel update
            else
            {
                Parallel.ForEach(particleList, particle => particle.Update(delta, null, null));

                //Remove destroyed particles
                for (LinkedListNode<Particle> node = particleList.First; node != null; )
                {
                    LinkedListNode<Particle> next = node.Next;
                    if (node.Value.isDestroyed) particleList.Remove(node);
                    node = next;
                }
            }

        }

        public static void Render(RenderEngine renderEngine)
        {
            foreach (Particle particle in particleList)
            {
                renderEngine.Render(particle);
            }
        }

        public static void SpawnParticle(Vector2 position, ParticleTemplate template)
        {
            //Don't spawn more particles than the set limit
            if (Count() >= maxParticles || template == null) return;

            Logger.Log("particle spawned: " + template, LogLevel.Info);
            //Spawn it and add it last in our list
            spawnList.AddLast(new Particle(position, template));
        }

        public static void SpawnProjectile(Entity shooter, ParticleTemplate template)
        {
            //Don't spawn more particles than the set limit
            if (Count() >= maxParticles || template == null) return;

            Logger.Log("projectile spawned: " + template, LogLevel.Info);

            Particle projectile = new Particle(shooter.Position, template);
            projectile.Rotation = shooter.Rotation;
            projectile.Speed = 200;

            //Spawn it and add it last in our list
            spawnList.AddLast(projectile);
        }

        public static int Count()
        {
            return particleList.Count + spawnList.Count;
        }

        public static void Clear()
        {
            particleList.Clear();
        }

    }
}
