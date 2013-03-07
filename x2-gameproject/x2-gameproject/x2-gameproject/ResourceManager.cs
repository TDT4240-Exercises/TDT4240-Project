using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace X2Game
{
    /**
     * Atomic class to handle assets and resources
     */
    static class ResourceManager
    {
        private static Dictionary<String, Texture2D> loadedTextures;
        private static GraphicsDevice device;
        private static string graphicsFolder;

        public static void freeResources()
        {
            //Unload textures
            foreach (Texture2D texture in loadedTextures.Values)
            {
                texture.Dispose();
            }
            loadedTextures.Clear();
        }


        /**
         * Must be called befure using the ResourceManager to initialize the location for the graphics files and which
         * GraphicsDevice to load the textures for.
         */
        public static void initialize(GraphicsDevice device, string graphicsFolder)
        {
            ResourceManager.device = device;
            loadedTextures = new Dictionary<String, Texture2D>();
            ResourceManager.graphicsFolder = graphicsFolder;
			debugTexture = new Texture2D (device, 1, 1);
			debugTexture.SetData (new[] {Color.White});
        }

		public static Texture2D GetDebugTexture()
		{
			return debugTexture;
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
                    using (FileStream fileStream = new FileStream("graphics/" + textureID, FileMode.Open))
                    {
                        texture = Texture2D.FromStream(device, fileStream);
                    }
                }
                catch
                {
                    throw new System.IO.FileLoadException("Cannot load '" + textureID + "' file!");
                }

                loadedTextures.Add(textureID, texture);
            }

            return loadedTextures[textureID];
        }

    }
}
