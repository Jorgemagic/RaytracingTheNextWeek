
namespace _17_StandardCornellBox
{
    public class Flip_face : HitTable
    {
        public HitTable ptr;       
        
        public Flip_face(HitTable p)
        {
            this.ptr = p;
        }

        public bool Hit(Ray r, float t_min, float t_max, ref Hit_Record rec)
        {
            if (!ptr.Hit(r, t_min, t_max, ref rec))
                return false;

            rec.Front_face = !rec.Front_face;
            return true;
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            return ptr.Bounding_box(t0, t1, out output_box);
        }
    }
}
