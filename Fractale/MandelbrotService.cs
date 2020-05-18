using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Fractale {

  public static class MandelbrotService {

    #region Public Methods

    public static int[] Calculate(MandelBrotArgs args) {
      //Parallel.For(0L, 300L, (zoomb) => {
      var zoom = /*-1 **/ args.RealZoom;

      var picture = new int[(long)(args.Size.Width * args.Size.Height)];
      //(var dx, var dy, var xs, var ys) = (2.6D / zoom / Width, 2.6D / zoom / Height, -0.99D - 2.6D /zoom, -0.30D - 2.6D / zoom);   // -2.1F, -1.3F);

      //var ran = 1D;
      //var dx = (ran / Width / zoom)  /*/ zoom*/ ;
      //var dx = zoom;
      //var dy = (ran / Height / zoom) /*/ zoom*/;
      //var dy = zoom;
      //var xs =-0.75D/*-1.04180483110546D*/ /*- 1.62917D*/ /*-0.77568377D*/ /*-0.745433D*/;
      //var ys = 0.1/*0.346342664848392D*/ /*- 0.0203968D*/ /*0.13646737D*/ /*-0.113021D*/;
      Parallel.For(0L, (long)args.Size.Height, (height) => {
        for (long width = 0; width < (long)args.Size.Width; width++) {
        //Parallel.For(0L, (long)args.Size.Width, (width) => {
          //var c = new Complex(xs - (((Width / 2))* dx) + (width * dx),
          //                    ys - (((Height / 2)) * dy) + (height * dy));
          var c = new Complex(args.Center.X + (width - args.Size.Width / 2) * zoom/* -   zoom /2*/,
                              args.Center.Y - (height - args.Size.Height / 2) * zoom/*-  zoom /2*/);

          picture[(long)(width + (height * args.Size.Width))] = MandelBrotFunction(c, new Complex(args.Z.X, args.Z.Y), args.Iterations);
          //picture[(long)(width + (height * args.Size.Width))] = GenerateColor(colorindex);
        }/*);*/
        //});

        //SavePicture(bitmap, "pic" + zoomb + ".png");
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

    private static BitmapSource ConvertToBitmapSource(MandelBrotArgs args, Bitmap pic) {
      return Imaging.CreateBitmapSourceFromHBitmap(pic.GetHbitmap(),
                                                                     IntPtr.Zero,
                                                                     Int32Rect.Empty,
                                                                     BitmapSizeOptions.FromWidthAndHeight((int)args.Size.Width,
                                                                                                          (int)args.Size.Height));
    }

    public static void GenerateDiashow(MandelBrotArgs args, int pictureCount, MainWindow window) {
      //Enumerable.Range((int)args.ZoomFactor, pictureCount).AsParallel().AsOrdered().ForAll((zoomfactor) => {
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
        //Application.Current.Dispatcher.Invoke(() => { window.Picture = ConvertToBitmapSource(zoomArgs, bitmap); });
        SavePicture(bitmap, $"pic_{zoomfactor}.png");
      });
    }
    public static void SavePicture(Bitmap bitmap, string path = "pic.png") {
      bitmap.Save(path, ImageFormat.Png);
    }

    #endregion Public Methods

    #region Private Methods
    public static int MandelBrotFunction(Complex c, Complex z = default, int iterations = 255) {
      var i = 0;
      var u = z;
      while (i < iterations) {
        var zrSquare = u.Real * u.Real;
        var ziSquare = u.Imaginary * u.Imaginary;
        if (zrSquare + ziSquare > 4) {
          break;
        }
        u = new Complex((zrSquare - ziSquare + c.Real), (2 * u.Imaginary * u.Real + c.Imaginary));
        i++;
      }
      return i;
    }

    #endregion Private Methods
  }
}