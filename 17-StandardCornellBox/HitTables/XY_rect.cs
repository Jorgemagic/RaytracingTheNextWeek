using System.Numerics;

namespace _17_StandardCornellBox
{
    public class XY_rect : HitTable
    {
        public Material mp;
        public float x0, x1, y0, y1, k;

        public XY_rect()
        { }

        public XY_rect(float _x0, float _x1, float _y0, float _y1, float _k, Material mat)
        {
            this.x0 = _x0;
            this.x1 = _x1;
            this.y0 = _y0;
            this.y1 = _y1;
            this.k = _k;
            this.mp = mat;
        }

        public bool Hit(Ray r, float t0, float t1, ref Hit_Record rec)
        {
            var t = (k - r.Origin.Z) / r.Direction.Z;

            if (t < t0 || t > t1)
            {
                return false;
            }

            var x = r.Origin.X + t * r.Direction.X;
            var y = r.Origin.Y + t * r.Direction.Y;

            if (x < x0 || x > x1 || y < y0 || y > y1)
            {
                return false;
            }

            rec.U = (x - x0) / (x1 - x0);
            rec.V = (y - y0) / (y1 - y0);
            rec.T = t;
            var outward_normal = new Vector3(0, 0, 1);
            rec.Set_Face_Normal(r, outward_normal);
            rec.Mat_ptr = mp;
            rec.P = r.At(t);

            return true;
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            // The bounding box must have non-zero width in each dimension, so pad the Z
            // dimension a small amount.
            output_box = new AABB(new Vector3(x0, y0, k - 0.0001f), new Vector3(x1, y1, k + 0.0001f));
            return true;
        }
    }
}
