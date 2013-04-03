﻿using System;
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
        private static Dictionary<String, Texture2D> _loadedTextures;
        private static Dictionary<String, ParticleTemplate> _loadedParticles;
        private static Dictionary<String, TileType> _loadedTiles;

        private static GraphicsDevice _device;
        public static string ContentFolder { get; private set; }
        public static Texture2D InvalidTexture { get; private set; }
		private static SpriteFont _debugFont;

        public static void FreeResources()
        {
            //Unload textures
            foreach (Texture2D texture in _loadedTextures.Values)
            {
                texture.Dispose();
            }
            _loadedTextures.Clear();
        }

        public static ParticleTemplate GetParticleTemplate(string particleID)
        {
            if (!_loadedParticles.ContainsKey(particleID))
            {
                try
                {
                    _loadedParticles[particleID] = new ParticleTemplate(ContentFolder + particleID);
                }
                catch
                {
                    Logger.Log("Unable to load ParticleTemplate: " + particleID, LogLevel.Warning);
                    _loadedParticles[particleID] = null;
                }
                
            }

            return _loadedParticles[particleID];
        }


        
        /// <summary>
        /// Must be called befure using the ResourceManager to initialize the location for the graphics files and which
        /// GraphicsDevice to load the textures for.

        /// </summary>
        /// <param name="device">The GraphicsDevice to use when loading textures</param>
        /// <param name="contentFolder">The location of the game assets</param>
        public static void Initialize(GraphicsDevice device, string contentFolder)
        {
            _device = device;
            _loadedTextures = new Dictionary<String, Texture2D>();
            _loadedParticles = new Dictionary<String, ParticleTemplate>();
            _loadedTiles = new Dictionary<String, TileType>();
            ContentFolder = contentFolder;

            //Default white texture
            InvalidTexture = new Texture2D(_device, 64, 64);
            Color[] data = new Color[InvalidTexture.Width*InvalidTexture.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            InvalidTexture.SetData(data);
        }


		public static void LoadDebugFont(ContentManager Content)
		{
            _debugFont = Content.Load<SpriteFont>("DebugFont");
		}

		public static SpriteFont GetDebugFont()
		{
			return _debugFont;
		}

        /**
         * Retrieves the custom Texture2D object or loads it into memory if not already loaded.
         */
        public static Texture2D GetTexture(String textureID)
        {
            if (!_loadedTextures.ContainsKey(textureID))
            {
                Texture2D texture;

                try
                {
                    using (FileStream fileStream = new FileStream(ContentFolder + textureID, FileMode.Open))
                    {
                        texture = Texture2D.FromStream(_device, fileStream);

                        //Replace the color BLACK with 100% transparency
//                        Color[] bits = new Color[texture.Width * texture.Height];
//                        texture.GetData(bits);
//                        for (int i = 0; i < bits.Length; i++)
//                        {  
//                           if((bits[i].PackedValue & 0xFFFFFF) == 0) bits[i] = Color.FromNonPremultiplied(0,0,0,0);
//                        }
//                        texture.SetData(bits);
                        Logger.Log("Texture \"" + textureID + "\" loaded.", LogLevel.Info);
                    }
                }
                catch(Exception ex)
                {
                    Logger.Log("Cannot load '" + textureID + "' file! - " + ex.Message, LogLevel.Warning);
                    texture = InvalidTexture;
                }

                _loadedTextures.Add(textureID, texture);
                texture.Name = textureID;
            }

            return _loadedTextures[textureID];
        }

        public static TileType GetTile(string tileID)
        {
            if (!_loadedTiles.ContainsKey(tileID))
            {
                _loadedTiles[tileID] = new TileType(ContentFolder + tileID); 
            }

            return _loadedTiles[tileID];
        }

    }
}