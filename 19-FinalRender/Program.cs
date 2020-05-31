using System;
using System.IO;
using System.Numerics;

namespace _19_FinalRender
{
    public class Program
    {
        public static Hitable_List Final_scene()
        {
            Hitable_List boxes1 = new Hitable_List();
            var ground = new Lambertian(new Solid_Color(0.48f, 0.83f, 0.53f));

            const int boxes_per_side = 20;
            for (int i = 0; i < boxes_per_side; i++)
            {
                for (int j = 0; j < boxes_per_side; j++)
                {
                    var w = 100.0f;
                    var x0 = -1000.0f + i * w;
                    var z0 = -1000.0f + j * w;
                    var y0 = 0.0f;
                    var x1 = x0 + w;
                    var y1 = Helpers.RandomFloat(1, 101);
                    var z1 = z0 + w;

                    boxes1.Add(new Box(new Vector3(x0, y0, z0), new Vector3(x1, y1, z1), ground));
                }
            }

            Hitable_List objects = new Hitable_List();

            objects.Add(boxes1); // new BVH_Node(boxes1, 0, 1));

            var light = new Diffuse_light(new Solid_Color(7, 7, 7));
            objects.Add(new XZ_rect(123, 423, 147, 412, 554, light));

            var center1 = new Vector3(400, 400, 200);
            var center2 = center1 + new Vector3(30, 0, 0);
            var moving_sphere_material = new Lambertian(new Solid_Color(0.7f, 0.3f, 0.1f));
            objects.Add(new Moving_Sphere(center1, center2, 0, 1, 50, moving_sphere_material));

            objects.Add(new Sphere(new Vector3(260, 150, 45), 50, new Dielectric(1.5f)));
            objects.Add(new Sphere(new Vector3(0, 150, 145), 50, new Metal(new Vector3(0.8f, 0.8f, 0.9f), 10.0f)));

            var boundary = new Sphere(new Vector3(360, 150, 145), 70, new Dielectric(1.5f));
            objects.Add(boundary);
            objects.Add(new Constant_medium(boundary, 0.2f, new Solid_Color(0.2f, 0.4f, 0.9f)));
            boundary = new Sphere(new Vector3(0, 0, 0), 5000, new Dielectric(1.5f));
            objects.Add(new Constant_medium(boundary, .0001f, new Solid_Color(1, 1, 1)));

            var emat = new Lambertian(new Image_texture("Resources/earthmap.jpg"));
            objects.Add(new Sphere(new Vector3(400, 200, 400), 100, emat));
            var pertext = new Noise_texture(0.1f);
            objects.Add(new Sphere(new Vector3(220, 280, 300), 80, new Lambertian(pertext)));

            Hitable_List boxes2 = new Hitable_List();
            var white = new Lambertian(new Solid_Color(.73f, .73f, .73f));
            int ns = 1000;
            for (int j = 0; j < ns; j++)
            {
                boxes2.Add(new Sphere(Helpers.RandomVector3(0, 165), 10, white));
            }

            objects.Add(new Translate(
                new Rotate_y(
                    boxes2,15), //new BVH_Node(boxes2, 0.0f, 1.0f), 15),
                    new Vector3(-100, 270, 395)
                )
            );

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
            int image_width = 384;
            int image_height = image_width;
            float aspect_ratio = (float)image_width / image_height;
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

                Hitable_List world = Final_scene();

                Vector3 lookfrom = new Vector3(278, 278, -800);
                Vector3 lookat = new Vector3(278, 278, 0);
                Vector3 vup = new Vector3(0, 1, 0);
                float dist_to_focus = 10.0f;
                float aperture = 0.0f;
                float vfov = 40.0f;
                Vector3 background = new Vector3(0.0f, 0.0f, 0.0f);

                Camera cam = new Camera(lookfrom, lookat, vup, vfov, aspect_ratio, aperture, dist_to_focus, 0.0f, 1.0f);

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