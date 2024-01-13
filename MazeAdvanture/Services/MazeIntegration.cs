using System.Reflection.Metadata.Ecma335;
using AtlasCopco.Integration.Maze;
using MazeAdvanture.Models;
using MazeAdvanture.Settings;
namespace MazeAdvanture.Services
{
    public class MazeIntegration : IMazeIntegration
    {
        private readonly MazeSettings _mazeSettings;
        private int _size;
        private Maze? _maze;
        public MazeIntegration(MazeSettings mazeSettings)
        {
            _mazeSettings = mazeSettings;
        }

        public void BuildMaze(int size)
        {
            _size = size;
            _maze = MazeGenerator.BuildMaze(size, _mazeSettings.RoomTypes);
        }
        public int GetEntranceRoom()
        {
            ArgumentNullException.ThrowIfNull(_maze, "Maze has not been built yet.");
            return _maze.EntranceRoom.Id;
        }
        public int? GetRoom(int roomId, char direction)
        {
            ArgumentNullException.ThrowIfNull(_maze, "Maze has not been built yet.");
            return _maze.GetRoom(roomId, direction);
        }

        public bool CausesInjury(int roomId)
        {
            ArgumentNullException.ThrowIfNull(_maze, "Maze has not been built yet.");
            return _maze.FindRoom(roomId)?.CausesInjury() ?? false;
        }

        public string GetDescription(int roomId)
        {
            ArgumentNullException.ThrowIfNull(_maze, "Maze has not been built yet.");
            return _maze.FindRoom(roomId)?.GetDescription() ?? "";
        }
        public bool HasTreasure(int roomId)
        {
            ArgumentNullException.ThrowIfNull(_maze, "Maze has not been built yet.");
            Room? room = _maze.FindRoom(roomId);
            return room?.IsTreasure ?? false;
        }
        public void DrawMaze()
        {
            ArgumentNullException.ThrowIfNull(_maze, "Maze has not been built yet.");
            for (int y = 0; y < _size; y++)
            {
                Console.WriteLine(new string('-', _size * 5));
                for (int x = 0; x < _size; x++)
                {
                    SetConsoleColor(y, x);
                    Console.Write($" {(_maze.Rooms[x, y].IsEntrance ? "Ent" : _maze.Rooms[x, y].IsTreasure ? "Trs" : _maze.Rooms[x, y].RoomType?.Name.Substring(0, 3))}");
                    ResetConsoleColor();
                    Console.Write("|");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }

        private static void ResetConsoleColor()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void SetConsoleColor(int y, int x)
        {
            if (_maze == null)
                return;
            if (_maze.Rooms[x, y].CausesInjury())
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.White;
            if (_maze.Rooms[x, y].IsSolutionPath)
                Console.BackgroundColor = ConsoleColor.Blue;
            else
                Console.BackgroundColor = ConsoleColor.Black;
            if (_maze.Rooms[x, y].IsEntrance || _maze.Rooms[x, y].IsTreasure)
                Console.BackgroundColor = ConsoleColor.Green;
        }
    }
}