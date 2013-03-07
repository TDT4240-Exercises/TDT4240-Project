using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace X2Game
{
    static class ParticleEngine
    {
        public static uint maxParticles = 1000;
        private static LinkedList<Particle> particleList = new LinkedList<Particle>();

        public static void update(TimeSpan delta)
        {
            for(LinkedListNode<Particle> node = particleList.First; node != null; node = node.Next)
            {
                node.Value.update(delta);

                //Has this particle been destroyed?
                if (node.Value.isDestroyed) particleList.Remove(node);
            }
        }

        public static void render(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particleList) p.render(spriteBatch);
        }

        public static void spawnParticle(Vector2 position, ParticleTemplate template)
        {
            //Don't spawn more particles than the set limit
            if (particleList.Count >= maxParticles) return;

            //Spawn it and add it last in our list
            particleList.AddLast(new Particle(position, template));
        }

        public static void clear()
        {
            particleList.Clear();
        }

    }
}
