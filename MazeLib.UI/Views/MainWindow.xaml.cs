using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;

namespace MazeLib.UI.Views
{
    public class MainWindow : Window
    {
        private ZoomBorder zoomBorder;

        public MainWindow()
        {
            InitializeComponent();

            zoomBorder = this.Find<ZoomBorder>("zoomBorder");
            zoomBorder.KeyDown += ZoomBorder_KeyDown;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ZoomBorder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F)
            {
                zoomBorder.Fill();
            }

            if (e.Key == Key.U)
            {
                zoomBorder.Uniform();
            }

            if (e.Key == Key.R)
            {
                zoomBorder.Reset();
            }

            if (e.Key == Key.T)
            {
                zoomBorder.ToggleStretchMode();
                zoomBorder.AutoFit();
            }
        }
    }
}