using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Fractale {

  public class Point : INotifyPropertyChanged {

    #region Public Properties

    public double X {
      get {
        return x;
      }
      set {
        if (x != value) {
          x = value;
          RaisePropertyChanged(nameof(X));
        }
      }
    }

    public double Y {
      get {
        return y;
      }
      set {
        if (y != value) {
          y = value;
          RaisePropertyChanged(nameof(Y));
        }
      }
    }

    #endregion Public Properties

    #region Public Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Public Events

    #region Public Methods

    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion Public Methods

    #region Private Fields

    private double x;
    private double y;

    #endregion Private Fields
  }
}