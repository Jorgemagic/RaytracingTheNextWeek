using System.Numerics;

namespace _07_PerlinHermitianSmoothing
{
    public interface Material
    {
        public bool Scatter(Ray r_in, Hit_Record rec, out Vector3 attenuation, out Ray scattered);
    }
}
