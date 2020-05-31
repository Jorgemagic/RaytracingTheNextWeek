using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Diffuse_light : Material
    {
        public Texture emit;

        public Diffuse_light(Texture a)
        {
            this.emit = a;
        }

        public override bool Scatter(Ray r_in, Hit_Record rec, out Vector3 attenuation, out Ray scattered)
        {
            attenuation = default;
            scattered = null;
            return false;
        }

        public override Vector3 Emitted(float u, float v, Vector3 p)
        {
            return this.emit.Value(u, v, p);
        }
    }
}
