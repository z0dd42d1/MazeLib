using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.MazeGenAlgos
{
    public class DepthFirst : MazeGenAlgorithmBase
    {
        private MazeCoordinate downright;
        private MazeCoordinate upperleft;
        private Stack<MazeCoordinate> pointStack = new Stack<MazeCoordinate>();
        private List<MazeCoordinate> possibleWays = new List<MazeCoordinate>();

        private Random random = new Random();

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override IList<MazeTransformationStep> GenerateMazeFullSize()
        {
            this.SetCurrentMaze(GetInitializedMaze());

            return InternalGenerateMazeFullSize().ToList();
        }

        private IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            var entrance = new MazeCoordinate(upperleft.x + 1 + random.Next(this.currentMaze.GetWidth() - 2), upperleft.y);

            pointStack.Push(entrance);

            Nullable<MazeCoordinate> newCorridor;

            while (pointStack.Count() != 0)
            {
                newCorridor = FindWay();

                if (newCorridor == null)
                {
                    newCorridor = pointStack.Pop(); // One step back
                    if (newCorridor != null)
                        yield return currentMaze.SetMazeTypeOnPos(newCorridor.Value, MazeFieldType.Corridor);
                }
                else
                {
                    pointStack.Push(newCorridor.Value);
                    yield return currentMaze.SetMazeTypeOnPos(newCorridor.Value, MazeFieldType.Corridor);
                }
            }

            yield return this.currentMaze.SetMazeTypeOnPos(entrance, MazeFieldType.Entrance);
        }

        private Nullable<MazeCoordinate> FindWay()
        {
            MazeCoordinate akt = pointStack.Peek();
            possibleWays.Clear();

            // four possible directions
            possibleWays.Add(new MazeCoordinate(akt.x, akt.y - 1));
            possibleWays.Add(new MazeCoordinate(akt.x + 1, akt.y));
            possibleWays.Add(new MazeCoordinate(akt.x - 1, akt.y));
            possibleWays.Add(new MazeCoordinate(akt.x, akt.y + 1));

            int i = random.Next(possibleWays.Count);

            while (possibleWays.Count != 0)
            {
                if (IsValidWay(possibleWays[i], akt))
                {
                    return possibleWays[i];
                }
                else
                {
                    possibleWays.Remove(possibleWays[i]);
                    if (possibleWays.Count != 0)
                        i = random.Next(possibleWays.Count);
                }
            }
            return null;
        }

        private bool IsValidWay(MazeCoordinate point, MazeCoordinate comeFrom)
        {
            // Out of the maze?
            if (point.x <= upperleft.x || point.x >= downright.x)
                return false;

            if (point.y >= upperleft.y || point.y <= downright.y)
                return false;

            //Allready somethin other than a wall?
            if (this.currentMaze.GetMazeTypeOnPos(point) != MazeFieldType.Wall)
                return false;

            // Make sure we do not create a way throu a wall.
            MazeCoordinate up = new MazeCoordinate(point.x, point.y - 1);
            MazeCoordinate right = new MazeCoordinate(point.x + 1, point.y);
            MazeCoordinate down = new MazeCoordinate(point.x, point.y + 1);
            MazeCoordinate left = new MazeCoordinate(point.x - 1, point.y);

            MazeCoordinate[] pointsToCheck = { up, right, down, left };

            for (int i = 0; i < pointsToCheck.Length; i++)
            {
                if (!pointsToCheck[i].Equals(comeFrom))
                {
                    if (currentMaze.GetMazeTypeOnPos(pointsToCheck[i]) == MazeFieldType.Corridor) return false;
                }
            }

            return true;
        }

        public override TileMapMaze GetInitializedMaze()
        {
            downright = new MazeCoordinate(currentMaze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, currentMaze.GetHeight() - 1);

            var maze = new TileMapMaze(currentMaze.GetWidth(), currentMaze.GetHeight());
            maze.OverrideAllMazeFields(MazeFieldType.Wall);
            return maze;
        }

        internal override void PlaceEntrance()
        {
            throw new NotImplementedException();
        }

        internal override void PlaceExit()
        {
            throw new NotImplementedException();
        }
    }
}