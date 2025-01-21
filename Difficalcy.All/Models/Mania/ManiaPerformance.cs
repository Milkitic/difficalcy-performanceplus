using Difficalcy.Models;

namespace Difficalcy.All.Models.Mania;

public record ManiaPerformance : Performance
{
    public double Difficulty { get; init; }
}