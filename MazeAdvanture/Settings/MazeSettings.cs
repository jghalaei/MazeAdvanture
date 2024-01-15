using MazeAdvanture.Models;
using MazeAdvanture.Models.Behaviours;

namespace MazeAdvanture.Settings
{

    public class MazeSettings
    {
        public static MazeSettings Default = new()
        {
            RoomTypes = [
                new RoomType("Forest",  "A lush, green room filled with towering trees, rustling leaves, and the occasional bird song."),
                new RoomType("Marsh", "A wet, foggy area with soft, muddy ground and the sound of distant water",
                            [ new Behaviour(EBehaveType.Trap, "Sink", 0.3,"You suddenly start sinking into hidden quicksand, struggling for a foothold.") ]),
                new RoomType("Hill", "A hot, dry room with sandy floors, sparse vegetation, and a glaring sun."),
                new RoomType("Desert", "Rolling hills with grassy knolls and a panoramic view of the distant terrain.",
                            [ new Behaviour(EBehaveType.Trap, "Dehydrate", 0.2,"Intense heat takes its toll, leaving you dizzy and dangerously dehydrated.") ]),
        ]
        };
        public List<RoomType> RoomTypes { get; set; } = new();
    }
}