using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeAdvanture.Models;
using MazeAdvanture.Models.Behaviours;
using MazeAdvanture.Services;
using Xunit;

namespace MazeAdvantureTests.Services
{

    public class MazeGeneratorTests
    {
        private List<RoomType> roomTypes;
        public MazeGeneratorTests()
        {
            roomTypes =
            [
                new RoomType("Forest", "Yuo are in forest"),
                new RoomType("Marsh", "you are in a marsh", new List<Behaviour> { new Behaviour(EBehaveType.Trap,"Sink", 0.3) }),
                new RoomType("Hill", "you are in a hill"),
                new RoomType("Desert", "you are in a desert", new List<Behaviour> { new Behaviour(EBehaveType.Trap,"Dehydrate", 0.3) }),
            ];
        }
        [Fact]
        public void BuildMaze_ReturnsMazeWithCorrectSize()
        {
            // Arrange
            int size = 10;

            // Act
            Maze maze = MazeGenerator.BuildMaze(size, roomTypes);

            // Assert
            Assert.Equal(size, maze.Size);
            Assert.NotNull(maze.Rooms);
            Assert.Equal(size, maze.Rooms.GetLength(0));
            Assert.Equal(size, maze.Rooms.GetLength(1));
        }
        [Fact]
        public void BuildMaze_ReturnsMazeWithEntranceAndTreasure()
        {
            // Arrange 
            int size = 10;

            // Act
            Maze maze = MazeGenerator.BuildMaze(size, roomTypes);

            //Assert    
            Assert.True(maze.EntranceRoom.IsEntrance);
            var HasTreasure = false;
            foreach (var room in maze.Rooms)
            {
                if (room.IsTreasure)
                {
                    HasTreasure = true;
                    break;
                }
            }
            Assert.True(HasTreasure);
        }
        [Fact]

        public void BuildMaze_ReturnsSolvableMaze()
        {
            for (int i = 0; i < 5; i++)
            {            // Arrange
                int size = Random.Shared.Next(4, 16);
                // Act
                Maze maze = MazeGenerator.BuildMaze(size, roomTypes);

                // Assert
                Assert.True(maze.IsSolvable());
            }
        }


    }
}