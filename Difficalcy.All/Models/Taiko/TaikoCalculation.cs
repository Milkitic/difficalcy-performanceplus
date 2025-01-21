using Difficalcy.Models;

namespace Difficalcy.All.Models.Taiko
{
    public record TaikoCalculation : Calculation<TaikoDifficulty, TaikoPerformance>
    {
        public double Accuracy { get; init; }
        public double Combo { get; init; }
    }
}
