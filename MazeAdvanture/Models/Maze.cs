using System.Runtime.CompilerServices;

namespace MazeAdvanture.Models
{
    public record Maze(int Size, Room[,] Rooms, Room EntranceRoom)
    {
        public Room? FindRoom(int roomId)
        {
            if (roomId < 1 || roomId > Size * Size) return null;
            int y = (roomId - 1) / Size;
            for (int i = 0; i < Size; i++)
            {
                if (Rooms[i, y].Id == roomId)
                    return Rooms[i, y];
            }
            return null;
        }

        public int? GetRoom(int roomId, char direction)
        {
            Room? room = FindRoom(roomId);
            if (room == null) return null;
            Room? nextRoom = null;
            switch (direction)
            {
                case 'W':
                    if (room.X > 0) nextRoom = Rooms[room.X - 1, room.Y];
                    break;
                case 'E':
                    if (room.X < Size - 1) nextRoom = Rooms[room.X + 1, room.Y];
                    break;
                case 'N':
                    if (room.Y > 0) nextRoom = Rooms[room.X, room.Y - 1];
                    break;
                case 'S':
                    if (room.Y < Size - 1) nextRoom = Rooms[room.X, room.Y + 1];
                    break;
            }
            if (nextRoom != null) return nextRoom.Id;
            return null;
        }
    }
}