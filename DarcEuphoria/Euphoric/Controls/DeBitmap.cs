using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DarcEuphoria.Euphoric.Controls
{
    public class DeBitmap
    {
        public DeBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new int[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb,
                BitsHandle.AddrOfPinnedObject());
        }

        public DeBitmap(string path)
        {
            var image = (Bitmap) Image.FromFile(path);
            Width = image.Width;
            Height = image.Height;
            Bits = new int[image.Width * image.Height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(image.Width, image.Height, image.Width * 4, PixelFormat.Format32bppPArgb,
                BitsHandle.AddrOfPinnedObject());
            Path = path;
            SetImage(path);
        }

        public DeBitmap(Bitmap bitmap)
        {
            var image = bitmap;
            Width = image.Width;
            Height = image.Height;
            Bits = new int[image.Width * image.Height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(image.Width, image.Height, image.Width * 4, PixelFormat.Format32bppPArgb,
                BitsHandle.AddrOfPinnedObject());
        }

        public Bitmap Bitmap { get; }
        public int[] Bits { get; }
        public int Width { get; }
        public int Height { get; }
        public bool Disposed { get; private set; }
        public string Path { get; }

        protected GCHandle BitsHandle { get; }


        public void SetPixel(int x, int y, Color colour)
        {
            var index = x + y * Width;
            var col = colour.ToArgb();
            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            var index = x + y * Width;
            var col = Bits[index];
            return Color.FromArgb(col);
        }

        public void SetImage(string path)
        {
            var image = (Bitmap) Image.FromFile(path);
            for (var x = 0; x < image.Width; x++)
            for (var y = 0; y < image.Height; y++)
            {
                var color = image.GetPixel(x, y);
                SetPixel(x, y, color);
            }
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}