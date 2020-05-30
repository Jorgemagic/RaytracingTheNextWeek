using System.Numerics;

namespace _06_PerlinTrilinearInterpolation
{
    public class Noise_texture : Texture
    {
        public Perlin Noise;

        public Noise_texture()
        {
            this.Noise = new Perlin();
        }

        public override Vector3 Value(float u, float v, Vector3 p)
        {
            return new Vector3(1) * this.Noise.Noise(p);
        }
    }
}
