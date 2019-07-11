using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MazeLibTests
{
    public class MazeCoordinateTests
    {
        [Fact]
        public void MazeCoordinate_EqualsReturnsTrue_WhenComparedWithEqualObject()
        {
            var foo = new MazeCoordinate(1337, 42);
            var bar = new MazeCoordinate(1337, 42);
            Assert.True(foo.Equals(bar));
            Assert.True(bar.Equals(foo));
        }

        [Fact]
        public void MazeCoordinate_EqualsReturnsFalse_WhenComparedWithDiffObject()
        {
            var foo = new MazeCoordinate(1337, 42);
            var bar = new MazeCoordinate(1336, 42);
            Assert.False(foo.Equals(bar));
            Assert.False(bar.Equals(foo));
        }

        [Fact]
        public void MazeCoordinate_ListContainsWorks_WhenLookingForIdenticalObj()
        {
            var foo = new MazeCoordinate(1337, 42);
            var bar = new MazeCoordinate(1337, 42);

            var list = new List<MazeCoordinate>();
            list.Add(foo);

            Assert.Contains(bar, list);
        }

        [Fact]
        public void MazeCoordinate_ListContainsWorks_WhenLookingForDiffObj()
        {
            var foo = new MazeCoordinate(1337, 42);
            var bar = new MazeCoordinate(1337, 41);

            var list = new List<MazeCoordinate>();
            list.Add(foo);

            Assert.DoesNotContain(bar, list);
        }

        [Fact]
        public void MazeCoordinate_IsAdjacentReturnsTrue_ifDirectlyAdjacent()
        {
            var foo = new MazeCoordinate(1337, 42);
            var bar = new MazeCoordinate(1337, 41);

            Assert.True(foo.IsAdjacentTo(bar));
            Assert.True(bar.IsAdjacentTo(foo));
        }

        [Fact]
        public void MazeCoordinate_IsAdjacentReturnsFalse_ifTheSameCoordinate()
        {
            var foo = new MazeCoordinate(1337, 42);
            var bar = new MazeCoordinate(1337, 42);

            Assert.False(foo.IsAdjacentTo(bar));
            Assert.False(bar.IsAdjacentTo(foo));
        }

        [Fact]
        public void MazeCoordinate_IsAdjacentReturnsFalse_ifNotAdjacentVerticalOrHorizontal()
        {
            var foo = new MazeCoordinate(1336, 41);
            var bar = new MazeCoordinate(1337, 42);

            Assert.False(foo.IsAdjacentTo(bar));
            Assert.False(bar.IsAdjacentTo(foo));
        }
    }
}