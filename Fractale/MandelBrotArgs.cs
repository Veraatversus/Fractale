
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Fractale {
  public class MandelBrotArgs : INotifyPropertyChanged {
    private long zoomFactor;

    public MandelBrotArgs() {
      Center = new Point();
      Z = new Point();
      Size = new Size();
    }
    public Point Center { get; set; }
    public Point Z { get; set; }
    public Size Size { get; set; }
    public long ZoomFactor {
      get { return zoomFactor; }
      set {
        if (zoomFactor != value) {
          zoomFactor = value;
          RaisePropertyChanged(nameof(ZoomFactor));
        }
      }
    }
    public decimal ZoomBase { get; set; }
    public int Iterations { get; set; }
    public decimal RealZoom => 1M / Pow(ZoomBase, ZoomFactor);


    public event PropertyChangedEventHandler PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public static decimal Pow(decimal x, long n) {
      decimal result = 1;
      try {
        while (n-- > 0) {
          result *= x;
        }

        return result;
      }
      catch (Exception) {

       return decimal.MaxValue;
      }
      
    }
  }
}
