using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using MazeAdvanture.Models.Behaviours;

namespace MazeAdvanture.Models
{
    public record Room(int Id, int X, int Y)
    {
        public IRoomType? RoomType { get; set; }
        public bool IsSolutionPath { get; set; }
        public bool IsEntrance { get; set; } = false;
        public bool IsTreasure { get; set; } = false;
        private Behaviour? TrigerredBehaviour;


        public bool CausesInjury()
        {
            if (TrigerredBehaviour == null)
                TrigerredBehaviour = RoomType?.TrigerredBehaviour();

            if (TrigerredBehaviour != null && TrigerredBehaviour.CoausesInjury)
                return true;
            return false;
        }

        public string GetDescription()
        {
            if (TrigerredBehaviour == null)
                TrigerredBehaviour = RoomType?.TrigerredBehaviour();
            string description = $"{RoomType?.Description}\r\n{TrigerredBehaviour?.Description}";
            return description;
        }


    }
}