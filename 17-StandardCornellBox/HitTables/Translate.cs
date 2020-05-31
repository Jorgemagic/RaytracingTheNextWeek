using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Translate : HitTable
    {
        public HitTable ptr;
        public Vector3 offset;

        public Translate(HitTable p, Vector3 displacement)
        {
            this.ptr = p;
            this.offset = displacement;
        }

        public bool Hit(Ray r, float t_min, float t_max, ref Hit_Record rec)
        {
            Ray moved_r = new Ray(r.Origin - offset, r.Direction, r.Time);
            if (!ptr.Hit(moved_r, t_min, t_max, ref rec))
                return false;

            rec.P += offset;
            rec.Set_Face_Normal(moved_r, rec.Normal);

            return true;
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            if (!ptr.Bounding_box(t0, t1, out output_box))
                return false;

            output_box = new AABB(
                output_box.min + offset,
                output_box.max + offset);

            return true;
        }
    }
}
