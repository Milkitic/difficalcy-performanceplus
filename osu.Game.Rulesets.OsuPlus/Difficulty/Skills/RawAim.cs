﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.OsuPlus.Difficulty.Preprocessing;

namespace osu.Game.Rulesets.OsuPlus.Difficulty.Skills
{
    public class RawAim : Aim
    {
        protected override double CalculateAimValue(OsuDifficultyHitObject current) => CalculateJumpAimValue(current) + CalculateFlowAimValue(current);

        protected override double GetDistance(OsuDifficultyHitObject current) => current.RawJumpDistance;

        public RawAim(Mod[] mods) : base(mods)
        {
        }
    }
}
