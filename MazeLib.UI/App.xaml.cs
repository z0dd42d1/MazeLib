using Avalonia;
using Avalonia.Markup.Xaml;

namespace MazeLib.UI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}