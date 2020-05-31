using System;
using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Checker_texture : Texture
    {
        public Texture odd;
        public Texture even;

        public Checker_texture()
        { }

        public Checker_texture(Texture t0, Texture t1)
        {
            this.even = t0;
            this.odd = t1;
        }

        public override Vector3 Value(float u, float v, Vector3 p)
        {
            var sines = (float)Math.Sin(10 * p.X) * (float)Math.Sin(10 * p.Y) * (float)Math.Sin(10 * p.Z);
            if (sines < 0)
            {
                return odd.Value(u, v, p);
            }
            else
            {
                return even.Value(u, v, p);
            }
        }
    }
}
