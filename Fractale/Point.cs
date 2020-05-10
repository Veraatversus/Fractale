using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fractale {
  public class Point : INotifyPropertyChanged {
    private double x;
    private double y;

    public double X {
      get { return x; }
      set {
        if (x != value) {
          x = value;
          RaisePropertyChanged(nameof(X));
        }
      }
    }
    public double Y {
      get { return y; }
      set {
        if (y != value) {
          y = value;
          RaisePropertyChanged(nameof(Y));
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string name = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
                     
  }
}
