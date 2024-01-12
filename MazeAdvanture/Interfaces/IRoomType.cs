using MazeAdvanture.Models.Behaviours;

namespace MazeAdvanture.Models
{
    public interface IRoomType
    {
        string Name { get; }
        string Description { get; }
        List<Behaviour>? Behaviours { get; }
        Behaviour? TrigerredBehaviour();
    }

}