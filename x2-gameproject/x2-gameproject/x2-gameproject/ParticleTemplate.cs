using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace X2Game
{
    class ParticleTemplate
    {
        private Dictionary<string, Object> values;

        public Texture2D texture { get; private set; }

        public float velocityAdd { get; private set; }

        public TimeSpan lifeTime { get; private set; }

        public float initialRotation { get; private set; }
        public float rotationAdd { get; private set; }

        public float initialSize { get; private set; }
        public float sizeAdd { get; private set; }

        //TODO: Load these from file and get from resource manager?
        public ParticleTemplate(string textureID, float velocityAdd, TimeSpan lifeTime, float initialRotation, float rotationAdd, float initialSize, float sizeAdd)
        {
            texture = ResourceManager.getTexture(textureID);
            this.velocityAdd = velocityAdd;
            this.lifeTime = lifeTime;
            this.initialRotation = initialRotation;
            this.rotationAdd = rotationAdd;
            this.initialSize = initialSize;
            this.sizeAdd = sizeAdd;
        }

        public T getValue<T>(string valueID)
        {
            return (T) values[valueID];
        }

        private Object parseType(string typeID, string value)
        {
            switch (typeID)
            {
                case "float":
                    return float.Parse(value);
                case "integer":
                    return int.Parse(value);
                case "texture":
                    return ResourceManager.getTexture(value);
            }

            return value;
        }

        public ParticleTemplate(string filePath)
        {
            // Create an XmlReader
            using (XmlReader xml = XmlReader.Create(filePath))
            {
                // Parse the file and display each of the nodes.
                while (xml.Read())
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        values.Add(xml.Name, parseType(xml.GetAttribute("type"), xml.Value));
                    }
                }
            }
            
        }
    }
}
