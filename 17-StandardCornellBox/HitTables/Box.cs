using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Box : HitTable
    {
        public Vector3 box_min;
        public Vector3 box_max;
        public Hitable_List sides;

        public Box()
        {
            this.sides = new Hitable_List();
        }

        public Box(Vector3 p0, Vector3 p1, Material ptr)
            : this()
        {
            box_min = p0;
            box_max = p1;

            sides.Add(new XY_rect(p0.X, p1.X, p0.Y, p1.Y, p1.Z, ptr));
            sides.Add(new Flip_face(
                new XY_rect(p0.X, p1.X, p0.Y, p1.Y, p0.Z, ptr)));

            sides.Add(new XZ_rect(p0.X, p1.X, p0.Z, p1.Z, p1.Y, ptr));
            sides.Add(new Flip_face(
                new XZ_rect(p0.X, p1.X, p0.Z, p1.Z, p0.Y, ptr)));

            sides.Add(new YZ_rect(p0.Y, p1.Y, p0.Z, p1.Z, p1.X, ptr));
            sides.Add(new Flip_face(
                new YZ_rect(p0.Y, p1.Y, p0.Z, p1.Z, p0.X, ptr)));
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            output_box = new AABB(box_min, box_max);
            return true;
        }

        public bool Hit(Ray r, float t0, float t1, ref Hit_Record rec)
        {
            return sides.Hit(r, t0, t1, ref rec);
        }
    }
}
