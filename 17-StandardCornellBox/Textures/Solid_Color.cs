using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace _17_StandardCornellBox
{
    public class Solid_Color : Texture
    {
        public Solid_Color()
        { }

        public Solid_Color(Vector3 c)
        {
            this.color_value = c;
        }

        public Solid_Color(float red, float green, float blue)
            : this(new Vector3(red, green, blue))
        {
        }

        public override Vector3 Value(float u, float v, Vector3 p)
        {
            return this.color_value;
        }
    }
}
