// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Osu.Objects;

namespace osu.Game.Rulesets.OsuPlus.Objects.Drawables
{
    public partial class DrawableSpinnerBonusTick : DrawableSpinnerTick
    {
        public DrawableSpinnerBonusTick()
            : base(null)
        {
        }

        public DrawableSpinnerBonusTick(SpinnerBonusTick spinnerTick)
            : base(spinnerTick)
        {
        }
    }
}
