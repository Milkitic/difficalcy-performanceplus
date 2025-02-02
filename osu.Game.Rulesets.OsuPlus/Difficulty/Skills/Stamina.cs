﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.OsuPlus.Difficulty.Preprocessing;

namespace osu.Game.Rulesets.OsuPlus.Difficulty.Skills
{
    public class Stamina : Speed
    {
        protected override double SkillMultiplier => base.SkillMultiplier * 0.3;
        protected override double StrainDecayBase => 0.45;

        public Stamina(Mod[] mods) : base(mods)
        {
        }

        protected override double StrainValueOf(DifficultyHitObject current)
        {
            var osuCurrent = (OsuDifficultyHitObject)current;

            double ms = osuCurrent.LastTwoStrainTime / 2;

            double tapValue = 2 / (ms - 20);
            double streamValue = 1 / (ms - 20);

            return (1 - osuCurrent.Flow) * tapValue + osuCurrent.Flow * streamValue;
        }
    }
}
