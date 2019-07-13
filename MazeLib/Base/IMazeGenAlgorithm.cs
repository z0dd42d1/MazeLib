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
        /// Returns the maze in its current state. Use this after generating a maze to get the endresult.
        /// </summary>
        /// <returns></returns>
        TileMapMaze GetMaze();

        /// <summary>
        /// Initialized the maze for the generation algorithm
        /// </summary>
        void InitializeMaze();

        /// <summary>
        /// Sets the maze object which will be transformed by the algorithm.
        /// </summary>
        void SetCurrentMaze(TileMapMaze maze);

        /// <summary>
        /// Returns the finished maze.
        /// </summary>
        IList<MazeTransformationStep> GenerateMazeWithTranformationList();

        void GenerateMaze();
    }
}