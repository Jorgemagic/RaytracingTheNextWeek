using System.Numerics;

namespace _17_StandardCornellBox
{
    public class AABB
    {
        public Vector3 min;
        public Vector3 max;

        public AABB()
        {
        }

        public AABB(Vector3 a, Vector3 b)
        {
            this.min = a;
            this.max = b;
        }

        public bool Hit(Ray r, float tmin, float tmax)
        {
            for (int a = 0; a < 3; a++)
            {
                var invD = 1.0f / Helpers.Vector3GetValue(r.Direction, a);
                var t0 = (Helpers.Vector3GetValue(min, a) - Helpers.Vector3GetValue(r.Origin, a)) * invD;
                var t1 = (Helpers.Vector3GetValue(max, a) - Helpers.Vector3GetValue(r.Origin, a)) * invD;
                if (invD < 0.0f)
                {
                    float tmp = t0;
                    t0 = t1;
                    t1 = tmp;
                }

                tmin = t0 > tmin ? t0 : tmin;
                tmax = t1 < tmax ? t1 : tmax;
                if (tmax <= tmin)
                    return false;
            }
            return true;
        }
    }
}
