using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace X2Game
{
    class Projectile : GameObject
    {
        private Texture2D texture;
        public Projectile(String textureID)
        {
            texture = ResourceManager.GetTexture(textureID);
        }

        public override void Update(TimeSpan delta, KeyboardState? keyboard, MouseState? mouse)
        {
            Position += Velocity;
        }

        public override bool CollidesWith(GameObject other)
        {
            //Projectiles normally do not collide with other projectiles... we might want to change this
            if(other is Projectile || other == this) return false;

            return other.hitBox.Intersects(hitBox);
        }
    }
}
