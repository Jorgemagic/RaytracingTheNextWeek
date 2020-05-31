using System;
using System.Numerics;

namespace _17_StandardCornellBox
{
    public class Image_texture : Texture
    {
        public static int bytes_per_pixel = 4;

        private byte[] data;
        private int width, height;
        private int bytes_per_scanline;

        public Image_texture()
        {
            this.data = null;
            this.width = 0;
            this.height = 0;
            this.bytes_per_scanline = 0;
        }

        public Image_texture(string filename)
        {
            var image = TextureHelper.Load(filename);

            this.data = image.Data;
            this.width = (int)image.Width;
            this.height = (int)image.Height;

            if (this.data == null)
            {
                Console.WriteLine($"ERROR: Could not load texture image file {filename}.");
                width = height = 0;
            }

            bytes_per_scanline = bytes_per_pixel * width;
        }

        public override Vector3 Value(float u, float v, Vector3 p)
        {
            // If we have no texture data, then return solid cyan as a debugging aid.
            if (data == null)
            {
                return new Vector3(0, 1, 1);
            }

            // Clamp input texture coordinates to [0,1] x [1,0]
            u = Math.Clamp(u, 0.0f, 1.0f);
            v = 1.0f - Math.Clamp(v, 0.0f, 1.0f);  // Flip V to image coordinates

            var i = (int)(u * width);
            var j = (int)(v * height);

            // Clamp integer mapping, since actual coordinates should be less than 1.0
            if (i >= width) i = width - 1;
            if (j >= height) j = height - 1;

            float color_scale = 1.0f / 255.0f;

            int index = j * bytes_per_scanline + i * bytes_per_pixel;
            Vector3 pixel = new Vector3(this.data[index], this.data[index + 1], this.data[index + 2]);

            return new Vector3(color_scale * pixel.X, color_scale * pixel.Y, color_scale * pixel.Z);
        }
    }
}
