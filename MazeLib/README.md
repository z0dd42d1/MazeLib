# MazeLib - a maze generation NuGet library

This lib intends to provide easy methods to generate 2D mazes.
It will be further expanded in the future.

**The library and its interfaces are not stable at the moment (alpha version).**


## How to use this libary

Create a maze object with the builder pattern (Parameters all predefined):
```
    MazeBuilder mazeBuilder = new MazeBuilder();
    var maze = mazeBuilder
        .Build();
```

Create a maze object with the builder pattern (Customized parameters):
```
    MazeBuilder mazeBuilder = new MazeBuilder();
    var maze = mazeBuilder
        .SetMazeAlgorithm(new DepthFirst())
        .SetMazeDimensions(200,200)
        .SetDrawCallback(t => DrawTile(t.coordinate.x,t.coordinate.y,t.typeAfterTransform))
        .Build();
```



Save it as a image:
```
MazeImageCreator.CreateMazeImage(maze, "filename", 10, "./");
```


| RandomizedPrims: | <img align="left"  width="305" height="305" src="https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/RandomizedPrims.gif" title="RandomizedPrim">| BinaryTree:   |<img align="left"  width="305" height="305" src="https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/BinaryTree.gif" title="BinaryTree">
| --------------- |-------------|------------|-----------|
| DepthFirst: | <img align="left"  width="305" height="305" src="https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/DepthFirst.gif" title="DepthFirst">|   |




<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/>
Red Tile mark the entrance, green tiles the exit.

Algorithms implemented as described on https://en.wikipedia.org/wiki/Maze_generation_algorithm
The implementation does not aim to be precise, but to reliable generate characteristic maze types.