using Difficalcy.Models;

namespace Difficalcy.All.Models.Osu;

public record OsuCalculation : Calculation<OsuDifficulty, OsuPerformance>
{
    public double Accuracy { get; init; }
    public double Combo { get; init; }
}