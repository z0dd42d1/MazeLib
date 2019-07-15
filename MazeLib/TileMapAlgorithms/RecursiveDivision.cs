using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeLib.TileMapAlgorithms
{
    internal class RecursiveDivision : MazeGenAlgorithmBase
    {
        private MazeCoordinate downright;
        private MazeCoordinate upperleft;

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override void InitializeMaze()
        {
            this.CurrentMaze.OverrideAllMazeFields();
            downright = new MazeCoordinate(this.CurrentMaze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, this.CurrentMaze.GetHeight() - 1);
            this.CurrentMaze.MakeRectangle(upperleft, downright);
        }

        internal override IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            foreach (var recursiveResult in RekursiveDiv(upperleft, downright))
            {
                yield return recursiveResult;
            }

            yield return TryPlaceEntrance();
            yield return TryPlaceExit();
        }

        private IEnumerable<MazeTransformationStep> RekursiveDiv(MazeCoordinate recUpperleft, MazeCoordinate recDownright)
        {
            int width = recDownright.X - recUpperleft.X;
            int height = recUpperleft.Y - recDownright.Y;

            int decide = 0;
            int maxdiff = 1;

            bool dontContinue = false;

            if (width >= 5 && height >= 5)
            {
                if (width - height > maxdiff)
                {
                    decide = 1;
                }
                else if (height - width > maxdiff)
                {
                    decide = 0;
                }
                else
                    decide = Random.Next(2);
            }
            else if (width < 5 && height < 5)
            {
                dontContinue = true; // Stop recursion
            }
            else if (width < 5 && height >= 5)
            {
                decide = 0;//horizontal teilen
            }
            else if (width >= 5 && height <= 5)
            {
                decide = 1;//senkrecht teilen
            }

            int newWallX, newWallY, triesToPlaceWall = 0;
            if (!dontContinue)
            {
                switch (decide)
                {// Horizontal oder Senkrecht
                    case 0:// Horizontal
                        int i;

                        do
                        { // Punkt aussuchen wo die Mauer hinkommt
                            if (triesToPlaceWall > 15)
                            {
                                dontContinue = true;
                            }

                            newWallY = recDownright.Y + 2 + Random.Next(height - 3);

                            triesToPlaceWall++;
                        } while (!dontContinue &&
                                  (CurrentMaze.IsCorridor(new MazeCoordinate(recUpperleft.X, newWallY))
                                || CurrentMaze.IsCorridor(new MazeCoordinate(recDownright.X, newWallY))));

                        for (i = 0; i < width; i++)
                        { // Neue Mauer ziehen
                            yield return CurrentMaze.SetMazeTypeOnPos(new MazeCoordinate(recUpperleft.X + i, newWallY), MazeFieldType.Wall);
                        }

                        // Durchgang erschaffen
                        int randX = (recUpperleft.X + 1 + Random.Next(width - 1));
                        yield return CurrentMaze.SetMazeTypeOnPos(new MazeCoordinate(randX, newWallY), MazeFieldType.Corridor);

                        // oben
                        MazeCoordinate upperleftNew = new MazeCoordinate(recUpperleft.X, recUpperleft.Y);
                        MazeCoordinate downrightNew = new MazeCoordinate(recDownright.X, newWallY);

                        foreach (var recursiveResult in RekursiveDiv(upperleftNew, downrightNew))
                        {
                            yield return recursiveResult;
                        }

                        // unten
                        upperleftNew = new MazeCoordinate(recUpperleft.X, newWallY);
                        downrightNew = new MazeCoordinate(recDownright.X, recDownright.Y);

                        foreach (var recursiveResult in RekursiveDiv(upperleftNew, downrightNew))
                        {
                            yield return recursiveResult;
                        }
                        break;

                    case 1:// Senkrecht

                        do
                        { // Punkt aussuchen wo die Mauer hinkommt
                            if (triesToPlaceWall > 15)
                            {
                                dontContinue = true;
                            }

                            newWallX = recUpperleft.X + 2 + Random.Next(width - 3);
                            triesToPlaceWall++;
                        } while (!dontContinue &&
                                  (CurrentMaze.IsCorridor(new MazeCoordinate(newWallX, recUpperleft.Y))
                                || CurrentMaze.IsCorridor(new MazeCoordinate(newWallX, recDownright.Y))));

                        for (i = 0; i < height; i++)
                        { // Neue Mauer ziehen
                            yield return CurrentMaze.SetMazeTypeOnPos(new MazeCoordinate(newWallX, recDownright.Y + i), MazeFieldType.Wall);
                        }

                        // Durchgang erschaffen

                        yield return CurrentMaze.SetMazeTypeOnPos(new MazeCoordinate(newWallX, recDownright.Y + 1 + Random.Next(height - 1)), MazeFieldType.Corridor);

                        //linkes Rechteck
                        upperleftNew = new MazeCoordinate(recUpperleft.X, recUpperleft.Y);
                        downrightNew = new MazeCoordinate(newWallX, recDownright.Y);

                        foreach (var recursiveResult in RekursiveDiv(upperleftNew, downrightNew))
                        {
                            yield return recursiveResult;
                        }

                        //rechtes Rechteck
                        upperleftNew = new MazeCoordinate(newWallX, recUpperleft.Y);
                        downrightNew = new MazeCoordinate(recDownright.X, recDownright.Y);

                        foreach (var recursiveResult in RekursiveDiv(upperleftNew, downrightNew))
                        {
                            yield return recursiveResult;
                        }// rechtes Rechteck
                        break;
                }
            }
        }
    }
}