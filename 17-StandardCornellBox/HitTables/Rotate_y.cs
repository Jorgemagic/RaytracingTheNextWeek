using System;
using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Rotate_y : HitTable
    {
        public HitTable ptr;
        public float sin_theta;
        public float cos_theta;
        public bool hasbox;
        public AABB bbox;

        public Rotate_y(HitTable p, float angle)
        {
            this.ptr = p;
            var radians = Helpers.Degress_to_radians(angle);
            sin_theta = (float)Math.Sin(radians);
            cos_theta = (float)Math.Cos(radians);
            hasbox = ptr.Bounding_box(0, 1, out bbox);

            Vector3 min = new Vector3(Helpers.Infinity, Helpers.Infinity, Helpers.Infinity);
            Vector3 max = new Vector3(-Helpers.Infinity, -Helpers.Infinity, -Helpers.Infinity);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var x = i * bbox.max.X + (1 - i) * bbox.min.X;
                        var y = j * bbox.max.Y + (1 - j) * bbox.min.Y;
                        var z = k * bbox.max.Z + (1 - k) * bbox.min.Z;

                        var newx = cos_theta * x + sin_theta * z;
                        var newz = -sin_theta * x + cos_theta * z;

                        Vector3 tester = new Vector3(newx, y, newz);

                        for (int c = 0; c < 3; c++)
                        {
                            Helpers.Vector3SetValue(ref min, c, (float)Math.Min(Helpers.Vector3GetValue(min, c), Helpers.Vector3GetValue(tester, c)));
                            Helpers.Vector3SetValue(ref max, c, (float)Math.Max(Helpers.Vector3GetValue(max, c), Helpers.Vector3GetValue(tester, c)));
                        }
                    }
                }
            }

            bbox = new AABB(min, max);
        }

        public bool Hit(Ray r, float t_min, float t_max, ref Hit_Record rec)
        {
            var origin = r.Origin;
            var direction = r.Direction;

            origin.X = cos_theta * r.Origin.X - sin_theta * r.Origin.Z;
            origin.Z = sin_theta * r.Origin.X + cos_theta * r.Origin.Z;

            direction.X = cos_theta * r.Direction.X - sin_theta * r.Direction.Z;
            direction.Z = sin_theta * r.Direction.X + cos_theta * r.Direction.Z;

            Ray rotated_r = new Ray(origin, direction, r.Time);

            if (!ptr.Hit(rotated_r, t_min, t_max, ref rec))
                return false;

            var p = rec.P;
            var normal = rec.Normal;

            p.X = cos_theta * rec.P.X + sin_theta * rec.P.Z;
            p.Z = -sin_theta * rec.P.X + cos_theta * rec.P.Z;

            normal.X = cos_theta * rec.Normal.X + sin_theta * rec.Normal.Z;
            normal.Z = -sin_theta * rec.Normal.X + cos_theta * rec.Normal.Z;

            rec.P = p;
            rec.Set_Face_Normal(rotated_r, normal);

            return true;
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            output_box = bbox;
            return hasbox;
        }
    }
}
