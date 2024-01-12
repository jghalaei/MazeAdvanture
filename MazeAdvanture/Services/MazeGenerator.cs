using MazeAdvanture.Models;

namespace MazeAdvanture.Services
{

    public class MazeGenerator
    {

        private Random random;
        private List<RoomType> roomTypes;
        private List<RoomType> safeRoomTypes;
        private int size;
        private Room[,] rooms;
        private Room entrance;

        public static Maze BuildMaze(int size, List<RoomType> roomTypes)
        {
            MazeGenerator generator = new MazeGenerator(size, roomTypes);
            return generator.GenereateMaze();
        }
        private MazeGenerator(int size, List<RoomType> roomTypes)
        {
            random = new Random();
            this.size = size;
            this.roomTypes = roomTypes;
            safeRoomTypes = GetSafeRoomTypes();
            if (safeRoomTypes.Count == 0)
                throw new ArgumentException("There are no safe rooms");
            rooms = InitializeMaze();
            entrance = SetRandomEntrance();

        }

        private List<RoomType> GetSafeRoomTypes()
        {
            return roomTypes.FindAll(r => r.Behaviours == null || r.Behaviours.Count == 0 || r.Behaviours.All(b => !b.CoausesInjury));
        }

        private Maze GenereateMaze()
        {
            while (!SetPathToTreasure()) ;
            FillAllMaze();
            return new Maze(size, rooms, entrance);

        }

        private Room[,] InitializeMaze()
        {
            var rooms = new Room[size, size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    rooms[x, y] = new Room(x + y * size + 1, x, y);
            return rooms;
        }
        private Stack<(int, int)> pathStack = new();
        private bool[,] visited = new bool[0, 0];
        private Queue<(int, int)> queueNextRooms = new();

        private bool SetPathToTreasure()
        {
            pathStack = new();
            visited = new bool[size, size];

            int currentX = entrance.X;
            int currentY = entrance.Y;

            while (!IsTreasure(currentX, currentY))
            {
                visited[currentX, currentY] = true;
                pathStack.Push((currentX, currentY));
                (int nextX, int nextY) = GetNextRoom(currentX, currentY);

                if (nextX == currentX && nextY == currentY)
                {
                    while (pathStack.Count > 0 && !HasAdjacentUnvisitedRoom(currentX, currentY))
                    {
                        (currentX, currentY) = pathStack.Pop();
                    }
                    if (pathStack.Count == 0)
                    {
                        return false;
                    }
                }
                else
                {
                    currentX = nextX;
                    currentY = nextY;
                }
            }

            pathStack.Push((currentX, currentY));
            rooms[currentX, currentY].IsTreasure = true;

            while (pathStack.Count > 0)
            {
                (int x, int y) = pathStack.Pop();
                rooms[x, y].IsSolutionPath = true;
                rooms[x, y].RoomType = safeRoomTypes[random.Next(safeRoomTypes.Count)];
            }
            return true;
        }

        private bool HasAdjacentUnvisitedRoom(int X, int Y)
        {
            return X > 0 && !visited[X - 1, Y] ||
                    Y > 0 && !visited[X, Y - 1] ||
                    X < size - 1 && !visited[X + 1, Y] ||
                    Y < size - 1 && !visited[X, Y + 1];
        }
        private (int nextX, int nextY) GetNextRoom(int currentX, int currentY)
        {
            //dequeue the next room
            if (queueNextRooms.Count > 0)
                return queueNextRooms.Dequeue();
            //pick a random valid direction
            List<(int, int)> directions = new List<(int, int)> { (0, 1), (1, 0), (0, -1), (-1, 0) };
            directions.Sort((a, b) => random.Next(-1, 2));
            int dirX = 0, dirY = 0;

            for (int i = 0; i < 4; i++)
            {
                (dirX, dirY) = directions[i];
                if (IsValidRoom(currentX + dirX, currentY + dirY))
                    break;
            }
            if (!IsValidRoom(currentX + dirX, currentY + dirY))
                return (currentX, currentY);
            queueNextRooms.Enqueue((currentX + dirX, currentY + dirY));
            //pick random number of rooms
            int rpts = random.Next(1, size / 2);
            for (int i = 0; i < rpts; i++)
            {
                currentX += dirX;
                currentY += dirY;
                if (IsValidRoom(currentX + dirX, currentY + dirY))
                    queueNextRooms.Enqueue((currentX + dirX, currentY + dirY));
                else
                    break;
            }
            //in case of no room found
            if (queueNextRooms.Count == 0)
                return (currentX, currentY);
            //dequeue the next room
            return queueNextRooms.Dequeue();
        }

        private bool IsValidRoom(int X, int Y)
        {
            return X >= 0 && X < size && Y >= 0 && Y < size && !visited[X, Y];
        }

        private bool IsTreasure(int currentX, int currentY)
        {
            return random.NextDouble() < 0.1 && currentX != entrance.X && currentY != entrance.Y;
        }


        private Room SetRandomEntrance()
        {
            int edge = random.Next(4);
            int x = 0;
            int y = 0;
            switch (edge)
            {
                case 0:
                    x = 0;
                    y = random.Next(size);
                    break;
                case 1:
                    x = random.Next(size);
                    y = size - 1;
                    break;
                case 2:
                    x = size - 1;
                    y = random.Next(size);
                    break;
                case 3:
                    x = random.Next(size);
                    y = 0;
                    break;
            };
            var entrance = rooms[x, y];
            entrance.IsEntrance = true;
            return entrance;
        }
        private void FillAllMaze()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (rooms[x, y].RoomType == null)
                        rooms[x, y].RoomType = roomTypes[random.Next(roomTypes.Count)];
                }
            }
        }
    }
}