using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace _17_StandardCornellBox
{
    public class Moving_Sphere : HitTable
    {
        public Vector3 center0, center1;
        public float time0, time1;
        public float radius;
        public Material mat_ptr;

        public Moving_Sphere()
        { }

        public Moving_Sphere(Vector3 cen0, Vector3 cen1, float t0, float t1, float r, Material m)
        {
            this.center0 = cen0;
            this.center1 = cen1;
            this.time0 = t0;
            this.time1 = t1;
            this.radius = r;
            this.mat_ptr = m;
        }


        public bool Hit(Ray r, float t_min, float t_max, ref Hit_Record rec)
        {            
                Vector3 oc = r.Origin - this.Center(r.Time);
                var a = r.Direction.LengthSquared();
                var half_b = Vector3.Dot(oc, r.Direction);
                var c = oc.LengthSquared() - radius * radius;

                var discriminant = half_b * half_b - a * c;

                if (discriminant > 0)
                {
                    var root = (float)Math.Sqrt(discriminant);

                    var temp = (-half_b - root) / a;
                    if (temp < t_max && temp > t_min)
                    {
                        rec.T = temp;
                        rec.P = r.At(rec.T);
                        var outward_normal = (rec.P - this.Center(r.Time)) / radius;
                        rec.Set_Face_Normal(r, outward_normal);
                        rec.Mat_ptr = mat_ptr;
                        return true;
                    }

                    temp = (-half_b + root) / a;
                    if (temp < t_max && temp > t_min)
                    {
                        rec.T = temp;
                        rec.P = r.At(rec.T);
                        var outward_normal = (rec.P - this.Center(r.Time)) / radius;
                        rec.Set_Face_Normal(r, outward_normal);
                        rec.Mat_ptr = mat_ptr;
                        return true;
                    }
                }
                return false;
            }

        public Vector3 Center(float time)
        {
            return center0 + ((time - time0) / (time1 - time0)) * (center1 - center0);
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            AABB box0 = new AABB(
                this.Center(t0) - new Vector3(radius),
                this.Center(t0) + new Vector3(radius));

            AABB box1 = new AABB(
                this.Center(t1) - new Vector3(radius),
                this.Center(t1) + new Vector3(radius));

            output_box = Helpers.Surrounding_box(box0, box1);
            return true;
        }
    }
}
