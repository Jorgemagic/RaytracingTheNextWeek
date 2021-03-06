﻿using System;
using System.IO;
using System.Numerics;

namespace _01_MotionBlur
{
    public class Program
    {
        public static Hitable_List Random_Scene()
        {
            Hitable_List world = new Hitable_List();

            var ground_material = new Lambertian(new Vector3(0.5f, 0.5f, 0.5f));
            world.Add(new Sphere(new Vector3(0, -1000, 0), 1000, ground_material));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    float choose_mat = (float)Helpers.random.NextDouble();
                    Vector3 center = new Vector3(a + 0.9f * (float)Helpers.random.NextDouble(), 0.2f, b + 0.9f * (float)Helpers.random.NextDouble());

                    if ((center - new Vector3(4, 0.2f, 0)).Length() > 0.9f)
                    {
                        Material sphere_material;

                        if (choose_mat < 0.8f)
                        {
                            // diffuse
                            Vector3 albedo = Helpers.RandomVector3(0, 1) * Helpers.RandomVector3(0, 1);
                            sphere_material = new Lambertian(albedo);
                            var center2 = center + new Vector3(0, Helpers.RandomFloat(0, 0.5f), 0);
                            world.Add(new Moving_Sphere(center, center2, 0.0f, 1.0f, 0.2f, sphere_material));
                        }
                        else if (choose_mat < 0.95f)
                        {
                            // metal
                            Vector3 albedo = Helpers.RandomVector3(0.5f, 1.0f);
                            float fuzz = Helpers.RandomFloat(0, 0.5f);
                            sphere_material = new Metal(albedo, fuzz);
                            world.Add(new Sphere(center, 0.2f, sphere_material));
                        }
                        else
                        {
                            // glass
                            sphere_material = new Dielectric(1.5f);
                            world.Add(new Sphere(center, 0.2f, sphere_material));
                        }
                    }
                }
            }

            var material1 = new Dielectric(1.5f);
            world.Add(new Sphere(new Vector3(0, 1, 0), 1.0f, material1));

            var material2 = new Lambertian(new Vector3(0.4f, 0.2f, 0.1f));
            world.Add(new Sphere(new Vector3(-4, 1, 0), 1.0f, material2));

            var material3 = new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f);
            world.Add(new Sphere(new Vector3(4, 1, 0), 1.0f, material3));

            return world;
        }

        static Vector3 Ray_color(Ray r, HitTable world, int depth)
        {
            Hit_Record rec = default;
            // If we've exceeded the ray bounce limit, no more light is gathered.
            if (depth <= 0)
                return Vector3.Zero;

            if (world.Hit(r, 0.001f, Helpers.Infinity, ref rec))
            {
                Ray scattered;
                Vector3 attenuation;
                if (rec.Mat_ptr.Scatter(r, rec, out attenuation, out scattered))
                    return attenuation * Ray_color(scattered, world, depth - 1);
                return Vector3.Zero;
            }

            Vector3 unit_direction = Vector3.Normalize(r.Direction);
            var t = 0.5f * (unit_direction.Y + 1.0f);
            return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
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

                Hitable_List world = Random_Scene();

                Vector3 lookfrom = new Vector3(13, 2, 3);
                Vector3 lookat = new Vector3(0, 0, 0);
                Vector3 vup = new Vector3(0, 1, 0);
                float dist_to_focus = 10.0f;
                float aperture = 0.0f;

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
                            pixel_color += Ray_color(r, world, max_depth);
                        }
                        Write_color(file, pixel_color, samples_per_pixel);
                    }
                }

                Console.WriteLine("Done.");
            }
        }
    }
}