﻿using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;

namespace _16_CornellBoxTwoBlocks
{
    public class TextureHelper
    {
        public struct Image
        {
            public uint Width;
            public uint Height;            
            public uint TexturePixelSize;
            public byte[] Data;
        }

        public static Image Load(string filename)
        {
            Image result = default;

            using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(filename))
            {
                result.Width = (uint)image.Width;
                result.Height = (uint)image.Height;
                result.TexturePixelSize = 4;

                var pixels = image.GetPixelSpan();

                for (int i = 0; i < pixels.Length; i++)
                {
                    ref Rgba32 pixel = ref pixels[i];
                    var a = pixel.A;
                    if (a == 0)
                    {
                        pixel.PackedValue = 0;
                    }
                    else
                    {
                        pixel.R = (byte)((pixel.R * a) >> 8);
                        pixel.G = (byte)((pixel.G * a) >> 8);
                        pixel.B = (byte)((pixel.B * a) >> 8);
                    }
                }

                result.Data = MemoryMarshal.AsBytes(pixels).ToArray();
            }

            return result;
        }
    }
}
