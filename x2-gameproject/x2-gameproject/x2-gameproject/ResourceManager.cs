using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace X2Game
{
    /**
     * Atomic class to handle assets and resources
     */
    static class ResourceManager
    {
        private static Dictionary<String, Texture2D> loadedTextures;
        private static Dictionary<String, ParticleTemplate> loadedParticles;

        private static GraphicsDevice device;
        private static string contentFolder;
        private static Texture2D debugTexture;
		private static SpriteFont debugFont;

        public static void freeResources()
        {
            //Unload textures
            foreach (Texture2D texture in loadedTextures.Values)
            {
                texture.Dispose();
            }
            loadedTextures.Clear();
        }

        public static ParticleTemplate getParticleTemplate(string particleID)
        {
            if (!loadedParticles.ContainsKey(particleID))
            {
                try
                {
                    loadedParticles[particleID] = new ParticleTemplate(contentFolder + particleID);
                }
                catch
                {
                    loadedParticles[particleID] = null;
                }
                
            }

            return loadedParticles[particleID];
        }


        
        /// <summary>
        /// Must be called befure using the ResourceManager to initialize the location for the graphics files and which
        /// GraphicsDevice to load the textures for.

        /// </summary>
        /// <param name="device">The GraphicsDevice to use when loading textures</param>
        /// <param name="contentFolder">The location of the game assets</param>
        public static void initialize(GraphicsDevice device, string contentFolder)
        {
            ResourceManager.device = device;
            loadedTextures = new Dictionary<String, Texture2D>();
            loadedParticles = new Dictionary<String, ParticleTemplate>();
            ResourceManager.contentFolder = contentFolder;

			debugTexture = new Texture2D (device, 1, 1);
			debugTexture.SetData (new[] {Color.White});
        }

		public static Texture2D GetDebugTexture()
		{
			return debugTexture;
		}

		public static void LoadDebugFont(ContentManager Content)
		{
            debugFont = Content.Load<SpriteFont>("DebugFont");
		}

		public static SpriteFont GetDebugFont()
		{
			return debugFont;
		}

        /**
         * Retrieves the custom Texture2D object or loads it into memory if not already loaded.
         */
        public static Texture2D getTexture(String textureID)
        {
            if (!loadedTextures.ContainsKey(textureID))
            {
                Texture2D texture;

                try
                {
                    using (FileStream fileStream = new FileStream(contentFolder + textureID, FileMode.Open))
                    {
                        texture = Texture2D.FromStream(device, fileStream);

                        //Replace the color BLACK with 100% transparency - TODO: this might be very slow
//                        Color[] bits = new Color[texture.Width * texture.Height];
//                        texture.GetData(bits);
//                        for (int i = 0; i < bits.Length; i++)
//                        {  
//                           if((bits[i].PackedValue & 0xFFFFFF) == 0) bits[i] = Color.FromNonPremultiplied(0,0,0,0);
//                        }
//                        texture.SetData(bits);

						Console.WriteLine("Texture \"" + textureID + "\" loaded.");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Cannot load '" + textureID + "' file! - " + ex.Message);
                    texture = new Texture2D(device, 64, 64);
                }

                loadedTextures.Add(textureID, texture);
                texture.Name = textureID;
            }

            return loadedTextures[textureID];
        }

    }
}
