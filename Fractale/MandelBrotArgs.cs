using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Fractale {

  public class MandelBrotArgs : INotifyPropertyChanged {

    #region Public Properties

    public Point Center { get; set; }
    public Point Z { get; set; }
    public Size Size { get; set; }

    public long ZoomFactor {
      get {
        return zoomFactor;
      }
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

    #endregion Public Properties

    #region Public Constructors

    public MandelBrotArgs() {
      Center = new Point();
      Z = new Point();
      Size = new Size();
    }

    #endregion Public Constructors

    #region Public Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Public Events

    #region Public Methods

    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion Public Methods

    #region Private Fields

    private long zoomFactor;

    #endregion Private Fields
  }
}