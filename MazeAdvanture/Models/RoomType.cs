using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Xml.XPath;
using MazeAdvanture.Models.Behaviours;

namespace MazeAdvanture.Models
{
    public class RoomType : IRoomType
    {
        public RoomType(string name, string description, List<Behaviour>? behaviours = null)
        {
            Name = name;
            Description = description;
            Behaviours = behaviours ?? new();
            double sumProbs = Behaviours.Sum(b => b.Probability);
            if (sumProbs > 1)
            {
                Behaviours = Behaviours
                    .Select(b => new Behaviour(b.BehaveType, b.Name, b.Probability / sumProbs))
                    .ToList();
            }

        }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Behaviour> Behaviours { get; private set; }

        public Behaviour? TrigerredBehaviour()
        {
            if (Behaviours == null || Behaviours.Count == 0)
                return null;
            var rnd = Random.Shared.NextDouble();
            double min = 0;
            double max = 0;
            foreach (var behaviour in Behaviours)
            {
                min = max;
                max += behaviour.Probability;
                if (rnd > min && rnd <= max)
                    return behaviour;
            }
            return null;
        }

    }

}