using System.Drawing;

namespace DrawMaze
{
    public interface IMazeGenerator
    {
        void GenerateMaze();
        Bitmap GetGraphicalMaze(bool includeHeatMap = false);
        string GetTextMaze(bool includePath = false);
    }
}