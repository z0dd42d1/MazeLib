using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace MazeLib.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            GenerateMazeBind = ReactiveCommand.Create(GenerateMaze);
        }

        private void GenerateMaze()
        {
            throw new NotImplementedException();
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public ReactiveCommand<Unit, Unit> GenerateMazeBind { get; private set; }
    }
}