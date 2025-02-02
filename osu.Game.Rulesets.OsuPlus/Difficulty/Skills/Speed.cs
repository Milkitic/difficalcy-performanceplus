﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.OsuPlus.Difficulty.Preprocessing;

namespace osu.Game.Rulesets.OsuPlus.Difficulty.Skills
{
    /// <summary>
    /// Represents the skill required to press keys with regards to keeping up with the speed at which objects need to be hit.
    /// </summary>
    public class Speed : StrainSkill
    {
        protected virtual double SkillMultiplier => 2600;
        protected virtual double StrainDecayBase => 0.1;

        private double currentStrain;

        private double strainDecay(double ms) => Math.Pow(StrainDecayBase, ms / 1000);

        protected override double CalculateInitialStrain(double time, DifficultyHitObject current) => currentStrain * strainDecay(time - current.Previous(0).StartTime);

        public Speed(Mod[] mods) : base(mods)
        {
        }

        protected override double StrainValueAt(DifficultyHitObject current)
        {
            currentStrain *= strainDecay(current.DeltaTime);
            currentStrain += StrainValueOf(current) * SkillMultiplier;

            return currentStrain;
        }

        protected virtual double StrainValueOf(DifficultyHitObject current)
        {
            var osuCurrent = (OsuDifficultyHitObject)current;

            double ms = osuCurrent.LastTwoStrainTime / 2;

            // Curves are similar to 2.5 / ms for tapValue and 1 / ms for streamValue, but scale better at high BPM.
            double tapValue = 30 / Math.Pow(ms - 20, 2) + 2 / ms;
            double streamValue = 12.5 / Math.Pow(ms - 20, 2) + 0.25 / ms + 0.005;

            return (1 - osuCurrent.Flow) * tapValue + osuCurrent.Flow * streamValue;
        }
    }
}
