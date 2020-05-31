using System;
using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Noise_texture : Texture
    {
        public Perlin Noise;
        public float Scale;

        public Noise_texture()
        {
            this.Noise = new Perlin();
        }

        public Noise_texture(float sc)
            : this()
        {
            this.Scale = sc;
        }

        public override Vector3 Value(float u, float v, Vector3 p)
        {
            return new Vector3(1) * 0.5f * (1.0f + (float)Math.Sin(this.Scale * p.Z + 10 * this.Noise.Turb(p)));
        }
    }
}
