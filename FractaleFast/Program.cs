using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FractaleFast {

  internal class Program {

    #region Public Methods
    public static void CalcMandelbrot() {
      const float xs = -2.1F;
      const float ys = -1.3F;
      const int zoom = 1;
      const int width = 4096;
      const int height = 4096;
       
      const int blackpixel = (byte)0 | (byte)0 << 8 | (byte)0 << 16 | (byte)255 << 24;

      var bits = new Int32[4096 * 4096];
      

      Console.WriteLine("Started...");
      var watch = new Stopwatch();
      watch.Start();

      var dx = 2.6F / zoom / width;
      var dy = 2.6F / zoom / height;

      
      for (int x = 0; x < width; x++) {
        for (int y = 0; y < height; y++) {
          var cr = xs + x * dx;
          var ci = ys + y * dy;

          var zr = 0F;
          var zi = 0F;

          float zrSquare;
          float ziSquare;

          byte i = 0;
          while (i < 255) {
            zrSquare = zr * zr;
            ziSquare = zi * zi;
            zi = 2 * zi * zr + ci;
            zr = zrSquare - ziSquare + cr;
            i++;

            if (zrSquare + ziSquare > 4) {
              break;
            }
          }

          if (i == 255) {
            bits[x + (y * width)] = blackpixel;
          }
          else {
            bits[x + (y * width)] = (byte)(i * 25 % 256) | (byte)(i * 3 % 256) << 8 | ((byte)(i % 256)) << 16 | (byte)255 << 24;
          }
        }
      }

      watch.Stop();
      Console.WriteLine($"Elapsed ms: {watch.ElapsedMilliseconds}");

      var bitsHandle = GCHandle.Alloc(bits, GCHandleType.Pinned);
      var bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, bitsHandle.AddrOfPinnedObject());
      bitmap.Save("b.png", ImageFormat.Png);
      bitmap.Dispose();
      bitsHandle.Free();
    }
    public static void CalcMandelbrotParallel() {
      const float xs = -2.1F;
      const float ys = -1.3F;
      const int zoom = 1;
      const int width = 4096;
      const int height = 4096;

      const int blackpixel = (byte)0 | (byte)0 << 8 | (byte)0 << 16 | (byte)255 << 24;

      var bits = new Int32[4096 * 4096];
      Console.WriteLine("Started...");
      var watch = new Stopwatch();
      watch.Start();

      var dx = 2.6F / zoom / width;
      var dy = 2.6F / zoom / height;

      Parallel.For(0, width, (x) => {
        for (int y = 0; y < height; y++) {
          var cr = xs + x * dx;
          var ci = ys + y * dy;

          var zr = 0F;
          var zi = 0F;

          float zrSquare;
          float ziSquare;

          byte i = 0;
          while (i < 255) {
            zrSquare = zr * zr;
            ziSquare = zi * zi;
            if (zrSquare + ziSquare > 4) {
              break;
            }

            zi = 2 * zi * zr + ci;
            zr = zrSquare - ziSquare + cr;
            i++;
          }

          if (i == 255) {
            bits[x + (y * width)] = blackpixel;
          }
          else {
            bits[x + (y * width)] = (byte)(i * 25 % 256) | (byte)(i * 3 % 256) << 8 | ((byte)(i % 256)) << 16 | (byte)255 << 24;
          }
        }
      });

      watch.Stop();
      Console.WriteLine($"Elapsed ms: {watch.ElapsedMilliseconds}");

      var bitsHandle = GCHandle.Alloc(bits, GCHandleType.Pinned);
      var bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, bitsHandle.AddrOfPinnedObject());
      bitmap.Save("b.png", ImageFormat.Png);
      bitmap.Dispose();
      bitsHandle.Free();
    }

    #endregion Public Methods

    #region Private Methods

    private static void Main(string[] args) {
      Task.Factory.StartNew(() => {
        for (int i = 0; i < 100; i++) {
          CalcMandelbrot();
        }
      }, TaskCreationOptions.LongRunning);
      Console.ReadLine();
    }

    #endregion Private Methods
  }
}