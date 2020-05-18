using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Fractal {

  public class Point : INotifyPropertyChanged {
    private decimal x;
    private decimal y;

    public decimal X {
      get {
        return x;
      }
      set {
        x = value;
        RaisePropertyChanged(nameof(X));
      }
    }

    public decimal Y {
      get { return y; }
      set { y = value; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}