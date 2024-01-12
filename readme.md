# Maze Integration

This Project include an API that can generate random mazes for the Treasure Hunter to practice on. The solution
will be plugged into an existing maze simulator, in which a player navigates a maze in search of the
big treasure.

## Projects

### MazeAdvanture:

The MazeAdvnatureLibrary provide the main functionality for the solution. It includes MazeIntegration Class which implement the IMazeIntegration.

### MazeAdvantureTests:

Includes Xunit tests for testing the MazeAdvanture.

### MazeAdvanturePlay:

a console app to provide a simple way to play and test the functionality of the solution.

## Usage

### The MazeAdvanture library provides below requirements:

- Produces the maze with any size. The maze always are square.
- The library get the RoomTypes and their behaviours as a settings. So that it can be very flexible. The MazeIntegration class would need MazeSettings in their constructor. you can set it using setting file. a sample of the settings file is provided in the following.
- Although the maze is generated randomly, The maze is guaranteed to have atleast one solution to solve. Plase make sure at least one of room types are safe.
- Room Types can have different behaviours. For now only Trap behaviours are implemented. However the library can be simply extended to support other kinds of behaviours.
- A room can have none to many behaviuors. If the summary of probabilities are more than 1, the chances will be devided to the sum of the probabilites.
- The MazeIntegration class Also contains **DrawMaze()**, which print the maze in console. It can give a better imagination of the produced maze for testing the library.
- Provides The Entrance Room Id.
- Proviced The Description of each room by its Id.
- Give the Adjacent Rooms by roomId and Direction.
- Check if the room has treasure.
- Check if the room caused injury.

## An Example of the Settings File:

```json
{
  "RoomTypes": [
    {
      "Name": "Forest",
      "Description": "A lush, green room filled with towering trees, rustling leaves, and the occasional bird song."
    },
    {
      "Name": "Marsh",
      "Description": "A wet, foggy area with soft, muddy ground and the sound of distant water",
      "Behaviours": [
        {
          "BehaveType": "Trap",
          "Name": "Sink",
          "Probability": 0.3,
          "Description": "You suddenly start sinking into hidden quicksand, struggling for a foothold."
        }
      ]
    },
    {
      "name": "Desert",
      "description": "A hot, dry room with sandy floors, sparse vegetation, and a glaring sun.",
      "Behaviours": [
        {
          "BehaveType": "Trap",
          "Name": "Dehydration",
          "Probability": 0.2,
          "Description": "Intense heat takes its toll, leaving you dizzy and dangerously dehydrated."
        }
      ]
    },
    {
      "Name": "Hills",
      "Description": "Rolling hills with grassy knolls and a panoramic view of the distant terrain."
    }
  ]
}
```
