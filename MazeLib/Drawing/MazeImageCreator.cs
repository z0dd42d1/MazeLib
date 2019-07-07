using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MazeLib
{
    public class MazeImageCreator
    {
        public static void CreateMazeImage(IMaze maze, string label, int tilesize, string targetPath)
        {
            using (var image = new Bitmap(tilesize * maze.GetWidth(), tilesize * maze.GetHeight()))
            {
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                SolidBrush redBrush = new SolidBrush(Color.Red);
                SolidBrush greenBrush = new SolidBrush(Color.SpringGreen);
                SolidBrush currentBrush = blackBrush;
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.FillRectangle(whiteBrush, new Rectangle(0, 0, maze.GetWidth() * tilesize, maze.GetHeight() * tilesize));

                    for (int x = 0; x <= maze.GetWidth() - 1; x++)
                    {
                        for (int y = 0; y <= maze.GetHeight() - 1; y++)
                        {
                            switch (maze.GetMazeTypeOnPos(x, y))
                            {
                                case MazeFieldType.Wall:
                                    currentBrush = blackBrush;
                                    break;

                                case MazeFieldType.Exit:
                                    currentBrush = greenBrush;
                                    break;

                                case MazeFieldType.Entrance:
                                    currentBrush = redBrush;
                                    break;

                                default:
                                    continue;
                            }

                            // Create rectangle.
                            Rectangle rect = new Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);

                            // Fill rectangle to screen.
                            graphics.FillRectangle(currentBrush, rect);
                        }
                    }
                    image.Save(Path.Combine(targetPath, $"{label}.png"), ImageFormat.Png);
                }
            }
        }
    }
}