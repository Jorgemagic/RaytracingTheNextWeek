using System;
using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Perlin
    {
        private static int point_count = 256;
        private Vector3[] ranvec;
        private int[] perm_x;
        private int[] perm_y;
        private int[] perm_z;

        public Perlin()
        {
            this.ranvec = new Vector3[point_count];
            for (int i = 0; i < point_count; ++i)
            {
                ranvec[i] = Vector3.Normalize(Helpers.RandomVector3(-1, 1));
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

            int i = (int)Math.Floor(p.X);
            int j = (int)Math.Floor(p.Y);
            int k = (int)Math.Floor(p.Z);
            Vector3[,,] c = new Vector3[2, 2, 2];

            for (int di = 0; di < 2; di++)
                for (int dj = 0; dj < 2; dj++)
                    for (int dk = 0; dk < 2; dk++)
                        c[di, dj, dk] = ranvec[
                            perm_x[(i + di) & 255] ^
                            perm_y[(j + dj) & 255] ^
                            perm_z[(k + dk) & 255]
                        ];

            return this.Perlin_interp(c, u, v, w);
        }

        public float Turb(Vector3 p, int depth = 7)
        {
            var accum = 0.0f;
            var temp_p = p;
            var weight = 1.0f;

            for (int i = 0; i < depth; i++)
            {
                accum += weight * this.Noise(temp_p);
                weight *= 0.5f;
                temp_p *= 2;
            }

            return Math.Abs(accum);
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
            for (int i = n - 1; i > 0; i--)
            {
                int target = Helpers.random.Next(0, i);
                int tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }
        }

        public float Trilinear_interp(float[,,] c, float u, float v, float w)
        {
            var accum = 0.0f;
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        accum += (i * u + (1 - i) * (1 - u)) *
                                 (j * v + (1 - j) * (1 - v)) *
                                 (k * w + (1 - k) * (1 - w)) * c[i, j, k];

            return accum;
        }

        public float Perlin_interp(Vector3[,,] c, float u, float v, float w)
        {
            var uu = u * u * (3 - 2 * u);
            var vv = v * v * (3 - 2 * v);
            var ww = w * w * (3 - 2 * w);
            var accum = 0.0f;

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                    {
                        Vector3 weight_v = new Vector3(u - i, v - j, w - k);
                        accum += (i * uu + (1 - i) * (1 - uu))
                               * (j * vv + (1 - j) * (1 - vv))
                               * (k * ww + (1 - k) * (1 - ww))
                               * Vector3.Dot(c[i, j, k], weight_v);
                    }

            return accum;
        }
    }
}
