using MazeAdvanture.Models;

namespace MazeAdvanture.Services
{

    public class MazeGenerator
    {
        private const double TreasureProbability = 0.1;
        private Random random;
        private List<RoomType> roomTypes;
        private List<RoomType> safeRoomTypes;
        private int size;
        private Room[,] rooms;
        private Room entrance;

        private bool[,] visitedRooms = new bool[0, 0];
        private Queue<(int, int)> queueNextRooms = new();
        public static Maze BuildMaze(int size, List<RoomType> roomTypes)
        {
            MazeGenerator generator = new MazeGenerator(size, roomTypes);
            return generator.GenerateMaze();
        }
        private MazeGenerator(int size, List<RoomType> roomTypes)
        {
            if (size <= 2) throw new ArgumentException("Size must be greater than 1");
            random = new Random();
            this.size = size;
            this.roomTypes = roomTypes ?? throw new ArgumentNullException("RoomTypes cannot be null");
            safeRoomTypes = GetSafeRoomTypes();
            if (safeRoomTypes.Count == 0)
                throw new ArgumentException("There are no safe rooms");
            rooms = InitializeMaze();
            entrance = SetRandomEntrance();
        }

        private Maze GenerateMaze()
        {
            while (!SetPathToTreasure()) ;
            FillAllMaze();
            return new Maze(size, rooms, entrance);

        }
        private List<RoomType> GetSafeRoomTypes()
        {
            return roomTypes.FindAll(r => r.Behaviours == null || r.Behaviours.Count == 0 || r.Behaviours.All(b => !b.CoausesInjury));
        }
        private Room[,] InitializeMaze()
        {
            var rooms = new Room[size, size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    rooms[x, y] = new Room(x + y * size + 1, x, y);
            return rooms;
        }
        private bool SetPathToTreasure()
        {
            Stack<(int, int)> stackSafePath = new();
            visitedRooms = new bool[size, size];

            int currentX = entrance.X;
            int currentY = entrance.Y;

            while (!IsTreasure(currentX, currentY))
            {
                visitedRooms[currentX, currentY] = true;
                stackSafePath.Push((currentX, currentY));

                (int nextX, int nextY) = GetNextRoom(currentX, currentY);
                if (nextX == currentX && nextY == currentY)//if room is locked, backtrack
                {
                    if (!BacktrackPath(stackSafePath, ref currentX, ref currentY))
                        return false;
                }
                else
                {
                    //set next room
                    currentX = nextX;
                    currentY = nextY;
                }
            }
            stackSafePath.Push((currentX, currentY));
            rooms[currentX, currentY].IsTreasure = true;

            MarkSafePath(stackSafePath);
            return true;
        }

        private void MarkSafePath(Stack<(int, int)> stackSafePath)
        {
            while (stackSafePath.Count > 0)
            {
                (int x, int y) = stackSafePath.Pop();
                rooms[x, y].IsSolutionPath = true;
                rooms[x, y].RoomType = safeRoomTypes[random.Next(safeRoomTypes.Count)];
            }
        }

        private bool BacktrackPath(Stack<(int, int)> stackSafePath, ref int currentX, ref int currentY)
        {
            while (!HasAdjacentUnvisitedRoom(currentX, currentY))
            {
                if (stackSafePath.Count == 0) // no safe path
                    return false;
                (currentX, currentY) = stackSafePath.Pop();
            }
            return true;
        }

        private bool HasAdjacentUnvisitedRoom(int X, int Y)
        {
            return (X > 0 && !visitedRooms[X - 1, Y]) ||
                   (Y > 0 && !visitedRooms[X, Y - 1]) ||
                   (X < size - 1 && !visitedRooms[X + 1, Y]) ||
                   (Y < size - 1 && !visitedRooms[X, Y + 1]);
        }
        private (int nextX, int nextY) GetNextRoom(int currentX, int currentY)
        {
            if (queueNextRooms.Count > 0)
                return queueNextRooms.Dequeue();
            AddRandomRoomsToQueue(currentX, currentY);
            return queueNextRooms.Count > 0 ? queueNextRooms.Dequeue() : (currentX, currentY);
        }

        private void AddRandomRoomsToQueue(int currentX, int currentY)
        {
            var directions = GetRandomDirections();
            int minLength = 1;
            int maxLength = size / 2;

            foreach (var (dirX, dirY) in directions)
            {
                if (!IsValidRoom(currentX + dirX, currentY + dirY))
                    continue; //go for next direction
                for (int i = 0; i < random.Next(minLength, maxLength); i++)
                {
                    if (!IsValidRoom(currentX + dirX, currentY + dirY))
                        return; // Exit after adding last valid rooms of direction to queue
                    currentX += dirX;
                    currentY += dirY;
                    queueNextRooms.Enqueue((currentX, currentY));
                }
                return; // Exit after adding rooms to queue
            }
        }

        private List<(int x, int y)> GetRandomDirections()
        {
            var dirs = new List<(int x, int y)> { (0, 1), (1, 0), (0, -1), (-1, 0) };
            int n = dirs.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (int, int) value = dirs[k];
                dirs[k] = dirs[n];
                dirs[n] = value;
            }
            return dirs;
        }

        private bool IsValidRoom(int x, int y)
        {
            return x >= 0 && x < size && y >= 0 && y < size && !visitedRooms[x, y];
        }

        private bool IsTreasure(int currentX, int currentY)
        {
            return random.NextDouble() < TreasureProbability && currentX != entrance.X && currentY != entrance.Y;
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