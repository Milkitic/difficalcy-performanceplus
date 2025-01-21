using Difficalcy.Models;

namespace Difficalcy.All.Models.Ctb;

public record CatchCalculation : Calculation<CatchDifficulty, CatchPerformance>
{
    public double Accuracy { get; init; }
    public double Combo { get; init; }
}