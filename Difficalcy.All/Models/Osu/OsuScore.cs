using System.ComponentModel.DataAnnotations;
using Difficalcy.Models;

namespace Difficalcy.All.Models.Osu;

public record OsuScore : Score, IValidatableObject
{
    [Range(0, int.MaxValue)]
    public int? Combo { get; init; }

    [Range(0, int.MaxValue)]
    public int Misses { get; init; } = 0;

    [Range(0, int.MaxValue)]
    public int Mehs { get; init; } = 0;

    [Range(0, int.MaxValue)]
    public int Oks { get; init; } = 0;

    [Range(0, int.MaxValue)]
    public int? SliderTails { get; init; }

    [Range(0, int.MaxValue)]
    public int? SliderTicks { get; init; } // LargeTickHits

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Misses > 0 && Combo is null)
        {
            yield return new ValidationResult(
                "Combo must be specified if Misses are greater than 0.",
                [nameof(Combo)]
            );
        }
    }
}