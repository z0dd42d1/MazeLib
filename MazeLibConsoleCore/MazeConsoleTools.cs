using MazeLib.Base;
using MazeLib.TileMapAlgorithms;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazeLibConsoleCore
{
    public partial class Program
    {
        public class MazeConsoleTools
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
                await CreateMazeImageAndGif(i).ConfigureAwait(false);
            }
        }
    }
}