using System;
using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Camera
    {
        private Vector3 origin;
        private Vector3 lower_left_corner;
        private Vector3 horizontal;
        private Vector3 vertical;
        private Vector3 u;
        private Vector3 v;
        private Vector3 w;
        // shutter opne/close times
        private float time0;
        private float time1;
        private float lens_radius;

        public Camera(Vector3 lookfrom,
                      Vector3 lookat,
                      Vector3 vup,
                      float vfov, // vertical field-of-view in degress
                      float aspect_ratio,
                      float aperture,
                      float focus_dist,
                      float t0 = 0,
                      float t1 = 0)
        {
            float theta = Helpers.Degress_to_radians(vfov);
            float h = (float)Math.Tan(theta / 2);
            float viewport_height = 2.0f * h;
            float viewport_width = aspect_ratio * viewport_height;

            w = Vector3.Normalize(lookfrom - lookat);
            u = Vector3.Normalize(Vector3.Cross(vup, w));
            v = Vector3.Cross(w, u);

            origin = lookfrom;
            horizontal = focus_dist * viewport_width * u;
            vertical = focus_dist * viewport_height * v;
            lower_left_corner = origin - horizontal / 2 - vertical / 2 - focus_dist * w;

            lens_radius = aperture / 2;
            this.time0 = t0;
            this.time1 = t1;
        }

        public Ray Get_Ray(float s, float t)
        {
            Vector3 rd = lens_radius * Helpers.Random_in_unit_disk();
            Vector3 offset = u * rd.X + v * rd.Y;

            return new Ray(this.origin + offset, this.lower_left_corner + s * horizontal + t * vertical - origin - offset, Helpers.RandomFloat(time0, time1));
        }
    }
}
