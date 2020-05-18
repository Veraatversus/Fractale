using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Fractal {

  public class DirectBitmap : IDisposable {

    #region Public Properties

    public Bitmap Bitmap { get; private set; }
    public Int32[] Bits { get; private set; }
    public bool Disposed { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }

    #endregion Public Properties

    #region Public Constructors

    public DirectBitmap(int width, int height) {
      Width = width;
      Height = height;
      Bits = new Int32[width * height];
      BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
      Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
    }

    #endregion Public Constructors

    #region Public Methods

    public void SetPixel(int x, int y, Color colour) {
      var col = colour.ToArgb();
      SetPixel(x, y, col);
    }

    public void SetPixel(int x, int y, int colour) {
      var index = x + (y * Width);
      Bits[index] = colour;
    }

    public Color GetPixel(int x, int y) {
      var index = x + (y * Width);
      var col = Bits[index];
      var result = Color.FromArgb(col);

      return result;
    }

    public void Dispose() {
      if (Disposed)
        return;
      Disposed = true;
      Bitmap.Dispose();
      BitsHandle.Free();
    }

    #endregion Public Methods

    #region Protected Properties

    protected GCHandle BitsHandle { get; private set; }

    #endregion Protected Properties
  }
}