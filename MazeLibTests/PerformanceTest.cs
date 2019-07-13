using MazeLib.Base;
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
using MazeLib.TileMapAlgorithms;

namespace MazeLibTests
{
    public class PerformanceTest : UnittestSerilogBase
    {
        public PerformanceTest(ITestOutputHelper output) : base(output)
        {
        }

        public object KnownMazeTypes { get; private set; }

        [Fact]
        public void TestPerformanceOfGenAlgos_WithSize150x150_DoesNotThrow()
        {
            var mazeGenAlgos = KnownMazesTypes.GetAllMazeAlgos();
            Stopwatch sw = new Stopwatch();

            foreach (IMazeGenAlgorithm algo in mazeGenAlgos)
            {
                var maze = new TileMapMaze(150, 150);
                algo.SetCurrentMaze(maze);

                double[] tests = new double[10];
                for (int i = 10 - 1; i >= 0; i--)
                {
                    sw.Start();

                    algo.GenerateMazeWithTranformationList();

                    sw.Stop();
                    tests[i] = sw.ElapsedTicks;
                    sw.Reset();
                }

                var testsEnumerable = tests.AsEnumerable<double>();
                double median = testsEnumerable.Median();

                Log.Logger.Information($"GenerateMazeFullSize {algo.GetName()} {maze.GetWidth()}x{maze.GetHeight()} Elapsed={TimeSpan.FromTicks(((long)median))}");
            }
        }

        [Fact]
        public void TestPerformanceRecordTransformation_WithPrimAndSize500x500_DoesNotThrow()
        {
            MazeBuilder mazeBuilder = new MazeBuilder()
                .RecordMazeTransformationSteps(true)
                .SetMazeAlgorithm(new RandomizedPrims())
                .SetMazeDimensions(500, 500);

            double median = MedianPerfTest(mazeBuilder, 5);

            Log.Logger.Information($"Randomized Prims with RecordTransformationSteps==true Elapsed={TimeSpan.FromTicks(((long)median))}");
            Log.Logger.Information($"RecordedTransformationSteps={mazeBuilder.MazeTransformationSteps.Count}");

            Assert.True(mazeBuilder.MazeTransformationSteps.Count > 1);
        }

        [Fact]
        public void TestPerformanceOnlyMaze_WithPrimAndSize500x500_DoesNotThrow()
        {
            MazeBuilder mazeBuilder = new MazeBuilder()
                .RecordMazeTransformationSteps(false)
                .SetMazeAlgorithm(new RandomizedPrims())
                .SetMazeDimensions(500, 500);

            double median = MedianPerfTest(mazeBuilder, 5);

            Log.Logger.Information($"Randomized Prims with RecordTransformationSteps==false Elapsed={TimeSpan.FromTicks(((long)median))}");
        }

        private static double MedianPerfTest(MazeBuilder mazeBuilder, int samples)
        {
            Stopwatch sw = new Stopwatch();
            double[] tests = new double[samples];
            for (int i = samples - 1; i >= 0; i--)
            {
                sw.Start();

                mazeBuilder.Build();

                sw.Stop();
                tests[i] = sw.ElapsedTicks;
                sw.Reset();
            }

            var testsEnumerable = tests.AsEnumerable<double>();
            double median = testsEnumerable.Median();
            return median;
        }
    }
}