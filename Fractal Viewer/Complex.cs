namespace Fractal {

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

    #endregion Public Methods
  }
}