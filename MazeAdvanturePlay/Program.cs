using System.ComponentModel.DataAnnotations;
using AtlasCopco.Integration.Maze;
using MazeAdvanture.Services;
using MazeAdvanture.Settings;
using Microsoft.Extensions.Configuration;

if (args.Length < 2)
{
    Console.WriteLine("Usage: MazeAdvanture.exe <draw/play> <size>");
    return;
}
var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json")
.Build();
MazeSettings mazeSettings = config.Get<MazeSettings>() ?? throw new Exception();
List<char> validDirs = new List<char> { 'N', 'S', 'E', 'W' };

string command = args[0];
int size;
if (int.TryParse(args[1], out size) == false)
    size = 10;
// draw
if (command == "draw")
{
    MazeIntegration m1 = new MazeIntegration(mazeSettings);
    m1.BuildMaze(size);
    m1.DrawMaze();
    return;
}
// play
IMazeIntegration mazeIntegration = new MazeIntegration(mazeSettings);
mazeIntegration.BuildMaze(10);

int roomId = mazeIntegration.GetEntranceRoom();
bool? result = null;
while (result == null && roomId != -1)
{
    result = CheckAndShowRoom(roomId);
    if (result == null)
        roomId = GetDirection(roomId);
}

int GetDirection(int roomId)
{

    while (true)
    {
        Console.Write("Enter direction (Q for Quit):");
        char dir = Console.ReadLine()![0];
        dir = char.ToUpper(dir);
        if (dir == 'Q') return -1;
        if (validDirs.Contains(dir) && mazeIntegration.GetRoom(roomId, dir) != null)
            return mazeIntegration.GetRoom(roomId, dir) ?? roomId;
        else
            Console.WriteLine("Invalid direction");
    }
}

bool? CheckAndShowRoom(int roomId)
{
    Console.WriteLine($"Current room: {roomId}, description: {mazeIntegration.GetDescription(roomId)}");
    if (mazeIntegration.HasTreasure(roomId))
    {
        Console.WriteLine("You found a treasure!");
        return true;
    }
    if (mazeIntegration.CausesInjury(roomId))
    {
        Console.WriteLine("You got hurt!");
        return false;
    }

    Console.WriteLine($"CausesInjury: {mazeIntegration.CausesInjury(roomId)}");
    Console.WriteLine("Valid directions: ");
    foreach (char d in validDirs)
    {
        if (mazeIntegration.GetRoom(roomId, d) != null)
        {
            int id = mazeIntegration.GetRoom(roomId, d) ?? 0;
            Console.WriteLine($"{d}: {id}, Description: {mazeIntegration.GetDescription(id)}");
        }
    }
    return null;
}
