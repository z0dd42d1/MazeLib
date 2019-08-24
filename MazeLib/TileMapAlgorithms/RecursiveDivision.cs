using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeLib.TileMapAlgorithms
{
    public class RecursiveDivision : MazeGenAlgorithmBase
    {
        private MazeCoordinate downright;
        private MazeCoordinate upperleft;

        private enum Strategy
        {
            DividedBigSpacesInTheMiddle,
            CreateCompactRandomLabyrinth
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override IEnumerable<MazeTransformationStep> InitializeMaze()
        {
            this.CurrentMaze.OverrideAllMazeFields();
            downright = new MazeCoordinate(this.CurrentMaze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, this.CurrentMaze.GetHeight() - 1);

            foreach (var step in this.CurrentMaze.MakeRectangle(upperleft, downright))
            {
                yield return step;
            }
        }

        internal override IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            foreach (var step in InitializeMaze())
            {
                yield return step;
            }

            foreach (var recursiveResult in RekursiveDiv(upperleft, downright, Random.Next(2) > 0))
            {
                yield return recursiveResult;
            }

            yield return TryPlaceEntrance();
            yield return TryPlaceExit();
        }

        private IEnumerable<MazeTransformationStep> RekursiveDiv(MazeCoordinate recUpperleft, MazeCoordinate recDownright, bool horizontal)
        {
            int width = recDownright.X - recUpperleft.X;
            int height = recUpperleft.Y - recDownright.Y;
            int minSize = 0;

            Strategy currentStrategy = CheckWhichStrategyToUse(width, height);

            if (!(width < minSize && height < minSize))
            {
                // Override the random decision for horizontal/vertical wall placement, this reduces long stretched corridors

                if (width > height)
                {
                    horizontal = false;
                }
                else
                {
                    horizontal = true;
                }

                if (horizontal)
                {// Horizontal
                    int newWallY = TryPlaceWallHorizontal(recUpperleft, recDownright, currentStrategy);
                    if (newWallY > 0)
                    {
                        for (int i = 1; i < width; i++)
                        { // Create wall
                            yield return CurrentMaze.SetMazeTypeOnPos(new MazeCoordinate(recUpperleft.X + i, newWallY), MazeFieldType.Wall);
                        }

                        // Place corridor
                        int randX = FindHorizontalCorridor(ref recUpperleft, ref recDownright);
                        yield return CurrentMaze.SetMazeTypeOnPos(new MazeCoordinate(randX, newWallY), MazeFieldType.Corridor);

                        // up
                        MazeCoordinate upperleftNew = new MazeCoordinate(recUpperleft.X, recUpperleft.Y);
                        MazeCoordinate downrightNew = new MazeCoordinate(recDownright.X, newWallY);

                        foreach (var recursiveResult in RekursiveDiv(upperleftNew, downrightNew, Random.Next(2) > 0))
                        {
                            yield return recursiveResult;
                        }

                        // down
                        upperleftNew = new MazeCoordinate(recUpperleft.X, newWallY);
                        downrightNew = new MazeCoordinate(recDownright.X, recDownright.Y);

                        foreach (var recursiveResult in RekursiveDiv(upperleftNew, downrightNew, Random.Next(2) > 0))
                        {
                            yield return recursiveResult;
                        }
                    }
                }
                else
                {// Vertical
                    int newWallX = TryPlaceWallVertical(recUpperleft, recDownright, currentStrategy);
                    if (newWallX > 0)
                    {
                        for (int i = 1; i < height; i++)
                        { // Create Wall
                            yield return CurrentMaze.SetMazeTypeOnPos(new MazeCoordinate(newWallX, recDownright.Y + i), MazeFieldType.Wall);
                        }

                        // Place corridor
                        int randY = FindVerticalCorridor(ref recUpperleft, ref recDownright);
                        yield return CurrentMaze.SetMazeTypeOnPos(new MazeCoordinate(newWallX, randY), MazeFieldType.Corridor);

                        //left
                        MazeCoordinate upperleftNew = new MazeCoordinate(recUpperleft.X, recUpperleft.Y);
                        MazeCoordinate downrightNew = new MazeCoordinate(newWallX, recDownright.Y);

                        foreach (var recursiveResult in RekursiveDiv(upperleftNew, downrightNew, Random.Next(2) > 0))
                        {
                            yield return recursiveResult;
                        }

                        //right
                        upperleftNew = new MazeCoordinate(newWallX, recUpperleft.Y);
                        downrightNew = new MazeCoordinate(recDownright.X, recDownright.Y);

                        foreach (var recursiveResult in RekursiveDiv(upperleftNew, downrightNew, Random.Next(2) > 0))
                        {
                            yield return recursiveResult;
                        }
                    }
                }
            }
        }

        private Strategy CheckWhichStrategyToUse(int width, int height)
        {
            int limitForStrategyChange = 8;
            if ((limitForStrategyChange >= height) ||
                (limitForStrategyChange >= width))
            {
                return Strategy.CreateCompactRandomLabyrinth;
            }
            else
            {
                return Strategy.DividedBigSpacesInTheMiddle;
            }
        }

        private int FindVerticalCorridor(ref MazeCoordinate recUpperleft, ref MazeCoordinate recDownright)
        {
            int type = Random.Next(2);

            switch (type)
            {
                case 1: // top
                    return recUpperleft.Y - 1;

                default: // bottom
                    return recDownright.Y + 1;
            }
        }

        private int FindHorizontalCorridor(ref MazeCoordinate recUpperleft, ref MazeCoordinate recDownright)
        {
            int type = Random.Next(2);

            switch (type)
            {
                case 1: // right
                    return recDownright.X - 1;

                default: // left
                    return recUpperleft.X + 1;
            }
        }

        /// <summary>
        /// Tries to find a place for the wall in the specified rectangle.
        /// </summary>
        /// <returns>An integer which describes the y coordinate where the wall should be placed</returns>
        private int TryPlaceWallHorizontal(MazeCoordinate recUpperleft, MazeCoordinate recDownright, Strategy strategy)
        {
            // first collect all possible coordinates
            int from = recDownright.Y + 2;
            int to = recUpperleft.Y - 2;
            int count = to - from;
            if (count <= 0) return -1;

            var possibleHorizontalCoordinates = Enumerable.Range(from, to - from).ToList();

            while (possibleHorizontalCoordinates.Count() > 0)
            {
                //try them one by one
                int coordinateToTest = ChooseCoordinateBasedOnStrategy(strategy, possibleHorizontalCoordinates);

                // will it cover a corridor?
                if (!(CurrentMaze.IsCorridor(new MazeCoordinate(recUpperleft.X, coordinateToTest))
                  || CurrentMaze.IsCorridor(new MazeCoordinate(recDownright.X, coordinateToTest))))
                {
                    // found valid coordinate
                    return coordinateToTest;
                }
                possibleHorizontalCoordinates.Remove(coordinateToTest);
            }

            // Failed to find a good coordinate.
            return -1;
        }

        private int ChooseCoordinateBasedOnStrategy(Strategy strategy, List<int> possibleHorizontalCoordinates)
        {
            int coordinateToTest = 0;
            if (strategy == Strategy.DividedBigSpacesInTheMiddle)
                coordinateToTest = possibleHorizontalCoordinates.ElementAt(possibleHorizontalCoordinates.Count / 2);
            else if (strategy == Strategy.CreateCompactRandomLabyrinth)
                if (Random.Next(2) > 0)
                {
                    // left
                    coordinateToTest = possibleHorizontalCoordinates.First();
                }
                else
                {
                    // right
                    coordinateToTest = possibleHorizontalCoordinates.Last();
                }
            return coordinateToTest;
        }

        /// <summary>
        /// Tries to find a place for the wall in the specified rectangle.
        /// </summary>
        /// <returns>An integer which describes the x coordinate where the wall should be placed</returns>
        private int TryPlaceWallVertical(MazeCoordinate recUpperleft, MazeCoordinate recDownright, Strategy strategy)
        {
            // first collect all possible coordinates
            int from = recUpperleft.X + 2;
            int to = recDownright.X - 2;
            int count = to - from;
            if (count <= 0) return -1;

            var possibleVerticalCoordinates = Enumerable.Range(from, to - from).ToList();

            while (possibleVerticalCoordinates.Count() > 0)
            {
                //try them one by one
                int coordinateToTest = ChooseCoordinateBasedOnStrategy(strategy, possibleVerticalCoordinates);

                // will it cover a corridor?
                if (!(CurrentMaze.IsCorridor(new MazeCoordinate(coordinateToTest, recUpperleft.Y))
                   || CurrentMaze.IsCorridor(new MazeCoordinate(coordinateToTest, recDownright.Y))))
                {
                    // found valid coordinate
                    return coordinateToTest;
                }
                possibleVerticalCoordinates.Remove(coordinateToTest);
            }

            // Failed to find a good coordinate.
            return -1;
        }
    }
}