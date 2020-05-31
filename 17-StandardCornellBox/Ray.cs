using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Ray
    {
        private Vector3 orig;
        private Vector3 dir;
        private float tm;

        public Vector3 Origin => this.orig;
        public Vector3 Direction => this.dir;

        public float Time => this.tm;

        public Ray(Vector3 a, Vector3 b, float ti = 0.0f)
        {
            this.orig = a;
            this.dir = b;
            this.tm = ti;
        }

        public Vector3 At(float t)
        {
            return this.orig + t * this.dir; //P(t) = A + tb
        }
    }
}
