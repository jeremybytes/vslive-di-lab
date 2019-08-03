using System;
using System.Diagnostics;

namespace DrawMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new MazeGenerator();
            CreateAndShowMaze(generator);
            Console.ReadLine();
        }

        private static void CreateAndShowMaze(MazeGenerator generator)
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
