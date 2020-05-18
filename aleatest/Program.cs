using Alea;
using Alea.Parallel;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GpuFractal {

  internal static class Program {

    #region Private Methods
    [GpuManaged]
    private static void Main() {
      while (true) {
        DoStuff();
      }
    }





    public static Gpu gpu { get; set; } = Gpu.Default;
    [GpuManaged]
    private static void DoStuff() {
      const float xs = -2.1F;
      const float ys = -1.3F;
      const int zoom = 1;
      const int width = 10240;
      const int height = 10240;

      const int blackpixel = (byte)0 | ((byte)0 << 8) | ((byte)0 << 16) | ((byte)255 << 24);

      var gpubits = gpu.Allocate<int>(width * height);
      Console.WriteLine("Started...");
      var watch = new Stopwatch();
      watch.Start();

      const float dx = 2.6F / zoom / width;
      const float dy = 2.6F / zoom / height;
      gpu.For(0, width * height, (index) => {
        //gpu.For(0, width, (x) => {
        //for (var y = 0; y < height; y++) {
        var y = index / height;
        var x = index - (y * height);
        var cr = xs + (x * dx);
        var ci = ys + (y * dy);

        var zr = 0F;
        var zi = 0F;

        float zrSquare;
        float ziSquare;

        byte i = 0;
        while (i < 255) {
          zrSquare = zr * zr;
          ziSquare = zi * zi;
          zi = (2 * zi * zr) + ci;
          zr = zrSquare - ziSquare + cr;
          i++;

          if (zrSquare + ziSquare > 4) {
            break;
          }
        }

        if (i == 255) {
          gpubits[index] = blackpixel;
        }
        else {
          gpubits[index] = (byte)(i * 25 % 256) | ((byte)(i * 3 % 256) << 8) | (((byte)(i % 256)) << 16) | ((byte)255 << 24);
        }
      });

      watch.Stop();
      Console.WriteLine($"Elapsed microseconds: {(double)watch.ElapsedTicks / Stopwatch.Frequency * 1000000}");

      var bits = Gpu.CopyToHost(gpubits);
      var bitsHandle = GCHandle.Alloc(bits, GCHandleType.Pinned);
      var bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, bitsHandle.AddrOfPinnedObject());
      bitmap.Save("b.png", ImageFormat.Png);
      bitmap.Dispose();
      bitsHandle.Free();
      Gpu.Free(gpubits);
    }

    #endregion Private Methods
  }
}