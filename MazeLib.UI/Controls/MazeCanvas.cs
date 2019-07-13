using Avalonia.Controls;
using Avalonia.Media;
using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeLib.UI.Controls
{
    internal class MazeCanvas : Canvas
    {
        public IMaze Maze { get; set; }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
        }
    }
}