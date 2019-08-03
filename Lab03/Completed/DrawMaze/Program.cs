using Algorithms;
using MazeGrid;
using System;
using System.Diagnostics;
using Ninject;

namespace DrawMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            // OPTION #1: Manual Composition
            //IMazeGenerator generator = 
            //    new ConsoleLoggingDecorator(
            //        new MazeGenerator(
            //            new ColorGrid(15, 15),
            //            new BinaryTree()));


            // OPTION #2: Ninject
            IKernel Container = new StandardKernel();
            Container.Bind<Grid>().ToMethod(c => new ColorGrid(15, 15));
            Container.Bind<IMazeAlgorithm>().To<Sidewinder>();
            Container.Bind<IMazeGenerator>().To<ConsoleLoggingDecorator>()
                .WithConstructorArgument<IMazeGenerator>(Container.Get<MazeGenerator>());
            IMazeGenerator generator = Container.Get<IMazeGenerator>();


            CreateAndShowMaze(generator);
            Console.ReadLine();
        }

        private static void CreateAndShowMaze(IMazeGenerator generator)
        {
            generator.GenerateMaze();

            var textMaze = generator.GetTextMaze(true);
            Console.WriteLine(textMaze);

            var graphicMaze = generator.GetGraphicalMaze(true);
            graphicMaze.Save("maze.png");
            Process p = new Process();
            p.StartInfo.FileName = "maze.png";
            p.Start();
        }
    }
}
