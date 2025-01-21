using Difficalcy.Models;

namespace Difficalcy.All.Models.Taiko
{
    public record TaikoDifficulty : Difficulty
    {
        public double Stamina { get; init; }
        public double Rhythm { get; init; }
        public double Colour { get; init; }
    }
}
