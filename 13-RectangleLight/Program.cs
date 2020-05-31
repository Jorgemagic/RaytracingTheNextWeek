using System;
using System.IO;
using System.Numerics;

namespace _13_RectangleLight
{
    public class Program
    {
        public static Hitable_List Simple_Light()
        {
            Hitable_List objects = new Hitable_List();

            var pertext = new Noise_texture(4);
            objects.Add(new Sphere(new Vector3(0, -1000, 0), 1000, new Lambertian(pertext)));
            objects.Add(new Sphere(new Vector3(0, 2, 0), 2, new Lambertian(pertext)));

            var difflight = new Diffuse_light(new Solid_Color(4, 4, 4));
            objects.Add(new Sphere(new Vector3(0, 7, 0), 2, difflight));
            objects.Add(new XY_rect(3, 5, 1, 3, -2, difflight));

            return objects;
        }

        static Vector3 Ray_color(Ray r, Vector3 background, HitTable world, int depth)
        {
            Hit_Record rec = default;
            // If we've exceeded the ray bounce limit, no more light is gathered.
            if (depth <= 0)
                return Vector3.Zero;

            // If the ray hits nothing, return the background color.
            if (!world.Hit(r, 0.001f, Helpers.Infinity, ref rec))
                return background;

            Ray scattered;
            Vector3 attenuation;
            Vector3 emitted = rec.Mat_ptr.Emitted(rec.U, rec.V, rec.P);

            if (!rec.Mat_ptr.Scatter(r, rec, out attenuation, out scattered))
                return emitted;

            return emitted + attenuation * Ray_color(scattered, background, world, depth - 1);
        }

        static void Write_color(StreamWriter file, Vector3 pixel_color, int samples_per_pixel)
        {
            var r = pixel_color.X;
            var g = pixel_color.Y;
            var b = pixel_color.Z;

            // Divide the color total by the number of samples.
            float scale = 1.0f / samples_per_pixel;
            r = (float)Math.Sqrt(scale * r);
            g = (float)Math.Sqrt(scale * g);
            b = (float)Math.Sqrt(scale * b);
            // Write the translated [0,255] value of each color component.            
            file.WriteLine($"{(int)(256 * Math.Clamp(r, 0.0, 0.999))} {(int)(256 * Math.Clamp(g, 0.0, 0.999))} {(int)(256 * Math.Clamp(b, 0.0, 0.999))}");
        }

        static void Main(string[] args)
        {
            float aspect_ratio = 16.0f / 9.0f;
            int image_width = 384;
            int image_height = (int)(image_width / aspect_ratio);
            int samples_per_pixel = 100;
            int max_depth = 50;

            string filePath = "image.ppm";

            using (var file = new StreamWriter(filePath))
            {
                file.WriteLine($"P3\n{image_width} {image_height}\n255");

                float viewport_height = 2.0f;
                float viewport_width = aspect_ratio * viewport_height;
                float focal_length = 1.0f;

                var origin = Vector3.Zero;
                var horizontal = new Vector3(viewport_width, 0, 0);
                var vertical = new Vector3(0, viewport_height, 0);
                var lower_left_corner = origin - horizontal / 2 - vertical / 2 - new Vector3(0, 0, focal_length);

                Hitable_List world = Simple_Light();

                Vector3 lookfrom = new Vector3(23, 4, 5f);
                Vector3 lookat = new Vector3(0, 2, 0);
                Vector3 vup = new Vector3(0, 1, 0);
                float dist_to_focus = 20.0f;
                float aperture = 0.0f;
                Vector3 background = new Vector3(0.0f, 0.0f, 0.0f);

                Camera cam = new Camera(lookfrom, lookat, vup, 20, aspect_ratio, aperture, dist_to_focus, 0.0f, 1.0f);

                for (int j = image_height - 1; j >= 0; --j)
                {
                    Console.WriteLine($"\rScanlines remaining: {j}");
                    for (int i = 0; i < image_width; ++i)
                    {
                        Vector3 pixel_color = Vector3.Zero;
                        for (int s = 0; s < samples_per_pixel; ++s)
                        {
                            float u = (float)(i + Helpers.random.NextDouble()) / (image_width - 1);
                            float v = (float)(j + Helpers.random.NextDouble()) / (image_height - 1);
                            Ray r = cam.Get_Ray(u, v);
                            pixel_color += Ray_color(r, background, world, max_depth);
                        }
                        Write_color(file, pixel_color, samples_per_pixel);
                    }
                }

                Console.WriteLine("Done.");
            }
        }
    }
}