using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Base
{
    public interface IMazeGenAlgorithm
    {
        /// <summary>
        /// Returns the name of the Algorithm as a string.
        /// </summary>
        string GetName();

        /// <summary>
        /// Returns the maze in its current state. Use ist after generating a maze to get the endresult.
        /// </summary>
        /// <returns></returns>
        TileMapMaze GetCurrentMaze();

        /// <summary>
        /// Returns a maze object that represents the base for the algoritm to start the generating
        /// This method is used to skip init step of the maze.
        /// </summary>
        TileMapMaze GetInitializedMaze();

        /// <summary>
        /// Sets the maze object which will be transformed by the algorithm.
        /// </summary>
        void SetCurrentMaze(TileMapMaze maze);

        /// <summary>
        /// Returns the finished maze. Can be awaited.
        /// </summary>
        IList<MazeTransformationStep> GenerateMazeFullSize();
    }
}