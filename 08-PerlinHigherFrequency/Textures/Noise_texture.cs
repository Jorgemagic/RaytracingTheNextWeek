using System.Numerics;

namespace _08_PerlinHigherFrequency
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
            return new Vector3(1) * this.Noise.Noise(this.Scale * p);
        }
    }
}
