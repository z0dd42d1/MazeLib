# MazeLib - a maze generation NuGet library

This lib intends to provide easy methods to generate 2D mazes.
It will be further expanded in the future.

The libary and its interfaces are not stable at the moment (alpha version).


## How to use this libary

Create a maze object with the builder pattern (Parameters all pre definded):
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

Example Result:
![BinaryTree](https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/BinaryTree.png)

![DepthFirst](https://raw.githubusercontent.com/z0dd42d1/MazeLib/master/Documentation/DepthFirst.png)
