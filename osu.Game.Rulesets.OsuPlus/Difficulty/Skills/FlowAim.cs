﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.OsuPlus.Difficulty.Preprocessing;
using osu.Game.Rulesets.OsuPlus.Objects;

namespace osu.Game.Rulesets.OsuPlus.Difficulty.Skills
{
    public class FlowAim : Aim
    {
        protected override double CalculateAimValue(OsuDifficultyHitObject current) => CalculateFlowAimValue(current) * CalculateSmallCircleBonus(((OsuHitObject)current.BaseObject).Radius);

        public FlowAim(Mod[] mods) : base(mods)
        {
        }
    }
}
