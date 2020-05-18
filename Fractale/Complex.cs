using System;

namespace Fractale {

  public class Complex {

    #region Public Fields

    public decimal Real;

    public decimal Imaginary;

    #endregion Public Fields

    #region Public Constructors

    public Complex(decimal real, decimal imaginary) {
      Real = real;
      Imaginary = imaginary;
    }

    #endregion Public Constructors

    #region Public Methods

    public static Complex operator *(Complex left, Complex right) {
      // Multiplication:  (a + bi)(c + di) = (ac -bd) + (bc + ad)i
      var result_Realpart = (left.Real * right.Real) - (left.Imaginary * right.Imaginary);
      var result_Imaginarypart = (left.Imaginary * right.Real) + (left.Real * right.Imaginary);
      return new Complex(result_Realpart, result_Imaginarypart);
    }

    public static Complex operator +(Complex left, Complex right) {
      return (new Complex((left.Real + right.Real), (left.Imaginary + right.Imaginary)));
    }

    //public static double Abs(Complex value) {
    //  //if (Double.IsInfinity(value.Real) || Double.IsInfinity(value.Imaginary)) {
    //  //  return Double.PositiveInfinity;
    //  //}

    //  // |value| == sqrt(a^2 + b^2) sqrt(a^2 + b^2) == a/a * sqrt(a^2 + b^2) = a * sqrt(a^2/a^2 +
    //  // b^2/a^2) Using the above we can factor out the square of the larger component to dodge overflow.

    //  var c = Math.Abs(value.Real);
    //  var d = Math.Abs(value.Imaginary);

    //  if (c > d) {
    //    var r = d / c;
    //    return c * Math.Sqrt(1.0D + r * r);
    //  }
    //  else if (d == 0) {
    //    return c;  // c is either 0.0 or NaN
    //  }
    //  else {
    //    var r = c / d;
    //    return d * Math.Sqrt(1.0D + r * r);
    //  }
    //}

    //public static decimal Sqrt(decimal x, decimal epsilon = 0.0M) {
    //  if (x < 0)
    //    throw new OverflowException("Cannot calculate square root from a negative number");

    //  decimal current = (decimal)Math.Sqrt((double)x), previous;
    //  do {
    //    previous = current;
    //    if (previous == 0.0M)
    //      return 0;
    //    current = (previous + x / previous) / 2;
    //  }
    //  while (Math.Abs(previous - current) > epsilon);
    //  return current;
    //}

    #endregion Public Methods
  }
}