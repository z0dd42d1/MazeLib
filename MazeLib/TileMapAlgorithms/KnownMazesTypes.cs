using MazeLib.Base;
using MazeLib.TileMapAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.TileMapAlgorithms
{
    public static class KnownMazesTypes
    {
        public static IList<IMazeGenAlgorithm> GetAllMazeAlgos() => new List<IMazeGenAlgorithm>()
        {
            new DepthFirst (),
            new BinaryTree (),
            new RandomizedPrims(),
        };

        public static IList<Type> GetAllMazeAlgosAsType() => GetAllMazeAlgos().Select(x => x.GetType()).ToList();
    }
}