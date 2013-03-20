using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace X2Game
{
    public enum EntityController
    {
        None,              //No controller
        DefaultAI,         //Default AI
        KamikazeAI,        //Rush towards opponent
        SmartAI,           //Sneaky AI hides behind walls, etc.
        Player             //This entity is controlled by a player
    }

    /// <summary>
    /// An Entity is a unit that is either controlled by an AI or a player
    /// </summary>
    class Entity : GameObject
    {
        public const float VELOCITY_FALLOFF = 0.95f;
        private Texture2D texture;
        private EntityController controller;

        private float health; //TODO: stats etc.

        /// <summary>
        /// Constructor for an Entity
        /// </summary>
        /// <param name="textureID">The name of the texture file to use</param>
        /// <param name="setController">Is this entity controlled by an AI or a player?</param>
        public Entity(String textureID, EntityController setController)
        {
            texture = ResourceManager.GetTexture(textureID);
            controller = setController;
            width = texture.Width;
            height = texture.Height;
        }

        public override void Update(TimeSpan delta)
        {
            position += velocity;
            velocity *= VELOCITY_FALLOFF;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, hitBox, Color.White);
            spriteBatch.Draw (
                texture, 
                new Vector2 (GetX (), GetY ()), 
                null, 
                Color.White, 
                rotation, 
                new Vector2 (width / 2, height / 2), 
                0.2f, 
                SpriteEffects.None, 
                1.0f
            );
        }

        public EntityController GetController()
        {
            return controller;
        }

    }
}
