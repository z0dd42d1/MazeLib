using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using MazeLib.Base;
using MazeLib.TileMapAlgorithms;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.UI.Controls
{
    public class MazeCanvas : Canvas
    {
        public static readonly DirectProperty<MazeCanvas, int> MazeHeightProperty =
        AvaloniaProperty.RegisterDirect<MazeCanvas, int>(nameof(MazeHeight), o => o.MazeHeight, (o, v) => o.MazeHeight = v);

        public static readonly DirectProperty<MazeCanvas, int> MazeWidthProperty =
        AvaloniaProperty.RegisterDirect<MazeCanvas, int>(nameof(MazeWidth), o => o.MazeWidth, (o, v) => o.MazeWidth = v);

        public readonly AvaloniaProperty<bool> busy =
       AvaloniaProperty.Register<MazeCanvas, bool>(nameof(busy));

        private HashSet<MazeTransformationStep> StepsToDraw = new HashSet<MazeTransformationStep>();

        private readonly double _tilewidth = 10;
        private ISolidColorBrush _mazeTileBrush;
        private Avalonia.Media.Pen _mazeTilePen;
        private int _MazeHeight = 100;
        private int _MazeWidth = 50;

        private IMaze _Maze;
        private Avalonia.Media.Imaging.Bitmap _MazeImage;

        public int MazeHeight
        {
            get { return _MazeHeight; }
            set { this.SetAndRaise(MazeHeightProperty, ref _MazeHeight, value); }
        }

        public int MazeWidth
        {
            get { return _MazeWidth; }
            set { this.SetAndRaise(MazeWidthProperty, ref _MazeWidth, value); }
        }

        public bool BusyGeneratingMaze { get; set; }
        public Subject<bool> BusyGeneratingMaze2 { get; set; }

        public MazeCanvas() : base()
        {
            _mazeTileBrush = Avalonia.Media.Brushes.Black;
            _mazeTilePen = new Avalonia.Media.Pen(_mazeTileBrush);

            BusyGeneratingMaze2 = new Subject<bool>();
            BusyGeneratingMaze2.OnNext(false);
            GenerateMazeBind = ReactiveCommand.CreateFromTask(GenerateMaze, canExecute: BusyGeneratingMaze2);
            BusyGeneratingMaze2.OnNext(false);
            BusyGeneratingMaze = true;
        }

        private async Task GenerateMaze()
        {
            BusyGeneratingMaze2.OnNext(false);
            await Task.Run(() =>
            {
                var builder = new MazeBuilder()
                .SetMazeAlgorithm(this.SelectedMazeGenAlgorithm)
                .SetMazeDimensions(MazeWidth, MazeHeight)
                .RecordMazeTransformationSteps();
                this._Maze = builder.Build();
                var image = MazeImageCreator.GetMazeImage(_Maze, 10);

                this._MazeImage = ConvertToAvaloniaImage(image);
                // this.StepsToDraw = builder.GetMazeTransformationSteps();

                Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);

                BusyGeneratingMaze2.OnNext(true);
            }).ConfigureAwait(true);
        }

        private Avalonia.Media.Imaging.Bitmap ConvertToAvaloniaImage(System.Drawing.Bitmap image)
        {
            var stream = new MemoryStream();

            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            image.Dispose();

            stream.Seek(0, SeekOrigin.Begin);

            var avaloniaImage = new Avalonia.Media.Imaging.Bitmap(stream);
            stream.Dispose();
            return avaloniaImage;
        }

        public IList<IMazeGenAlgorithm> MazeGenAlgorithms { get { return KnownMazesTypes.GetAllMazeAlgos(); } }

        public IMazeGenAlgorithm SelectedMazeGenAlgorithm { get; set; }

        public ReactiveCommand<Unit, Unit> GenerateMazeBind { get; set; }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (_MazeImage != null)
                context.DrawImage(_MazeImage, 100,
                    new Rect(_MazeImage.Size),
                    new Rect(_MazeImage.Size));
            /*
            if (_Maze != null)
            {
                for (int y = _Maze.GetHeight() - 1; y >= 0; y--)
                {
                    for (int x = _Maze.GetWidth() - 1; x >= 0; x--)
                    {
                        if (_Maze.GetMazeTypeOnPos(x, y) == MazeFieldType.Wall)
                            context.FillRectangle(_mazeTileBrush, new Rect(x * _tilewidth, y * _tilewidth, _tilewidth, _tilewidth));
                    }
                }
            }

            foreach (MazeTransformationStep s in StepsToDraw)
            {
                DrawTransformationStep(s, context);
            }
             */
        }

        private void DrawTransformationStep(MazeTransformationStep currentStep, DrawingContext context)
        {
            context.FillRectangle(_mazeTileBrush, new Rect(currentStep.coordinate.X * _tilewidth, currentStep.coordinate.Y * _tilewidth, _tilewidth, _tilewidth));
        }
    }
}