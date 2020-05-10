using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fractale {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged {
    private BitmapSource picture;
    private MandelBrotArgs args;
    private double mouseX;
    private double mouseY;

    public double MouseX {
      get { return mouseX; }
      set {
        if (mouseX != value) {
          mouseX = value;
          RaisePropertyChanged(nameof(MouseX));
        }
      }
    }
    public double MouseY {
      get { return mouseY; }
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
      get { return picture; }
      set {
        if (picture != value) {
          picture = value;
          RaisePropertyChanged(nameof(Picture));
        }
      }
    }
    public MandelBrotArgs Args {
      get { return args; }
      set {
        if (args != value) {
          args = value;
          RaisePropertyChanged(nameof(Args));
        }
      }
    }
    public int PictureCount { get; set; }
    private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
      //var m = new Mandelbrot(1920, 1080);
      //var m = new Mandelbrot(1280, 720);
      Args = new MandelBrotArgs {
        ZoomBase = 2D,
        ZoomFactor = 7,
        Iterations = 255,
        Size = new Size(512, 512)
      };
      Picture = MandelbrotService.GenerateBitmapSource(MandelbrotService.Calculate(Args), Args);
      //MandelbrotService.SavePicture(Picture);
      //this.Image.Source = Image. Picture;
      //var m = new Mandelbrot(10000, 10000);
      //m.Calculate();

    }
    public event PropertyChangedEventHandler PropertyChanged;
    public void RaisePropertyChanged([CallerMemberName]string name = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
      Picture = MandelbrotService.GenerateBitmapSource(MandelbrotService.Calculate(Args), Args);
    }

    private void Image_MouseMove(object sender, MouseEventArgs e) {
      var pos = e.GetPosition((IInputElement)sender);
      MouseX = Args.Center.X + pos.X * args.RealZoom - args.Size.Width / 2 * args.RealZoom;
      MouseY = Args.Center.Y + pos.Y * args.RealZoom - args.Size.Width / 2 * args.RealZoom;
    }

    private void Image_MouseWheel(object sender, MouseWheelEventArgs e) {
      var pos = e.GetPosition((IInputElement)sender);
      MouseX = Args.Center.X + pos.X * args.RealZoom - args.Size.Width / 2 * args.RealZoom;
      MouseY = Args.Center.Y + pos.Y * args.RealZoom - args.Size.Width / 2 * args.RealZoom;
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
  }
}
