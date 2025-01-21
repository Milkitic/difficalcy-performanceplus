using Difficalcy.Models;

namespace Difficalcy.All.Models.Taiko
{
    public record TaikoPerformance : Performance
    {
        public double Difficulty { get; init; }
        public double Accuracy { get; init; }
    }
}
