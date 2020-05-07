using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Fractale {

  public static class MandelbrotService {

    #region Public Methods

    public static int[] Calculate(MandelBrotArgs args) {
      var zoom = args.RealZoom;
      var picture = new int[(long)(args.Size.Width * args.Size.Height)];
      Parallel.For(0L, (long)args.Size.Height, (height) => {
        Parallel.For(0L, (long)args.Size.Width, (width) => {
          var c = new Complex(args.Center.X + (width - args.Size.Width / 2) * zoom,
                              args.Center.Y + (height - args.Size.Height / 2) * zoom);

          picture[(long)(width + (height * args.Size.Width))] = MandelBrotFunction(c, new Complex(args.Z.X, args.Z.Y), args.Iterations);
        });
      });
      return picture;
    }

    public static Color GenerateColor(int number) {
      return Color.FromArgb(255, number * 25 % 256, number * 3 % 256, number % 256);
    }

    public static Bitmap GenerateBitmap(int[] picture, MandelBrotArgs args) {
      var pic = new Bitmap((int)args.Size.Width, (int)args.Size.Height, PixelFormat.Format32bppArgb);

      for (int x = 0; x < (long)args.Size.Width; x++) {
        for (int y = 0; y < (long)args.Size.Height; y++) {
          var arrayIndex = y * (int)args.Size.Width + x;
          var color = picture[arrayIndex];
          pic.SetPixel(x, y, GenerateColor(color));
        }
      }

      return pic;
    }

    public static BitmapSource GenerateBitmapSource(int[] picture, MandelBrotArgs args) {
      var pic = GenerateBitmap(picture, args);
      var bitmapSource = ConvertToBitmapSource(args, pic);
      return bitmapSource;
    }

    public static void GenerateDiashow(MandelBrotArgs args, int pictureCount, MainWindow window) {
      Parallel.For(args.ZoomFactor, args.ZoomFactor + pictureCount, (zoomfactor) => {
        var zoomArgs = new MandelBrotArgs {
          Center = new Point { X = args.Center.X, Y = args.Center.Y },
          Iterations = args.Iterations,
          Size = new Size { Width = args.Size.Width, Height = args.Size.Height },
          Z = new Point { X = args.Z.X, Y = args.Z.Y },
          ZoomBase = args.ZoomBase,
          ZoomFactor = zoomfactor
        };
        var bitmap = GenerateBitmap(Calculate(zoomArgs), zoomArgs);
        SavePicture(bitmap, $"pic_{zoomfactor}.png");
      });
    }

    public static void SavePicture(Bitmap bitmap, string path = "pic.png") {
      bitmap.Save(path, ImageFormat.Png);
    }

    public static int MandelBrotFunction(Complex c, Complex z = default, int iterations = 255) {
      var i = 0;
      var u = z;
      while (i < iterations && Complex.Abs(u) < 2) {
        u = (u * u) + c;
        i++;
      }
      return i;
    }

    #endregion Public Methods

    #region Private Methods

    private static BitmapSource ConvertToBitmapSource(MandelBrotArgs args, Bitmap pic) {
      return Imaging.CreateBitmapSourceFromHBitmap(pic.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                                                   BitmapSizeOptions.FromWidthAndHeight((int)args.Size.Width, (int)args.Size.Height));
    }

    #endregion Private Methods
  }
}