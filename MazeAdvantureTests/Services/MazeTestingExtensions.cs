using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeAdvanture.Models;

namespace MazeAdvantureTests.Services
{
    public static class MazeTestingExtensions
    {

        public static bool IsSolvable(this Maze maze)
        {
            return DFS(maze, new bool[maze.Size, maze.Size], maze.EntranceRoom);
        }

        private static bool DFS(Maze maze, bool[,] visited, Room room)
        {
            if (room.IsTreasure)
            {
                return true;
            }
            visited[room.X, room.Y] = true;

            foreach (var adjacentRoom in GetAdjacentRooms(maze, room))
            {
                if (!visited[adjacentRoom.X, adjacentRoom.Y] && !adjacentRoom.CausesInjury() && DFS(maze, visited, adjacentRoom))
                {
                    return true;
                }
            }
            return false;
        }

        private static List<Room> GetAdjacentRooms(Maze maze, Room room)
        {
            List<Room> lst = new List<Room>();
            if (room.X > 0) lst.Add(maze.Rooms[room.X - 1, room.Y]);
            if (room.X < maze.Size - 1) lst.Add(maze.Rooms[room.X + 1, room.Y]);
            if (room.Y > 0) lst.Add(maze.Rooms[room.X, room.Y - 1]);
            if (room.Y < maze.Size - 1) lst.Add(maze.Rooms[room.X, room.Y + 1]);
            return lst;
        }
    }
}