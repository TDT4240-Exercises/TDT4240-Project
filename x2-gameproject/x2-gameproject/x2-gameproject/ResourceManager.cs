﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace X2Game
{
    /**
     * Atomic class to handle assets and resources
     */
    class ResourceManager
    {
        private static ResourceManager instance;
        private Dictionary<String, Texture2D> loadedTextures;
        private static GraphicsDevice device;
        private String graphicsFolder;

        private ResourceManager(String graphicsFolder)
        {
            loadedTextures = new Dictionary<String, Texture2D>();
            this.graphicsFolder = graphicsFolder;
        }

        public ~ResourceManager()
        {
            //Deconstructor... unload textures
            foreach (Texture2D texture in loadedTextures.Values)
            {
                texture.Dispose();
            }
        }

        /**
         * Retrieves the ResourceManager singleton
         */
        public static ResourceManager getInstance()
        {
            if (instance == null) throw new Exception("ResourceManager was used before it was properly initialized!");
            return instance;
        }

        /**
         * Must be called befure using the ResourceManager to initialize the location for the graphics files and which
         * GraphicsDevice to load the textures for.
         */
        public static void initialize(GraphicsDevice device, String graphicsFolder)
        {
            ResourceManager.device = device;
            instance = new ResourceManager(graphicsFolder);
        }

        /**
         * Retrieves the custom Texture2D object or loads it into memory if not already loaded.
         */
        public Texture2D getTexture(String textureID)
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
