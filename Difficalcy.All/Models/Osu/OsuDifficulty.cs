using Difficalcy.Models;

namespace Difficalcy.All.Models.Osu;

public record OsuDifficulty : Difficulty
{
    public double Aim { get; init; }
    public double Speed { get; init; }
    public double Flashlight { get; init; }
}