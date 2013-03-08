using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    abstract class GenericDataStructure
    {
        private HybridDictionary values;    //high performance, case insensitive, returns null for non-existing elements
        private Type enumType;

        /// <summary>
        /// All sub-classes must inherit and implement this constructor
        /// </summary>
        /// <param name="filePath">the XML file to parse and load data from</param>
        /// <param name="valueID">an enumeration of valid valueIDs we are allowed to parse from the XML</param>
        protected GenericDataStructure(string filePath, Type valueID)
        {
            enumType = valueID;
            values = new HybridDictionary(8, true);

            // Create an XmlReader
            using (XmlReader xml = XmlReader.Create(filePath))
            {

                // Parse the file and display each of the nodes.
                while (xml.Read())
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        if (xml.Depth == 0)
                        {
                            if (xml.Name != GetType().Name)
                            {
                                Console.WriteLine("Wrong XML type: " + filePath + " - " + xml.Name + " requires " + GetType().Name);
                                return;
                            }
                            continue;
                        }

                        //Is this a valid name?
                        if (!enumType.IsEnumDefined(xml.Name))
                        {
                            Console.WriteLine("Illegal parse: " + filePath + " - " + xml.Name + " is not a valid value");
                            continue;
                        }

                        //Load value!
                        values.Add(xml.Name, parseValue(xml));
                    }
                }
            }
        }

        private object parseValue(XmlReader xml)
        {
            string typeName = xml.GetAttribute("type");

            //Default type is string
            if (typeName == null) xml.ReadElementContentAsString();
            typeName = typeName.ToLower();
            
            //Else try to figure out what type this is
            switch (typeName)
            {
                case "int64":
                case "long":
                    return xml.ReadElementContentAsLong();

                case "single":
                case "float":
                    return xml.ReadElementContentAsFloat();

                case "int32":
                case "int":
                case "integer":
                    return xml.ReadElementContentAsInt();

                case "string":
                case "text":
                    return xml.ReadElementContentAsString();

                case "bool":
                case "boolean":
                    return xml.ReadElementContentAsBoolean();

                case "texture":
                case "texture2d":
                    return ResourceManager.getTexture(xml.ReadElementContentAsString());

                case "particle":
                    Console.WriteLine("Loaded particle!");
                    return ResourceManager.getParticleTemplate(xml.ReadElementContentAsString());

                default:
                    Console.WriteLine("Unknown data type parsed: " + typeName + " (treating it as string)");
                    return xml.ReadElementContentAsString();
            }
        }

        protected void setDefaultValue(Enum valueID, object value)
        {
            setDefaultValue(valueID.ToString(), value);
        }

        protected void setDefaultValue(string valueID, object value)
        {
            if (!enumType.IsEnumDefined(valueID)) throw new ArgumentException(valueID + " is not a valid enum type!");
            if (values.Contains(valueID)) return; //don't override explicit values
            values.Add(valueID, value);
        }

        public T getValue<T>(string valueID)
        {
            if (values[valueID] == null) return default(T);  //null, 0.0, "" or whatever

            return (T) values[valueID];
        }

        public T getValue<T>(Enum valueID)
        {
            return getValue<T>(valueID.ToString());
        }

        public void writeToFile(string fileUri)
        {

            using (XmlWriter xml = XmlWriter.Create(fileUri))
            {
                xml.WriteStartDocument();
                xml.WriteWhitespace("\n");

                xml.WriteStartElement(GetType().Name);
                xml.WriteWhitespace("\n");

                foreach (DictionaryEntry entry in values)
                {
                    xml.WriteWhitespace("\t");

                    xml.WriteStartElement(entry.Key.ToString());
                    xml.WriteAttributeString("type", entry.Value.GetType().Name);

                    switch (entry.Value.GetType().Name)
                    {
                        case "ParticleTemplate":
                            xml.WriteValue(((ParticleTemplate)entry.Value).particleID);
                            break;

                        case "Texture2D":
                            xml.WriteValue(((Texture2D)entry.Value).Name);
                            break;

                        default:
                            xml.WriteValue(entry.Value);
                            break;
                    }

                    xml.WriteEndElement();

                    xml.WriteWhitespace("\n");
                }
               
            }
        }
    }
}
