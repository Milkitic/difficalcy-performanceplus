// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.OsuPlus.Judgements;

namespace osu.Game.Rulesets.OsuPlus.Objects
{
    public class HitCircle : OsuHitObject
    {
        public override Judgement CreateJudgement() => new OsuJudgement();
    }
}
