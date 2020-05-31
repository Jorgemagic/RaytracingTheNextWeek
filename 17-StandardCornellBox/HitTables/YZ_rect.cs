using System.Numerics;

namespace _17_StandardCornellBox
{
    public class YZ_rect : HitTable
    {
        public Material mp;
        public float y0, y1, z0, z1, k;

        public YZ_rect()
        { }

        public YZ_rect(float _y0, float _y1, float _z0, float _z1, float _k, Material mat)
        {
            this.y0 = _y0;
            this.y1 = _y1;
            this.z0 = _z0;
            this.z1 = _z1;
            this.k = _k;
            this.mp = mat;
        }

        public bool Hit(Ray r, float t0, float t1, ref Hit_Record rec)
        {
            var t = (k - r.Origin.X) / r.Direction.X;
            if (t < t0 || t > t1)
                return false;
            var y = r.Origin.Y + t * r.Direction.Y;
            var z = r.Origin.Z + t * r.Direction.Z;
            if (y < y0 || y > y1 || z < z0 || z > z1)
                return false;
            rec.U = (y - y0) / (y1 - y0);
            rec.V = (z - z0) / (z1 - z0);
            rec.T = t;
            var outward_normal = new Vector3(1, 0, 0);
            rec.Set_Face_Normal(r, outward_normal);
            rec.Mat_ptr = mp;
            rec.P = r.At(t);
            return true;
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            // The bounding box must have non-zero width in each dimension, so pad the X
            // dimension a small amount.
            output_box = new AABB(new Vector3(k - 0.0001f, y0, z0), new Vector3(k + 0.0001f, y1, z1));
            return true;
        }        
    }
}
