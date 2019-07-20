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
        .SetDrawCallback(t => DrawTile(t.coordinate.X,t.coordinate.Y,t.typeAfterTransform))
        .Build();
```



Save it as a image:
```
MazeImageCreator.CreateMazeImage(maze, "filename", 10, "./");
```


<p>RandomizedPrims:</p>
<img src="https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/RandomizedPrims.gif"> 
<p>BinaryTree: </p> 
<img src="https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/BinaryTree.gif"> 
<p>DepthFirst: </p>
<img src="https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/DepthFirst.gif">
<p>RecursiveDivision: </p>
<img src="https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/RecursiveDivision.gif">



Red Tile mark the entrance, green tiles the exit.

Algorithms implemented as described on https://en.wikipedia.org/wiki/Maze_generation_algorithm
The implementation does not aim to be precise, but to reliable generate characteristic maze types.