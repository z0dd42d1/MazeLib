using MazeLib;
using MazeLib.Base;
using MazeLib.TileMapAlgorithms;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeLibConsoleCore
{
    public static partial class Program
    {
        private static void Main(string[] args)
        {
            var configB = new ConfigurationBuilder();
            var configRoot = configB.AddCommandLine(args).Build();

            var console = new MazeConsoleTools();
            console.CreateDemoImageAndGifs(configRoot);
        }

        private static async Task CreateMazeImageAndGif(IMazeGenAlgorithm i)
        {
            MazeBuilder mazeBuilder = new MazeBuilder();
            var maze = mazeBuilder
                .SetMazeAlgorithm(i)
                .SetDrawCallback(m =>
                {
                    if (i is RandomizedPrims)
                    {
                        // drawMaze.TransformMaze(m);
                        // MazeImageCreator.CreateMazeImage(drawMaze, i.GetName(), 10, "./");
                    }
                })
                .RecordMazeTransformationSteps(true)
                .SetMazeDimensions(55, 55)
                .Build();

            await MazeImageCreator.CreateMazeAnimationGifAsync(mazeBuilder.GetMazeTransformationSteps(), mazeBuilder.GetInitializedMaze, i.GetName(), 5, "./").ConfigureAwait(false);

            MazeImageCreator.CreateMazeImage(maze, i.GetName(), 5, "./");

            Console.WriteLine($"Finished maze image and gif creation for algorithm={i.GetName()}.");
        }
    }
}