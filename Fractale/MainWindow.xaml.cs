using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Fractale {

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged {

    #region Public Properties

    public double MouseX {
      get {
        return mouseX;
      }
      set {
        if (mouseX != value) {
          mouseX = value;
          RaisePropertyChanged(nameof(MouseX));
        }
      }
    }

    public double MouseY {
      get {
        return mouseY;
      }
      set {
        if (mouseY != value) {
          mouseY = value;
          RaisePropertyChanged(nameof(MouseY));
        }
      }
    }

    public BitmapSource Picture {
      get {
        return picture;
      }
      set {
        if (picture != value) {
          picture = value;
          RaisePropertyChanged(nameof(Picture));
        }
      }
    }

    public MandelBrotArgs Args {
      get {
        return args;
      }
      set {
        if (args != value) {
          args = value;
          RaisePropertyChanged(nameof(Args));
        }
      }
    }

    public int PictureCount { get; set; }

    #endregion Public Properties

    #region Public Constructors

    public MainWindow() {
      System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
      InitializeComponent();
      DataContext = this;
      Loaded += MainWindow_Loaded;
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

    private BitmapSource picture;
    private MandelBrotArgs args;
    private double mouseX;
    private double mouseY;

    #endregion Private Fields

    #region Private Methods

    private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
      Args = new MandelBrotArgs {
        ZoomBase = 2D,
        ZoomFactor = 7,
        Iterations = 255,
        Size = new Size(512, 512)
      };
      Picture = MandelbrotService.GenerateBitmapSource(MandelbrotService.Calculate(Args), Args);
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
      Picture = MandelbrotService.GenerateBitmapSource(MandelbrotService.Calculate(Args), Args);
    }

    private void Image_MouseMove(object sender, MouseEventArgs e) {
      var pos = e.GetPosition((IInputElement)sender);
      SetMouseCoords(pos);
    }

    private void SetMouseCoords(System.Windows.Point pos) {
      MouseX = Args.Center.X + pos.X * args.RealZoom - args.Size.Width / 2 * args.RealZoom;
      MouseY = Args.Center.Y + pos.Y * args.RealZoom - args.Size.Width / 2 * args.RealZoom;
    }

    private void Image_MouseWheel(object sender, MouseWheelEventArgs e) {
      var pos = e.GetPosition((IInputElement)sender);
      SetMouseCoords(pos);
      Args.Center.X = MouseX;
      Args.Center.Y = MouseY;
      if (e.Delta > 0) {
        Args.ZoomFactor++;
      }
      else {
        Args.ZoomFactor--;
      }
      Picture = MandelbrotService.GenerateBitmapSource(MandelbrotService.Calculate(Args), Args);
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
      MandelbrotService.GenerateDiashow(Args, PictureCount, this);
    }

    #endregion Private Methods
  }
}