using System;

namespace MazeLib.Base
{
    public interface IMazeBuilder
    {
        /// <summary>
        /// Creates the Maze.
        /// </summary>
        /// <returns>The finished maze object.</returns>
        IMaze Build();

        /// <summary>
        /// Use this method to set a draw callback function if you want to animate the maze creation process.
        /// </summary>
        /// <param name="callback">A action which gets the current transformation step on the maze passed as a parameter. </param>
        MazeBuilder SetDrawCallback(Action<MazeTransformationStep> callback);

        /// <summary>
        /// Use this method to set the Maze Algorithm which will be used to create the maze.
        /// </summary>
        /// <param name="algo">The used maze algorithm</param>
        MazeBuilder SetMazeAlgorithm(IMazeGenAlgorithm algo);

        /// <summary>
        /// Use this method to set the size of the generated maze.
        /// </summary>
        /// <param name="width">The maze width</param>
        /// <param name="height">The maze height</param>
        MazeBuilder SetMazeDimensions(int width, int height);
    }
}