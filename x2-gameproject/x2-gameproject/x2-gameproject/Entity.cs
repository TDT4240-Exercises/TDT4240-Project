﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace X2Game
{
    public enum EntityController
    {
        KAMIKAZE_AI,        //Rush towards opponent
        DEFAULT_AI,         //Default AI
        SMART_AI,           //Sneaky AI hides behind walls, etc.
        PLAYER              //This entity is controlled by a player
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
            texture = ResourceManager.getTexture(textureID);
            controller = setController;
        }

        public override void update(TimeSpan delta)
        {
            position += velocity;
            velocity *= VELOCITY_FALLOFF;
        }

        public override void render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitBox, Color.White);
        }

        public EntityController getController()
        {
            return controller;
        }

    }
}
