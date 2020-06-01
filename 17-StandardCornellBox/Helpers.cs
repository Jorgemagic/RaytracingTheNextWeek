using System;
using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Helpers
    {
        // Constants
        public static float Infinity = float.MaxValue;
        public static Random random = new Random();

        // Utility Functions
        public static float Degress_to_radians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }

        public static float RandomFloat(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        public static Vector3 RandomVector3(float min, float max)
        {
            return new Vector3(RandomFloat(min, max), RandomFloat(min, max), RandomFloat(min, max));
        }

        public static Vector3 Random_in_unit_sphere()
        {
            while (true)
            {
                Vector3 p = RandomVector3(-1, 1);
                if (p.LengthSquared() >= 1)
                    continue;
                return p;
            }
        }

        public static Vector3 Random_unit_Vector()
        {
            float a = RandomFloat(0, 2.0f * (float)Math.PI);
            float z = RandomFloat(-1, 1);
            float r = (float)Math.Sqrt(1 - z * z);
            return new Vector3(r * (float)Math.Cos(a), r * (float)Math.Sin(a), z);
        }

        public static Vector3 Random_in_hemisphere(Vector3 normal)
        {
            Vector3 in_unit_sphere = Random_unit_Vector();
            if (Vector3.Dot(in_unit_sphere, normal) > 0.0) // In the same hemisphere as the normal
                return in_unit_sphere;
            else
                return -in_unit_sphere;
        }

        public static Vector3 Reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }

        public static Vector3 Refract(Vector3 uv, Vector3 n, float etai_over_etat)
        {
            float cos_theta = Vector3.Dot(-uv, n);
            Vector3 r_out_parallel = etai_over_etat * (uv + cos_theta * n);
            Vector3 r_out_perp = -(float)Math.Sqrt(1.0f - r_out_parallel.LengthSquared()) * n;
            return r_out_parallel + r_out_perp;
        }

        public static float Schlick(float cosine, float ref_idx)
        {
            float r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * (float)Math.Pow((1 - cosine), 5);
        }

        public static Vector3 Random_in_unit_disk()
        {
            while (true)
            {
                Vector3 p = new Vector3(RandomFloat(-1, 1), RandomFloat(-1, 1), 0);
                if (p.LengthSquared() >= 1)
                    continue;

                return p;
            }
        }

        public static float Vector3GetValue(Vector3 v, int i)
        {
            if (i == 0)
            {
                return v.X;
            }
            else if (i == 1)
            {
                return v.Y;
            }
            else if (i == 2)
            {
                return v.Z;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static void Vector3SetValue(ref Vector3 v, int i, float newValue)
        {
            if (i == 0)
            {
                v.X = newValue;
            }
            else if (i == 1)
            {
                v.Y = newValue;
            }
            else if (i == 2)
            {
                v.Z = newValue;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static AABB Surrounding_box(AABB box0, AABB box1)
        {
            Vector3 small = new Vector3((float)Math.Min(box0.min.X, box1.min.X),
                                        (float)Math.Min(box0.min.Y, box1.min.Y),
                                        (float)Math.Min(box0.min.Z, box1.min.Z));

            Vector3 big = new Vector3((float)Math.Max(box0.max.X, box1.max.X),
                                      (float)Math.Max(box0.max.Y, box1.max.Y),
                                      (float)Math.Max(box0.max.Z, box1.max.Z));
            return new AABB(small, big);
        }

        public static int Box_compare(HitTable a, HitTable b, int axis)
        {
            AABB box_a = null;
            AABB box_b = null;

            if (!a.Bounding_box(0, 0, out box_a) || !b.Bounding_box(0, 0, out box_b))
                Console.WriteLine("No bounding box in bvh_node constructor.");

            if (Helpers.Vector3GetValue(box_a.min, axis) < Helpers.Vector3GetValue(box_b.min, axis))
            {
                return -1;
            }
            else
            {
                return 1;
            }

        }
    }
}
