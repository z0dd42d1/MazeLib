using MazeLib;
using MazeLib.Base;
using MazeLib.TileMapAlgorithms;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeLibConsoleCore
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var configB = new ConfigurationBuilder();
            var configRoot = configB.AddCommandLine(args).Build();

            var console = new MazeConsole();
            console.CreateDemoImageAndGifs(configRoot);
        }

        public class MazeConsole
        {
            public void CreateDemoImageAndGifs(IConfigurationRoot configRoot)
            {
                if (configRoot["CreateDemoImages"] != null)
                {
                    bool run = configRoot["InfiniteDemo"] != null ? true : false;

                    while (run)
                    {
                        var list = KnownMazesTypes.GetAllMazeAlgos();
                        var tasks = new List<Task>();
                        foreach (IMazeGenAlgorithm i in list)
                        {
                            tasks.Add(CreateImageAndGifForAlgoAsync(i));
                        }
                        Task.WaitAll(tasks.ToArray());

                        Console.WriteLine("Tap key for another batch.");
                        Console.ReadKey();
                    }
                }
            }

            private static async Task CreateImageAndGifForAlgoAsync(IMazeGenAlgorithm i)
            {
                var drawMaze = new TileMapMaze(55, 55);
                drawMaze.OverrideAllMazeFields(MazeFieldType.Wall);
                await CreateMazeImageAndGif(i);
            }
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

            await MazeImageCreator.CreateMazeAnimationGifAsync(mazeBuilder.GetMazeTransformationSteps(), mazeBuilder.GetInitializedMaze, i.GetName(), 5, "./");

            MazeImageCreator.CreateMazeImage(maze, i.GetName(), 5, "./");

            Console.WriteLine($"Finished maze image and gif creation for algorithm={i.GetName()}.");
        }
    }
}