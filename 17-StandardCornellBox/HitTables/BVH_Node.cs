using System;
using System.Collections.Generic;
using System.Text;

namespace _17_StandardCornellBox
{
    public class BVH_Node : HitTable
    {
        public HitTable left;
        public HitTable right;
        public AABB box;

        public BVH_Node()
        {
        }

        public BVH_Node(Hitable_List list, float time0, float time1)
            : this(list.Objects, 0, (uint)list.Objects.Count, time0, time1)
        {

        }

        public BVH_Node(List<HitTable> objects, uint start, uint end, float time0, float time1)
        {
            int axis = Helpers.random.Next(0, 2);

            IComparer<HitTable> comparator;
            if (axis == 0)
            {
                comparator = new XComparator();
            }
            else if (axis == 1)
            {
                comparator = new YComparator();
            }
            else
            {
                comparator = new ZComparator();
            }

            uint object_span = end - start;

            if (object_span == 1)
            {
                left = right = objects[(int)start];
            }
            else if (object_span == 2)
            {
                if (comparator.Compare(objects[(int)start], objects[(int)start + 1]) <= 0)
                {
                    left = objects[(int)start];
                    right = objects[(int)start + 1];
                }
                else
                {
                    left = objects[(int)start + 1];
                    right = objects[(int)start];
                }
            }
            else
            {
                objects.Sort((int)start, (int)(end - start), comparator);

                var mid = start + object_span / 2;
                left = new BVH_Node(objects, start, mid, time0, time1);
                right = new BVH_Node(objects, mid, end, time0, time1);
            }

            AABB box_left = null;
            AABB box_right = null;

            if (!left.Bounding_box(time0, time1, out box_left)
               || !right.Bounding_box(time0, time1, out box_right)
            )
                Console.WriteLine("No bounding box in bvh_node constructor.");

            box = Helpers.Surrounding_box(box_left, box_right);
        }

        public bool Bounding_box(float t0, float t1, out AABB output_box)
        {
            output_box = this.box;
            return true;
        }

        public bool Hit(Ray r, float t_min, float t_max, ref Hit_Record rec)
        {
            if (!this.box.Hit(r, t_min, t_max))
            {
                return false;
            }

            bool hit_left = left.Hit(r, t_min, t_max, ref rec);
            bool hit_right = right.Hit(r, t_min, hit_left ? rec.T : t_max, ref rec);

            return hit_left || hit_right;
        }

        public static bool Box_compare(HitTable a, HitTable b, int axis)
        {
            AABB box_a = null;
            AABB box_b = null;

            if (!a.Bounding_box(0, 0, out box_a) || !b.Bounding_box(0, 0, out box_b))
                Console.WriteLine("No bounding box in bvh_node constructor.");

            return Helpers.Vector3GetValue(box_a.min, axis) < Helpers.Vector3GetValue(box_b.min, axis);
        }

        public static bool Box_x_compare(HitTable a, HitTable b)
        {
            return Box_compare(a, b, 0);
        }

        public static bool Box_y_compare(HitTable a, HitTable b)
        {
            return Box_compare(a, b, 1);
        }
        public static bool Box_z_compare(HitTable a, HitTable b)
        {
            return Box_compare(a, b, 2);
        }
    }
}
