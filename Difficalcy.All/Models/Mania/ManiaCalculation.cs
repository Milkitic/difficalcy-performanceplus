using Difficalcy.Models;

namespace Difficalcy.All.Models.Mania;

public record ManiaCalculation : Calculation<ManiaDifficulty, ManiaPerformance>
{
    public double Accuracy { get; init; }
}