namespace MazeLib.Base
{
    public interface IMaze
    {
        /// <summary>
        /// Returns the type of the maze field on a specified position.
        /// </summary>
        /// <param name="x">x-axis position</param>
        /// <param name="y">y-axis position</param>
        /// <returns>The type of the field</returns>
        MazeFieldType GetMazeTypeOnPos(int x, int y);

        /// <summary>
        /// Returns the type of the maze field on a specified position.
        /// </summary>
        /// <param name="coordinate">the coordinate requested</param>
        /// <returns>The type of the field</returns>
        MazeFieldType GetMazeTypeOnPos(MazeCoordinate coordinate);

        /// <summary>
        /// Sets the maze field type on the specified position.
        /// </summary>
        /// <param name="x">x-axis position</param>
        /// <param name="y">y-axis position</param>
        /// <param name="type">The type this field shall become</param>
        /// <returns>A tranformation object which represents the change to the maze that has been made with this call.</returns>
        MazeTransformationStep SetMazeTypeOnPos(int x, int y, MazeFieldType type);

        /// <summary>
        /// Sets the maze field type on the specified position.
        /// </summary>
        /// <param name="coordinate">the coordinate requested</param>
        /// <param name="type">The type this field shall become</param>
        /// <returns>A tranformation object which represents the change to the maze that has been made with this call.</returns>
        MazeTransformationStep SetMazeTypeOnPos(MazeCoordinate coordinate, MazeFieldType type);

        /// <summary>
        /// Changes the maze according to the passed transformation step.
        /// </summary>
        /// <param name="step"></param>
        void TransformMaze(MazeTransformationStep step);

        /// <summary>
        /// Gets the width of this maze.
        /// </summary>
        int GetWidth();

        /// <summary>
        /// Gets the height of this maze.
        /// </summary>
        int GetHeight();
    }
}