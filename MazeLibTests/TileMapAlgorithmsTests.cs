﻿using MazeLib.Base;
using MazeLib.TileMapAlgorithms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MazeLibTests
{
    // Generate a neutral object array for each algorithm and return it as an enumerable to use it in ClassData + Theory Attribute.
    public class TestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = GetArrayOfMazeAlgos();

        private static List<object[]> GetArrayOfMazeAlgos()
        {
            var list = new List<object[]>();

            var mazes = KnownMazesTypes.GetAllMazeAlgos().ToArray<object>();

            foreach (IMazeGenAlgorithm algorithm in mazes)
            {
                list.Add(new object[] { algorithm });
            }
            return list;
        }

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class TileMapAlgorithmsTests
    {
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void GenerateFullSizeMaze_GeneratesMaze_10x10(IMazeGenAlgorithm mazeGenAlgorithm)
        {
            var mazeBuilder = new MazeBuilder()
                .SetMazeAlgorithm(mazeGenAlgorithm)
                .SetMazeDimensions(10, 10);

            Assert.NotNull(mazeBuilder.Build());
            //TODO Test maze is solvable
        }

        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void GenerateFullSizeMaze_GeneratesMaze_100x100(IMazeGenAlgorithm mazeGenAlgorithm)
        {
            var mazeBuilder = new MazeBuilder()
                .SetMazeAlgorithm(mazeGenAlgorithm)
                .SetMazeDimensions(100, 100);

            Assert.NotNull(mazeBuilder.Build());
            //TODO Test maze is solvable
        }
    }
}