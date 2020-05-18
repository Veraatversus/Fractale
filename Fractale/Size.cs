using System;
using System.Collections.Generic;
using System.Text;

namespace Fractale {
  public class Size {
    public Size() {

    }
    public Size(decimal width, decimal height) {
      Width = width;
      Height = height;
    }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
  }
}
