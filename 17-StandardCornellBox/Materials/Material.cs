using System.Numerics;

namespace _17_StandardCornellBox
{
    public abstract class Material
    {
        public virtual Vector3 Emitted(float u, float v, Vector3 p)
        {
            return Vector3.Zero;
        }

        public abstract bool Scatter(Ray r_in, Hit_Record rec, out Vector3 attenuation, out Ray scattered);        
    }
}
