﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.OsuPlus.Skinning.Default;
using osuTK;

namespace osu.Game.Rulesets.OsuPlus.Edit.Blueprints.HitCircles.Components
{
    public partial class HitCirclePiece : BlueprintPiece<HitCircle>
    {
        public HitCirclePiece()
        {
            Origin = Anchor.Centre;

            Size = OsuHitObject.OBJECT_DIMENSIONS;

            CornerRadius = Size.X / 2;
            CornerExponent = 2;

            InternalChild = new RingPiece();
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;
        }

        public override void UpdateFrom(HitCircle hitObject)
        {
            base.UpdateFrom(hitObject);

            Scale = new Vector2(hitObject.Scale);
        }
    }
}
