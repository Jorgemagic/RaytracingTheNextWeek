using System;
using System.Numerics;

namespace _05_PerlinNoise
{
    public class Perlin
    {
        private static int point_count = 256;
        private float[] ranfloat;
        private int[] perm_x;
        private int[] perm_y;
        private int[] perm_z;

        public Perlin()
        {
            this.ranfloat = new float[point_count];
            for (int i = 0; i < point_count; ++i)
            {
                ranfloat[i] = (float)Helpers.random.NextDouble();
            }

            this.perm_x = Perlin_generate_perm();
            this.perm_y = Perlin_generate_perm();
            this.perm_z = Perlin_generate_perm();
        }

        public float Noise(Vector3 p)
        {
            var u = p.X - (float)Math.Floor(p.X);
            var v = p.Y - (float)Math.Floor(p.Y);
            var w = p.Z - (float)Math.Floor(p.Z);

            var i = (int)(4 * p.X) & 255;
            var j = (int)(4 * p.Y) & 255;
            var k = (int)(4 * p.Z) & 255;

            return ranfloat[perm_x[i] ^ perm_y[j] ^ perm_z[k]];
        }

        private static int[] Perlin_generate_perm()
        {
            var p = new int[point_count];

            for (int i = 0; i < point_count; i++)
            {
                p[i] = i;
            }

            Permute(p, point_count);

            return p;
        }

        private static void Permute(int[] p, int n)
        {
            for(int i = n - 1; i > 0; i--) {
                int target = Helpers.random.Next(0, i);
                int tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }
        }
    }
}
