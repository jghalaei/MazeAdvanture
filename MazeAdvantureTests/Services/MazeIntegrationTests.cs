using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MazeAdvanture.Models;
using MazeAdvanture.Models.Behaviours;
using MazeAdvanture.Services;
using MazeAdvanture.Settings;
using Xunit;

namespace MazeAdvantureTests.Services
{
    public class MazeIntegrationTests
    {
        private readonly MazeSettings _mazeSettings;

        public MazeIntegrationTests()
        {
            _mazeSettings = new MazeSettings();
            _mazeSettings.RoomTypes =
            [
                new RoomType("Forest", "Yuo are in forest"),
                new RoomType("Marsh", "you are in a marsh", new List<Behaviour> { new Behaviour(EBehaveType.Trap,"Sink", 0.3) }),
                new RoomType("Hill", "you are in a hill"),
                new RoomType("Desert", "you are in a desert", new List<Behaviour> { new Behaviour(EBehaveType.Trap,"Dehydrate", 0.3) }),
            ];
        }

        [Fact]
        public void BuildMaze_success()
        {
            // Arrange
            var mazeIntegration = new MazeIntegration(_mazeSettings);
            int size = 10;

            // Act
            mazeIntegration.BuildMaze(size);

            // Assert
            var id = mazeIntegration.GetEntranceRoom();
            Assert.True(id >= 0 && id < size * size);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(2)]

        public void BuildMaze_fail_invalid_size(int size)
        {
            // Arrange
            var mazeIntegration = new MazeIntegration(_mazeSettings);
            // Act
            Assert.Throws<ArgumentException>(() => mazeIntegration.BuildMaze(size));

        }

        [Fact]
        public void TestGetEntranceRoom()
        {
            // Arrange
            var mazeIntegration = new MazeIntegration(_mazeSettings);
            var size = 5;
            // Act
            mazeIntegration.BuildMaze(size);
            var id = mazeIntegration.GetEntranceRoom();

            // Assert
            Assert.True(id >= 0 && id <= size * size);
        }

        [Theory]
        [InlineData(5, 6, 'N', 1)]
        [InlineData(5, 3, 'W', 2)]
        [InlineData(5, 3, 'E', 4)]
        [InlineData(5, 8, 'S', 13)]
        [InlineData(5, 1, 'N', null)]
        [InlineData(5, 21, 'S', null)]
        [InlineData(5, 6, 'W', null)]
        [InlineData(5, 5, 'E', null)]
        [InlineData(5, 30, 'E', null)]  //not existing room
        public void TestGetRoom(int size, int roomId, char direction, int? result)
        {
            // Arrange
            var mazeIntegration = new MazeIntegration(_mazeSettings);
            // Act
            mazeIntegration.BuildMaze(size);
            // Assert
            Assert.Equal(mazeIntegration.GetRoom(roomId, direction), result);
        }


        [Fact]
        public void TestGetDescription()
        {
            // Arrange
            var mazeIntegration = new MazeIntegration(_mazeSettings);
            int roomId = 1;

            // Act
            mazeIntegration.BuildMaze(5);
            var result = mazeIntegration.GetDescription(roomId);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void TestHasTreasure()
        {
            // Arrange
            var mazeIntegration = new MazeIntegration(_mazeSettings);
            int size = 4;
            bool trs = false;

            // Act
            mazeIntegration.BuildMaze(size);
            for (int i = 1; i <= size * size; i++)
                if (mazeIntegration.HasTreasure(i)) trs = true;

            // Assert
            Assert.True(trs);
        }


    }
}