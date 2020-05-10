
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
    public double ZoomBase { get; set; }
    public int Iterations { get; set; }
    public double RealZoom => -1 / Math.Pow(ZoomBase, ZoomFactor);


    public event PropertyChangedEventHandler PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}
