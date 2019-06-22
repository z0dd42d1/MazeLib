using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.MazeGenAlgos
{
    public static class KnownMazesTypes
    {
        public static IList<IMazeGenAlgorithm> GetAllMazeAlgos() => new List<IMazeGenAlgorithm>()
        {
            new DepthFirst (),
            new BinaryTree ()
        };
    }
}