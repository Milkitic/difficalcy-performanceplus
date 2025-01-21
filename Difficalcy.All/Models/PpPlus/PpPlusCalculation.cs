using Difficalcy.Models;

namespace Difficalcy.All.Models.PpPlus
{
    public record PpPlusCalculation : Calculation<PpPlusDifficulty, PpPlusPerformance>
    {
        public double Accuracy { get; init; }
        public double Combo { get; init; }
    }
}
