using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeAdvanture.Models;
using MazeAdvanture.Models.Behaviours;
using MazeAdvanture.Services;
using Xunit;

namespace MazeAdvantureTests.Models
{
    public class MazeTests
    {
        private List<RoomType> roomTypes;
        private Maze maze;

        public MazeTests()
        {
            roomTypes =
            [
                new RoomType("Forest", "Yuo are in forest"),
                new RoomType("Marsh", "you are in a marsh", new List<Behaviour> { new Behaviour(EBehaveType.Trap, "Sink", 0.3) }),
                new RoomType("Hill", "you are in a hill"),
                new RoomType("Desert", "you are in a desert", new List<Behaviour> { new Behaviour(EBehaveType.Trap, "Dehydrate", 0.3) }),
            ];
            maze = MazeGenerator.BuildMaze(5, roomTypes);
        }

        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(5, 4, 0)]
        [InlineData(13, 2, 2)]

        public void TestFindRoom_ReturnsRoom(int roomId, int X, int Y)
        {
            // Act
            var result = maze.FindRoom(roomId);

            // Assert
            Assert.Equal(X, result?.X);
            Assert.Equal(Y, result?.Y);

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(30)]
        public void TestFindRoom_ReturnsNull(int roomId)
        {
            // Act
            var result = maze.FindRoom(roomId);

            // Assert
            Assert.Null(result);
        }
        [Theory]
        [InlineData(6, 'N', 1)]
        [InlineData(3, 'W', 2)]
        [InlineData(3, 'E', 4)]
        [InlineData(8, 'S', 13)]
        [InlineData(1, 'N', null)]
        [InlineData(21, 'S', null)]
        [InlineData(6, 'W', null)]
        [InlineData(5, 'E', null)]
        [InlineData(30, 'E', null)]  //not existing room
        public void TestGetRoom_ReturnsRoomIdOrNull(int roomId, char dir, int? expected)
        {
            // Act
            var result = maze.GetRoom(roomId, dir);

            // Assert
            Assert.Equal(expected, result);

        }
    }
}