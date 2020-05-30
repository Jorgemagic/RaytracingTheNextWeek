using System.Numerics;

namespace _02_BoundingVolumeHierarchies
{
    public class Lambertian : Material
    {
        public Vector3 Albedo;

        public Lambertian(Vector3 a)
        {
            this.Albedo = a;
        }

        public bool Scatter(Ray r_in, Hit_Record rec, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 scatter_direction = rec.Normal + Helpers.Random_unit_Vector();
            scattered = new Ray(rec.P, scatter_direction, r_in.Time);
            attenuation = this.Albedo;
            return true;
        }
    }
}
