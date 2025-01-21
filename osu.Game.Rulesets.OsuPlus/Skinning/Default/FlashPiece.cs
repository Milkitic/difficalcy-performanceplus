// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.OsuPlus.Objects;

namespace osu.Game.Rulesets.OsuPlus.Skinning.Default
{
    public partial class FlashPiece : Container
    {
        public FlashPiece()
        {
            Size = OsuHitObject.OBJECT_DIMENSIONS;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Blending = BlendingParameters.Additive;
            Alpha = 0;

            Child = new CircularContainer
            {
                Masking = true,
                RelativeSizeAxes = Axes.Both,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both
                }
            };
        }
    }
}
