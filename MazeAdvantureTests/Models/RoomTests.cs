using Xunit;
using MazeAdvanture.Models;
using MazeAdvanture.Models.Behaviours;
using Moq;

namespace MazeAdvantureTests.Models;

public class RoomTests
{
    [Fact]
    public void TestCausesInjury_ReturnsTrue_WhenTrapTrgigerred()
    {
        Mock<IRoomType> mock = new();

        mock.Setup(x => x.TrigerredBehaviour()).Returns(new Behaviour(EBehaveType.Trap, "Sink", 1));
        // Arrange

        var room = new Room(1, 0, 0)
        {
            RoomType = mock.Object
        };

        // Act
        var result = room.CausesInjury();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestCausesInjury_ReturnsFalse_WhenTrapNotTrgigerred()
    {
        // Arrange
        Mock<IRoomType> mock = new();
        mock.Setup(x => x.TrigerredBehaviour()).Returns(() => null);

        var room = new Room(1, 0, 0)
        {
            RoomType = mock.Object
        };

        // Act
        var result = room.CausesInjury();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetDescription_ReturnsRoomTypeDescription()
    {
        // Arrange
        var room = new Room(1, 0, 0)
        {
            RoomType = new RoomType("Test", "This is a test room.")
        };

        // Act
        var result = room.GetDescription();

        // Assert
        Assert.StartsWith("This is a test room.", result);
    }


    [Fact]
    public void GetDescription_ReturnsRoomTypeDescription_AndTrigerredBehaviour()
    {
        // Arrange
        var room = new Room(1, 0, 0)
        {
            RoomType = new RoomType("Test", "This is a test room.",
        new List<Behaviour> { new Behaviour(EBehaveType.Trap, "Sink", 1, "This is a test behaviour.") })
        };


        // Act
        var result = room.GetDescription();

        // Assert
        Assert.Equal("This is a test room.\r\nThis is a test behaviour.", result);
    }
}