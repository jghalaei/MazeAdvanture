namespace MazeAdvanture.Models.Behaviours
{
    public class Behaviour
    {
        public EBehaveType BehaveType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Probability { get; set; }
        public bool CoausesInjury { get; protected set; }

        public Behaviour(EBehaveType behaveType, string name, double probability, string description = "")
        {
            BehaveType = behaveType;
            Name = name;
            Probability = probability;
            Description = description;
            CoausesInjury = (behaveType == EBehaveType.Trap);
        }



        public virtual BehaviourResult Behave(Maze maze)
        {
            return new BehaviourResult(BehaveType);
        }

    }
}