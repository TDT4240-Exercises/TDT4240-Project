using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace X2Game
{
    class Projectile : GameObject
    {
        private Texture2D texture;
        public Projectile(String textureID)
        {
            texture = ResourceManager.GetTexture(textureID);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitBox, Color.White);
        }

        public override void Update(TimeSpan delta)
        {
            position += velocity;
        }

        public override bool CollidesWith(GameObject other)
        {
            //Projectiles normally do not collide with other projectiles... we might want to change this
            if(other is Projectile || other == this) return false;

            return other.hitBox.Intersects(hitBox);
        }
    }
}
