using MazeLib.Base;
using MazeLib.MazeGenAlgos;
using MazeLibTests.Base;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using MathNet.Numerics.Statistics;

namespace MazeLibTests
{
    public class PerformanceTest : UnittestSerilogBase
    {
        public PerformanceTest(ITestOutputHelper output) : base(output)
        {
        }

        public object KnownMazeTypes { get; private set; }

        [Fact]
        public void TestPerformanceOfGenAlgos_WithSize100x100_DoesNotThrow()
        {
            var mazeGenAlgos = KnownMazesTypes.GetAllMazeAlgos();
            Stopwatch sw = new Stopwatch();

            foreach (IMazeGenAlgorithm algo in mazeGenAlgos)
            {
                var maze = new TileMapMaze(500, 500);
                algo.SetCurrentMaze(maze);

                double[] tests = new double[10];
                for (int i = 10 - 1; i >= 0; i--)
                {
                    sw.Start();

                    var transforms = algo.GenerateMazeFullSize();

                    sw.Stop();
                    tests[i] = sw.ElapsedTicks;
                    sw.Reset();
                }

                var testsEnumerable = tests.AsEnumerable<double>();
                double median = testsEnumerable.Median();

                Log.Logger.Information($"GenerateMazeFullSize {algo.GetName()} {maze.GetWidth()}x{maze.GetHeight()} Elapsed={TimeSpan.FromTicks(((long)median))}");
            }
        }
    }
}