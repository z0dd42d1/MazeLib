using MazeLib;
using MazeLib.Base;
using MazeLib.MazeGenAlgos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeLibConsoleCore
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configB = new ConfigurationBuilder();
            var configRoot = configB.AddCommandLine(args).Build();

            if (configRoot["CreateDemoImages"] != null)
            {
                bool run = configRoot["InfiniteDemo"] != null ? true : false;

                while (run)
                {
                    var list = KnownMazesTypes.GetAllMazeAlgos();
                    foreach (IMazeGenAlgorithm i in list)
                    {
                        MazeBuilder mazeBuilder = new MazeBuilder();
                        var maze = mazeBuilder
                            .SetMazeAlgorithm(i)
                            .SetMazeDimensions(61, 61)
                            .Build();

                        MazeImageCreator.CreateMazeImage(maze, i.GetName(), 10, "./");
                    }
                    Console.ReadKey();
                }
            }
        }

        private static async void StartCommandLineTestOutput()
        {
            // Generate Maze
            try
            {
                var maze = new TileMapMaze(30, 30);
                var algo = new DepthFirst();
                algo.SetCurrentMaze(maze);

                maze.OverrideAllMazeFields();
                while (true)
                {
                    algo.GenerateMazeFullSize();

                    MazeImageCreator.CreateMazeImage(algo.GetCurrentMaze(), algo.GetName(), 10, "./");

                    //PrintMaze(algo.GetGeneratedMaze());
                    //Console.ReadKey();
                    //PrintMaze(algo.InitializeMaze(), transforms);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            Console.ReadKey();
        }

        private static void PrintMaze(TileMapMaze maze, IList<MazeTransformationStep> transforms)
        {
            foreach (MazeTransformationStep s in transforms)
            {
                maze.TransformMaze(s);
                PrintMaze(maze);
                Thread.Sleep(1);
            }
        }

        private static void PrintMaze(TileMapMaze maze)
        {
            Console.Clear();
            StringBuilder stringBuilder = new StringBuilder();

            for (int h = 0; h < maze.GetHeight(); h++)
            {
                for (int w = 0; w < maze.GetWidth(); w++)
                {
                    switch (maze.GetMazeTypeOnPos(w, h))
                    {
                        case MazeFieldType.Corridor:
                            stringBuilder.Append(' ');
                            break;

                        case MazeFieldType.Wall:
                            stringBuilder.Append('■');
                            break;

                        case MazeFieldType.Entrance:
                            stringBuilder.Append('▦');
                            break;

                        case MazeFieldType.Exit:
                            stringBuilder.Append('▣');
                            break;
                    }
                }
                stringBuilder.Append(Environment.NewLine);
            }

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}