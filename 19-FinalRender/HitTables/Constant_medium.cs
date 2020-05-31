using System;
using System.Numerics;

namespace _19_FinalRender
{
    public class Constant_medium : HitTable
    {
        public HitTable boundary;
        public Material phase_function;
        public float neg_inv_density;

        public Constant_medium(HitTable b, float d, Texture a)
        {
            this.boundary = b;
            this.neg_inv_density = -1 / d;
            this.phase_function = new Isotropic(a);
        }

        public bool Hit(Ray r, float t_min, float t_max, ref Hit_Record rec)
        {
            // Print occasional samples when debugging. To enable, set enableDebug true.
            const bool enableDebug = false;
            bool debugging = enableDebug && ((float)Helpers.random.NextDouble() < 0.00001f);

            Hit_Record rec1, rec2;
            rec1 = rec2 = default;

            if (!boundary.Hit(r, -Helpers.Infinity, Helpers.Infinity, ref rec1))
                return false;

            if (!boundary.Hit(r, rec1.T + 0.0001f, Helpers.Infinity, ref rec2))
                return false;

            if (debugging) Console.WriteLine($"t0={rec1.T}, t1={rec2.T}");

            if (rec1.T < t_min) rec1.T = t_min;
            if (rec2.T > t_max) rec2.T = t_max;

            if (rec1.T >= rec2.T)
                return false;

            if (rec1.T < 0)
                rec1.T = 0;

            float ray_length = r.Direction.Length();
            float distance_inside_boundary = (rec2.T - rec1.T) * ray_length;
            float hit_distance = neg_inv_density * (float)Math.Log(Helpers.random.NextDouble());

            if (hit_distance > distance_inside_boundary)
                return false;

            rec.T = rec1.T + hit_distance / ray_length;
            rec.P = r.At(rec.T);

            if (debugging)
            {
                Console.WriteLine($"hit_distance = {hit_distance} \n " +
                                 $"rec.t = {rec.T} \n" +
                                 $"rec.p = {rec.P}");
            }

            rec.Normal = new Vector3(1, 0, 0);  // arbitrary
            rec.Front_face = true;     // also arbitrary
            rec.Mat_ptr = phase_function;

            return true;
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            return this.boundary.Bounding_box(t0, t1, out output_box);
        }
    }
}
