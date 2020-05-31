using System.Numerics;

namespace _18_CornellBoxWithSmoke
{
    public class Isotropic : Material
    {
        public Texture albedo;

        public Isotropic(Texture a)
        {
            this.albedo = a;
        }

        public override bool Scatter(Ray r_in, Hit_Record rec, out Vector3 attenuation, out Ray scattered)
        {
            scattered = new Ray(rec.P, Helpers.Random_in_unit_sphere(), r_in.Time);
            attenuation = albedo.Value(rec.U, rec.V, rec.P);
            return true;
        }
    }
}
