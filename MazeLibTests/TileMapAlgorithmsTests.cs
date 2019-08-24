using MazeLib.Base;
using MazeLib.TileMapAlgorithms;
using MazeLibTests.Base;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace MazeLibTests
{
    // Generate a neutral object array for each algorithm and return it as an enumerable to use it in ClassData + Theory Attribute.
    public class TestDataGeneratorCollection : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = GetArrayOfMazeAlgos();

        private static List<object[]> GetArrayOfMazeAlgos()
        {
            var list = new List<object[]>();

            var mazes = KnownMazesTypes.GetAllMazeAlgos().ToArray<object>();

            var dimensonList = new List<(int, int)>()
            {
                (10,10), // even, even
                (11,11), // odd, odd
                (50,80), // even, even(bigger)
                (80,50), // even(bigger), even
                (17,10), // odd(bigger), even
                (10,17), // even, odd(bigger)
            };

            foreach (IMazeGenAlgorithm algorithm in mazes)
            {
                foreach ((int, int) d in dimensonList)
                {
                    list.Add(new object[] { algorithm, d });
                }
            }

            return list;
        }

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class TileMapAlgorithmsTests : UnittestSerilogBase
    {
        public TileMapAlgorithmsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [ClassData(typeof(TestDataGeneratorCollection))]
        public void GenerateFullSizeMaze_GeneratesMaze_DoesNotThrow(IMazeGenAlgorithm mazeGenAlgorithm, (int, int) dimensions)
        {
            Log.Logger.Information($"Test combination: {mazeGenAlgorithm?.GetName()},x={dimensions.Item1},y={dimensions.Item2}");

            var mazeBuilder = new MazeBuilder()
                .SetMazeAlgorithm(mazeGenAlgorithm)
                .SetMazeDimensions(dimensions.Item1, dimensions.Item2);

            Assert.NotNull(mazeBuilder.Build());
            //TODO Test maze is solvable
        }

        [Theory]
        [ClassData(typeof(TestDataGeneratorCollection))]
        public void GenerateFullSizeMaze_WithRecordMazeTransformationStepsTrue_MazeTransformationStepsContainOneExitAndEntry(IMazeGenAlgorithm mazeGenAlgorithm, (int, int) dimensions)
        {
            Log.Logger.Information($"Test combination: {mazeGenAlgorithm?.GetName()},x={dimensions.Item1},y={dimensions.Item2}");

            var mazeBuilder = new MazeBuilder()
                .SetMazeAlgorithm(mazeGenAlgorithm)
                .RecordMazeTransformationSteps(true)
                .SetMazeDimensions(30, 30);
            mazeBuilder.Build();

            var steps = mazeBuilder.GetMazeTransformationSteps();
            Assert.True(steps.Count > 1);
            Assert.True(steps.Single(x => x.typeAfterTransform.Equals(MazeFieldType.Exit)) != null);
            Assert.True(steps.Single(x => x.typeAfterTransform.Equals(MazeFieldType.Entrance)) != null);
        }
    }
}