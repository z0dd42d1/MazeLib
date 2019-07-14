using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Base
{
    public abstract class MazeGenAlgorithmBase : IMazeGenAlgorithm
    {
        private const int MINIMUM_MAZE_SIZE = 5;

        protected Random random = new Random();
        protected TileMapMaze currentMaze;

        internal abstract IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize();

        public IList<MazeTransformationStep> GenerateMazeWithTranformationList()
        {
            ValidateMinimumMazeRequirements();

            InitializeMaze();

            return InternalGenerateMazeFullSize().ToList();
        }

        private void ValidateMinimumMazeRequirements()
        {
            if (this.currentMaze == null)
            {
                throw new Exception("Cannot create a maze when the maze object is null. Set a valid maze object or use the MazeBuilder class.");
            }
            if (this.currentMaze.GetWidth() < MINIMUM_MAZE_SIZE ||
                this.currentMaze.GetHeight() < MINIMUM_MAZE_SIZE)
            {
                throw new Exception($"Minimum dimensions for mazes are {MINIMUM_MAZE_SIZE} for both width and height. Can't create mazes with smaller values...");
            }
        }

        public void GenerateMaze()
        {
            InitializeMaze();

            InternalGenerateMazeFullSize().Last();
        }

        public virtual TileMapMaze GetMaze()
        {
            return currentMaze;
        }

        public virtual int GetMazeWidth()
        {
            return currentMaze.GetWidth();
        }

        public virtual int GetMazeHeight()
        {
            return currentMaze.GetHeight();
        }

        public void SetCurrentMaze(TileMapMaze maze)
        {
            this.currentMaze = maze;
        }

        public abstract void InitializeMaze();

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
            for (int x = currentMaze.GetWidth() - 2; x >= 1; x--)
            {
                possibleEntrances.Add(new MazeCoordinate(x, 0));
            }

            // check in random order if entrance is valid
            while (possibleEntrances.Count > 0)
            {
                var possibleEntr = possibleEntrances.ElementAt(random.Next(possibleEntrances.Count));

                if (currentMaze.GetMazeTypeOnPos(possibleEntr.x, 1) == MazeFieldType.Corridor)
                {
                    return currentMaze.SetMazeTypeOnPos(possibleEntr, MazeFieldType.Entrance);
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
            for (int x = currentMaze.GetWidth() - 2; x >= 1; x--)
            {
                possibleExits.Add(new MazeCoordinate(x, currentMaze.GetHeight() - 1));
            }

            // check in random order if exit is valid
            while (possibleExits.Count > 0)
            {
                var possibleExit = possibleExits.ElementAt(random.Next(possibleExits.Count));

                if (currentMaze.GetMazeTypeOnPos(possibleExit.x, possibleExit.y - 1) == MazeFieldType.Corridor)
                {
                    return currentMaze.SetMazeTypeOnPos(possibleExit, MazeFieldType.Exit);
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