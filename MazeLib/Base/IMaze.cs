namespace MazeLib.Base
{
    public interface IMaze
    {
        MazeFieldType GetMazeTypeOnPos(int x, int y);

        MazeFieldType GetMazeTypeOnPos(MazeCoordinate coordinate);

        MazeTransformationStep SetMazeTypeOnPos(int x, int y, MazeFieldType type);

        MazeTransformationStep SetMazeTypeOnPos(MazeCoordinate coordinate, MazeFieldType type);

        void TransformMaze(MazeTransformationStep step);

        int GetWidth();

        int GetHeight();
    }
}