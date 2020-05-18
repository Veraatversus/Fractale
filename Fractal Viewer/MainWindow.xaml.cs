using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Fractal {

  /// <summary> Interaction logic for MainWindow.xaml </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged {
    private BitmapSource picture;
    private MandelBrotArgs args;
    private decimal mouseX;
    private decimal mouseY;

    public decimal MouseX {
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

    public decimal MouseY {
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

    public MainWindow() {
      System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
      InitializeComponent();
      DataContext = this;
      Loaded += MainWindow_Loaded;
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

    private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
      Args = new MandelBrotArgs {
        ZoomBase = 2,
        ZoomFactor = 7,
        Iterations = 255,
        Size = new Size(512, 512)
      };
      Picture = MandelbrotService.GenerateBitmapSource(MandelbrotService.Calculate(Args), Args);
      IsLoaded = true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public bool IsLoaded { get; set; }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
      if (IsLoaded) {
        IsLoaded = false;
        Picture = MandelbrotService.GenerateBitmapSource(MandelbrotService.Calculate(Args), Args);
        IsLoaded = true;
      }
    }

    private void Image_MouseMove(object sender, MouseEventArgs e) {
      var pos = e.GetPosition((IInputElement)sender);
      MouseX = Args.Center.X + (((decimal)pos.X - (Args.Size.Width / 2)) * Args.RealZoom);
      MouseY = Args.Center.Y - (((decimal)pos.Y - (Args.Size.Height / 2)) * Args.RealZoom);
    }

    private void Image_MouseWheel(object sender, MouseWheelEventArgs e) {
      var pos = e.GetPosition((IInputElement)sender);
      MouseX = Args.Center.X + (((decimal)pos.X - (Args.Size.Width / 2)) * Args.RealZoom);
      MouseY = Args.Center.Y - (((decimal)pos.Y - (Args.Size.Height / 2)) * Args.RealZoom);

      if (IsLoaded) {
        IsLoaded = false;
        Args.Center.X = MouseX;
        Args.Center.Y = MouseY;
        if (e.Delta > 0) {
          Args.ZoomFactor++;
        }
        else {
          Args.ZoomFactor--;
        }
        Picture = MandelbrotService.GenerateBitmapSource(MandelbrotService.Calculate(Args), Args);
        IsLoaded = true;
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
      MandelbrotService.GenerateDiashow(Args, PictureCount);
    }
  }
}