using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Base
{
    public abstract class MazeGenAlgorithmBase : IMazeGenAlgorithm
    {
        protected TileMapMaze currentMaze;
        private Action drawCallback;

        public abstract IList<MazeTransformationStep> GenerateMazeFullSize();

        public virtual TileMapMaze GetCurrentMaze()
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

        public void SetDrawCallback(Action action)
        {
            this.drawCallback = action;
        }

        public abstract TileMapMaze GetInitializedMaze();

        public abstract string GetName();

        internal abstract MazeTransformationStep PlaceEntrance();

        internal abstract MazeTransformationStep PlaceExit();
    }
}