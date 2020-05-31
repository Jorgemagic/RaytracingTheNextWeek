using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Lambertian : Material
    {
        public Texture Albedo;

        public Lambertian(Texture a)
        {
            this.Albedo = a;
        }

        public override bool Scatter(Ray r_in, Hit_Record rec, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 scatter_direction = rec.Normal + Helpers.Random_unit_Vector();
            scattered = new Ray(rec.P, scatter_direction, r_in.Time);
            attenuation = this.Albedo.Value(rec.U, rec.V, rec.P);
            return true;
        }
    }
}
