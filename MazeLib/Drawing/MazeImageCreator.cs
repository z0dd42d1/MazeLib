using AnimatedGif;
using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MazeLib
{
    public class MazeImageCreator
    {
        public static Image GetMazeImage(IMaze maze, int tilesize)
        {
            var image = new Bitmap(tilesize * maze.GetWidth(), tilesize * maze.GetHeight());

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
                // Fix output image orientation, the drawing libs use the upper left corner as 0,0 with y going down and x going right.
                image.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            return image;
        }

        public static void CreateMazeImage(IMaze maze, string label, int tilesize, string targetPath)
        {
            using (Image image = GetMazeImage(maze, tilesize))
            {
                image.Save(Path.Combine(targetPath, $"{label}.png"), ImageFormat.Png);
            }
        }

        public static async Task CreateMazeAnimationGifAsync(IList<MazeTransformationStep> mazeTransformationSteps, IMaze maze, string label, int tilesize, string targetPath)
        {
            int stepsPerImage = 30;
            int index = 0;

            using (var gif = new AnimatedGifCreator($"{label}.gif", 16)) // 16ms == 60fps
            {
                await gif.AddFrameAsync(GetMazeImage(maze, tilesize), delay: -1, quality: GifQuality.Bit8);
                foreach (MazeTransformationStep step in mazeTransformationSteps)
                {
                    maze.TransformMaze(step);
                    index++;
                    if (index % stepsPerImage == 0)
                    {
                        await gif.AddFrameAsync(GetMazeImage(maze, tilesize), delay: -1, quality: GifQuality.Bit8);
                    }
                }
                var finished = GetMazeImage(maze, tilesize);
                for (int i = 40 - 1; i >= 0; i--)
                {
                    await gif.AddFrameAsync(finished, delay: -1, quality: GifQuality.Bit8);
                }
            }
        }
    }
}