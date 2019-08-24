using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Base
{
    public abstract class MazeGenAlgorithmBase : IMazeGenAlgorithm
    {
        private Random random = new Random();
        private TileMapMaze currentMaze;

        protected Random Random { get => random; set => random = value; }
        protected TileMapMaze CurrentMaze { get => currentMaze; set => currentMaze = value; }

        internal abstract IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize();

        public IList<MazeTransformationStep> GenerateMazeWithTranformationList()
        {
            ValidateMinimumMazeRequirements();

            return InternalGenerateMazeFullSize().ToList();
        }

        private void ValidateMinimumMazeRequirements()
        {
            if (this.CurrentMaze == null)
            {
                throw new Exception("Cannot create a maze when the maze object is null. Set a valid maze object or use the MazeBuilder class.");
            }
            if (this.CurrentMaze.GetWidth() < Constants.MINIMUM_MAZE_SIZE ||
                this.CurrentMaze.GetHeight() < Constants.MINIMUM_MAZE_SIZE)
            {
                throw new Exception($"Minimum dimensions for mazes are {Constants.MINIMUM_MAZE_SIZE} for both width and height. Can't create mazes with smaller values...");
            }
        }

        public void GenerateMaze()
        {
            ValidateMinimumMazeRequirements();

            InternalGenerateMazeFullSize().Last();
        }

        public virtual TileMapMaze GetMaze()
        {
            return CurrentMaze;
        }

        public virtual int GetMazeWidth()
        {
            return CurrentMaze.GetWidth();
        }

        public virtual int GetMazeHeight()
        {
            return CurrentMaze.GetHeight();
        }

        public void SetCurrentMaze(TileMapMaze maze)
        {
            this.CurrentMaze = maze;
        }

        public abstract IEnumerable<MazeTransformationStep> InitializeMaze();

        public abstract string GetName();

        public override string ToString()
        {
            return GetName();
        }

        /// <summary>
        /// Places a random and reachable entrance on the bottom of the maze.
        /// Throws an exception if no entrance can be placed.
        ///
        /// This method can be overwritten if the algorithm does not need it, or there are special cases.
        /// </summary>
        /// <returns>The transformation step for the entrance placement</returns>
        internal virtual MazeTransformationStep TryPlaceEntrance()
        {
            List<MazeCoordinate> possibleEntrances = new List<MazeCoordinate>();

            // collect all possible entrances
            for (int x = CurrentMaze.GetWidth() - 2; x >= 1; x--)
            {
                possibleEntrances.Add(new MazeCoordinate(x, 0));
            }

            // check in random order if entrance is valid
            while (possibleEntrances.Count > 0)
            {
                var possibleEntr = possibleEntrances.ElementAt(Random.Next(possibleEntrances.Count));

                if (CurrentMaze.GetMazeTypeOnPos(possibleEntr.X, 1) == MazeFieldType.Corridor)
                {
                    return CurrentMaze.SetMazeTypeOnPos(possibleEntr, MazeFieldType.Entrance);
                }
                else
                {
                    possibleEntrances.Remove(possibleEntr);
                }
            }

            throw new Exception("Could not find a suitable entrance position.");
        }

        /// <summary>
        /// Places a  random and reachable exit on the top of the maze.
        /// Throws an exception if no exit can be placed.
        ///
        /// This method can be overwritten if the algorithm does not need it, or there are special cases.
        /// </summary>
        /// <returns>The transformation step for the exit placement</returns>
        internal virtual MazeTransformationStep TryPlaceExit()
        {
            List<MazeCoordinate> possibleExits = new List<MazeCoordinate>();

            // collect all possible exits
            for (int x = CurrentMaze.GetWidth() - 2; x >= 1; x--)
            {
                possibleExits.Add(new MazeCoordinate(x, CurrentMaze.GetHeight() - 1));
            }

            // check in random order if exit is valid
            while (possibleExits.Count > 0)
            {
                var possibleExit = possibleExits.ElementAt(Random.Next(possibleExits.Count));

                if (CurrentMaze.GetMazeTypeOnPos(possibleExit.X, possibleExit.Y - 1) == MazeFieldType.Corridor)
                {
                    return CurrentMaze.SetMazeTypeOnPos(possibleExit, MazeFieldType.Exit);
                }
                else
                {
                    possibleExits.Remove(possibleExit);
                }
            }

            throw new Exception("Could not find a suitable exit position.");
        }
    }
}