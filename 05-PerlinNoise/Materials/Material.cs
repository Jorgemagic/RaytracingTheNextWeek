﻿using System.Numerics;

namespace _05_PerlinNoise
{
    public interface Material
    {
        public bool Scatter(Ray r_in, Hit_Record rec, out Vector3 attenuation, out Ray scattered);
    }
}
