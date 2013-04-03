using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        private EntityController controller;

        protected float Health;
        protected float TurnRate;

        /// <summary>
        /// Constructor for an Entity
        /// </summary>
        /// <param name="textureID">The name of the texture file to use</param>
        /// <param name="setController">Is this entity controlled by an AI or a player?</param>
        public Entity(String textureID, EntityController setController)
        {
            Texture = ResourceManager.GetTexture(textureID);
            controller = setController;
            Width = Texture.Width;
            Height = Texture.Height;
        }

        public override void Update(TimeSpan delta, KeyboardState? keyboard, MouseState? mouse)
        {
            Position += Velocity;
            Velocity *= VELOCITY_FALLOFF;
        }

        public EntityController GetController()
        {
            return controller;
        }

    }
}
