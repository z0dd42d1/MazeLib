﻿using MazeLib.Base;
using MazeLib.TileMapAlgorithms;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeLib.Base
{
    public class MazeBuilder : IMazeBuilder
    {
        private MazeGenAlgorithmBase algo;
        private Action<MazeTransformationStep> drawCallback;
        private int width;
        private int height;
        private bool recordMazeTranformationSteps = false;
        private HashSet<MazeTransformationStep> mazeTransformationSteps;

        public HashSet<MazeTransformationStep> GetMazeTransformationSteps()
        {
            // TODO this is kinda shitty
            if (!recordMazeTranformationSteps)
                throw new Exception("MazeTransformation Steps not recorded, use the RecordMazeTransformationSteps() method on the builder.");

            return mazeTransformationSteps;
        }

        public MazeBuilder()
        {
            this.algo = new DepthFirst();
            this.width = 31;
            this.height = 41;
            this.algo.SetCurrentMaze(new TileMapMaze(width, height));
        }

        public MazeBuilder SetMazeAlgorithm(IMazeGenAlgorithm algo)
        {
            this.algo = algo as MazeGenAlgorithmBase;
            return this;
        }

        public MazeBuilder SetDrawCallback(Action<MazeTransformationStep> callback)
        {
            this.drawCallback = callback;
            return this;
        }

        public MazeBuilder RecordMazeTransformationSteps(bool recordMazeTranformationStep = true)
        {
            this.recordMazeTranformationSteps = recordMazeTranformationStep;
            return this;
        }

        public MazeBuilder SetMazeDimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        public IMaze Build()
        {
            var maze = new TileMapMaze(width, height);
            maze.SetDrawCallback(this.drawCallback);
            this.algo.SetCurrentMaze(maze);

            if (recordMazeTranformationSteps)
            {
                this.mazeTransformationSteps = new HashSet<MazeTransformationStep>(this.algo.GenerateMazeWithTranformationList());
            }
            else
            {
                this.algo.GenerateMaze();
            }

            return algo.GetMaze();
        }
    }
}