namespace Fractale {

  public class Size {

    #region Public Properties

    public double Width { get; set; }

    public double Height { get; set; }

    #endregion Public Properties

    #region Public Constructors

    public Size() {
    }

    public Size(double width, double height) {
      Width = width;
      Height = height;
    }

    #endregion Public Constructors
  }
}