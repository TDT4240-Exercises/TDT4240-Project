using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace X2Game
{
    public enum UnitValues
    {
        Health,
        TurnRate,
        Speed,
        Texture
    }

    class UnitType : GenericDataStructure
    {
        public readonly Texture2D Texture;

        public UnitType(string xmlPath)
            : base(xmlPath, typeof(UnitValues))
        {
            Texture = ResourceManager.GetTexture("units/" + GetValue<String>(UnitValues.Texture));
        }

    }
}
