Lab 03 - Injection Patterns & Cross-Cutting Concerns
====================================================

Objectives
-----------
This lab incorporates useful design patterns (such as the Decorator pattern) as well as various dependency injection patterns (including constructor injection and property injection).

Application Overview
--------------------

The "Starter" folder contains the code files for this lab. Open the "Mazes" solution.

This is a console application that produces mazes. There are a number of algorithms that generate mazes with different speeds and biases (a bias could be a tendency to include diagonal paths, longer/shorter paths, or twistier/straighter paths). 

The "DrawMaze" project contains the console application and "MazeGenerator" that we'll be working with.

The "Algorithms" project contains a number of different maze algorithms.

The "MazeGrid" project contains the infrastructure for the grid, cells, text outputs, and graphical outputs. This code was originally taken from "Mazes for Programmers" and translated from Ruby to C#.

Build the solution and run the application. A console window will pop up showing a 15 x 15 maze with a path traced from the center to the lower right corner. In addition, a bitmap will be generated and automatically opened. This shows a "heat map" with the darker areas being furthest from the center of the maze.

Lab Goals
---------
The "MazeGenerator" class creates the grid and "new"s up the maze algorithm that it wants to use. We want to move this responsibility for creating the grid and selecting a maze algorithm outside of the class.

In addition, we want to add logging to the "MazeGenerator". Instead of adding a logging object directly to the class, we will add logging as a cross-cutting concern.

Current Classes
---------------

Program.cs:
```c#
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new MazeGenerator();
            CreateAndShowMaze(generator);
            Console.ReadLine();
        }
        ...
    }
```

MazeGenerator.cs:
```c#
    public class MazeGenerator
    {
        private Grid mazeGrid;
        private bool isGenerated = false;

        public MazeGenerator()
        {
            mazeGrid = new ColorGrid(15, 15);
        }

        private void GenerateMaze()
        {
            var algorithm = new BinaryTree();
            algorithm?.CreateMaze(mazeGrid);
            isGenerated = true;
        }
        ...
    }
```

Hints
-----
In MazeGenerator.cs:
* Remove the "new ColorGrid()" call from the constructor by injecting a grid instead.

* Remove the "new" call from the "GenerateMaze" method by injecting the algorithm object into the class. Note: there is already an "IMazeAlgorithm" interface.

In Program.cs:
* Compose the objects in the "Main" method.

For Logging:
* Create a new Logging Decorator class to wrap the MazeGenerator. This should log "Enter Method" and "Exit Method" messages for "GetTextMaze" and "GetGraphicalMaze".

* Update the "Main" method to compose the objects using the decorator.

BONUS CHALLENGE: Add the Ninject DI container to compose the objects.

If you need more assistance, step-by-step instructions are included below. Otherwise, **STOP READING NOW**


Grid & Algorithms: Step-By-Step
-------------------------------
1. Add readonly fields for the Grid and IMazeAlgorithm.

In the "MazeGenerator.cs" file, add the following fields to the "MazeGenerator" class.

```c#
    private readonly Grid mazeGrid;
    private readonly IMazeAlgorithm algorithm;
    private bool isGenerated = false;
```

*Note: these fields are set to "readonly" so that they can only be set in the constructor.*

2. Update the constructor to accept these as parameters and set the private fields.

```c#
    public MazeGenerator(Grid grid, IMazeAlgorithm algorithm)
    {
        this.mazeGrid = grid;
        this.algorithm = algorithm;
    }
```

3. Update the "GenerateMaze" method to remove the local algorithm variable. Instead, it will use the private field.

```c#
    public void GenerateMaze()
    {
        algorithm?.CreateMaze(mazeGrid);
        isGenerated = true;
    }
```

*Note: the "algorithm" has a null check (the "?." operator). If the field is null, then the "GenerateMaze" method will do nothing, and the grid will remain in its default state.*

4. Compose the objects in the "Main" method.

In the "Program.cs" file, update "var generator = new MazeGenerator()" with the new parameters.

```c#
    static void Main(string[] args)
    {
        MazeGenerator generator = 
            new MazeGenerator(
                new ColorGrid(15, 15),
                new BinaryTree());

        CreateAndShowMaze(generator);
        Console.ReadLine();
    }
```

5. Build and run the application.

Try updating the parameters of the ColorGrid constructor to generate mazes of different sizes. Note: large mazes take a while for the bitmap version to generate.

Try changing the "BinaryTree" algorithm for one of the other maze algorithms.

Logging: Step-By-Step
----------------------
1. Extract an interface from the "MazeGenerator" class.

In the "MazeGenerator.cs" file, right-click on the "MazeGenerator" class, select "Quick Actions and Refactorings", and then "Extract interface".

Name the interface "IMazeGenerator" and make sure that it includes all 3 methods: "GenerateMaze", "GetTextMaze", and "GetGraphicalMaze".

2. Create a new logging decorator class that implements the new interface.

```c#
    public class ConsoleLoggingDecorator : IMazeGenerator
```

3. Add the interface implementation code by right-clicking on "IMazeGenerator" and selecting "Quick Actions and Refactorings", then "Implement interface".

```c#
    public void GenerateMaze()
    {
        throw new NotImplementedException();
    }

    public Bitmap GetGraphicalMaze(bool includeHeatMap = false)
    {    
        throw new NotImplementedException();
    }
    
    public string GetTextMaze(bool includePath = false)
    {    
        throw new NotImplementedException();
    }
```

4. Add a field and constructor parameter to wrap a real MazeGenerator.

```c#
    private readonly IMazeGenerator wrappedGenerator;

    public ConsoleLoggingDecorator(IMazeGenerator mazeGenerator)
    {
        wrappedGenerator = mazeGenerator;
    }
```

5. Add the code to log to the console. Here is one way of implementing that functionality:

```c#
    private void LogToConsole(string message)
    {
        Console.WriteLine($"{DateTime.Now:s}: {message}");
    }

    private void LogEnterMethod([CallerMemberName] string methodName = null)
    {
        LogToConsole($"Entering '{methodName}'");
    }

    private void LogExitMethod([CallerMemberName] string methodName = null)
    {
        LogToConsole($"Exiting '{methodName}'");
    }
```

6. Update the interface members to call the logging methods and the "real" methods from the wrapped repository.

```c#
    public void GenerateMaze()
    {
        LogEnterMethod();
        wrappedGenerator.GenerateMaze();
        LogExitMethod();
    }

    public Bitmap GetGraphicalMaze(bool includeHeatMap = false)
    {
        LogEnterMethod();
        var result = wrappedGenerator.GetGraphicalMaze(includeHeatMap);
        LogExitMethod();
        return result;
    }

    public string GetTextMaze(bool includePath = false)
    {
        LogEnterMethod();
        var result = wrappedGenerator.GetTextMaze(includePath);
        LogExitMethod();
        return result;
    }
```

7. In the "Program.cs" file, update the composition root with the logging decorator.

```c#
    static void Main(string[] args)
    {
        IMazeGenerator generator = 
            new ConsoleLoggingDecorator(
                new MazeGenerator(
                    new ColorGrid(15, 15),
                    new Sidewinder()));

        CreateAndShowMaze(generator);
        Console.ReadLine();
    }
```

Note that the type of the "generator" was changed to the interface "IMazeGenerator". We also need to change the signature of the "CreateAndShowMaze" method to use the interface.

```c#
   private static void CreateAndShowMaze(IMazeGenerator generator)
```

8. Build and run the application. The console should contain messages such as the following:

```
    2018-09-10T19:25:58: Entering 'GenerateMaze'
    2018-09-10T19:25:58: Exiting 'GenerateMaze'
    2018-09-10T19:25:58: Entering 'GetTextMaze'
    2018-09-10T19:25:58: Exiting 'GetTextMaze'
    ... [Text maze output here]
    2018-09-10T19:25:58: Entering 'GetGraphicalMaze'
    2018-09-10T19:25:58: Exiting 'GetGraphicalMaze'
```

BONUS CHALLENGE - Ninject: Step-By-Step
---------------------------------------
1. Add the Ninject NuGet package to the "DrawMaze" project;

Right-click on the "References" and choose "Manage NuGet Packages...". Use the search box to locate and install "Ninject".

2. Add the assembly references to the top of the "Program.cs" file.

```c#
    using Ninject;
```

3. Instantiate and configure the container.

```c#
    static void Main(string[] args)
    {
        // Ninject DI Container
        IKernel Container = new StandardKernel();
        Container.Bind<Grid>().ToMethod(c => new ColorGrid(15, 15));
        Container.Bind<IMazeAlgorithm>().To<Sidewinder>();
        Container.Bind<IMazeGenerator>().To<ConsoleLoggingDecorator>()
            .WithConstructorArgument<IMazeGenerator>(Container.Get<MazeGenerator>());
```

This contains the following:
* Instantiate the Ninject DI container.
* Map the "Grid" abstract class to a factory method (in this case, a constructor with parameters).
* Map the "IMazeAlgorithm" to a concrete implementation.
* Map the "IMazeGenerator" to the logging decorator. Since the logging decorator constructor needs an "IMazeGenerator" to wrap, we include "WithConstructorArgument" to inject the real maze generator and to prevent circular references.

4. Ask the container for an "IMazeGenerator".

```c#
    IMazeGenerator generator = Container.Get<IMazeGenerator>();
```

5. Build and run the application.

The results should be the same as when we wired up the objects manually. Change the "IMazeAlgorithm" mapping to try out different implementations.

***
*End of Lab 03 - Injection Patterns & Cross-Cutting Concerns*
***
