using System.Text.Json;
using Difficalcy.All.Models.PpPlus;
using Difficalcy.Models;
using Difficalcy.Services;
using osu.Game.Online.API;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.OsuPlus;
using osu.Game.Rulesets.OsuPlus.Difficulty;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using LazerMod = osu.Game.Rulesets.Mods.Mod;

namespace Difficalcy.All.Services
{
    public class PpPlusCalculatorService
        : CalculatorService<PpPlusScore, PpPlusDifficulty, PpPlusPerformance, PpPlusCalculation>
    {
        private readonly IBeatmapProvider beatmapProvider;
        private readonly string osuCommitHash;

        public PpPlusCalculatorService(
            ICache cache,
            IBeatmapProvider beatmapProvider,
            string osuCommitHash
        )
            : base(cache)
        {
            this.beatmapProvider = beatmapProvider;
            this.osuCommitHash = osuCommitHash;
            if (osuCommitHash.Length == 0)
                throw new ArgumentException("OSU_COMMIT_HASH must be provided in configuration");
        }

        private OsuRuleset OsuRuleset { get; } = new OsuRuleset();

        public override CalculatorInfo Info
        {
            get
            {
                var packageName = "https://github.com/Syriiin/osu/tree/performanceplus";
                return new CalculatorInfo
                {
                    RulesetName = OsuRuleset.Description,
                    CalculatorName = "PerformancePlus (PP+)",
                    CalculatorPackage = packageName,
                    CalculatorVersion = osuCommitHash,
                    CalculatorUrl = $"{packageName}/tree/{osuCommitHash}",
                };
            }
        }

        protected override async Task EnsureBeatmap(string beatmapId)
        {
            await beatmapProvider.EnsureBeatmap(beatmapId);
        }

        protected override (object, string) CalculateDifficultyAttributes(
            string beatmapId,
            Mod[] mods
        )
        {
            var workingBeatmap = GetWorkingBeatmap(beatmapId);
            var lazerMods = mods.Select(ModToLazerMod).ToArray();

            var difficultyCalculator = OsuRuleset.CreateDifficultyCalculator(workingBeatmap);
            var difficultyAttributes =
                difficultyCalculator.Calculate(lazerMods) as OsuDifficultyAttributes;

            // Serialising anonymous object with same names because Mods and Skills can't be serialised
            return (
                difficultyAttributes,
                JsonSerializer.Serialize(
                    new
                    {
                        difficultyAttributes.StarRating,
                        difficultyAttributes.MaxCombo,
                        difficultyAttributes.AimDifficulty,
                        difficultyAttributes.JumpAimDifficulty,
                        difficultyAttributes.FlowAimDifficulty,
                        difficultyAttributes.PrecisionDifficulty,
                        difficultyAttributes.SpeedDifficulty,
                        difficultyAttributes.StaminaDifficulty,
                        difficultyAttributes.AccuracyDifficulty,
                        difficultyAttributes.ApproachRate,
                        difficultyAttributes.OverallDifficulty,
                        difficultyAttributes.HitCircleCount,
                        difficultyAttributes.SpinnerCount,
                    }
                )
            );
        }

        protected override object DeserialiseDifficultyAttributes(string difficultyAttributesJson)
        {
            return JsonSerializer.Deserialize<OsuDifficultyAttributes>(
                difficultyAttributesJson,
                new JsonSerializerOptions() { IncludeFields = true }
            );
        }

        protected override PpPlusCalculation CalculatePerformance(
            PpPlusScore score,
            object difficultyAttributes
        )
        {
            var osuDifficultyAttributes = (OsuDifficultyAttributes)difficultyAttributes;

            var workingBeatmap = GetWorkingBeatmap(score.BeatmapId);
            var mods = score.Mods.Select(ModToLazerMod).ToArray();
            var beatmap = workingBeatmap.GetPlayableBeatmap(OsuRuleset.RulesetInfo, mods);

            var combo =
                score.Combo
                ?? beatmap.HitObjects.Count
                    + beatmap.HitObjects.OfType<Slider>().Sum(s => s.NestedHitObjects.Count - 1);
            var statistics = GetHitResults(
                beatmap.HitObjects.Count,
                score.Misses,
                score.Mehs,
                score.Oks
            );
            var accuracy = CalculateAccuracy(statistics);

            var scoreInfo = new ScoreInfo()
            {
                Accuracy = accuracy,
                MaxCombo = combo,
                Statistics = statistics,
                Mods = mods,
            };

            var performanceCalculator = OsuRuleset.CreatePerformanceCalculator();
            var performanceAttributes =
                performanceCalculator.Calculate(scoreInfo, osuDifficultyAttributes)
                as OsuPerformanceAttributes;

            return new PpPlusCalculation()
            {
                Difficulty = GetDifficultyFromDifficultyAttributes(osuDifficultyAttributes),
                Performance = GetPerformanceFromPerformanceAttributes(performanceAttributes),
                Accuracy = accuracy,
                Combo = combo,
            };
        }

        private CalculatorWorkingBeatmap GetWorkingBeatmap(string beatmapId)
        {
            using var beatmapStream = beatmapProvider.GetBeatmapStream(beatmapId);
            return new CalculatorWorkingBeatmap(OsuRuleset, beatmapStream);
        }

        private LazerMod ModToLazerMod(Mod mod)
        {
            var apiMod = new APIMod { Acronym = mod.Acronym };
            foreach (var setting in mod.Settings)
                apiMod.Settings.Add(setting.Key, setting.Value);

            return apiMod.ToMod(OsuRuleset);
        }

        private static Dictionary<HitResult, int> GetHitResults(
            int hitResultCount,
            int countMiss,
            int countMeh,
            int countOk
        )
        {
            var countGreat = hitResultCount - countOk - countMeh - countMiss;

            return new Dictionary<HitResult, int>
            {
                { HitResult.Great, countGreat },
                { HitResult.Ok, countOk },
                { HitResult.Meh, countMeh },
                { HitResult.Miss, countMiss },
            };
        }

        private static double CalculateAccuracy(Dictionary<HitResult, int> statistics)
        {
            var countGreat = statistics[HitResult.Great];
            var countOk = statistics[HitResult.Ok];
            var countMeh = statistics[HitResult.Meh];
            var countMiss = statistics[HitResult.Miss];
            var total = countGreat + countOk + countMeh + countMiss;

            if (total == 0)
                return 1;

            return (double)(6 * countGreat + 2 * countOk + countMeh) / (6 * total);
        }

        private static PpPlusDifficulty GetDifficultyFromDifficultyAttributes(
            OsuDifficultyAttributes difficultyAttributes
        )
        {
            return new PpPlusDifficulty()
            {
                Total = difficultyAttributes.StarRating,
                Aim = difficultyAttributes.AimDifficulty,
                JumpAim = difficultyAttributes.JumpAimDifficulty,
                FlowAim = difficultyAttributes.FlowAimDifficulty,
                Precision = difficultyAttributes.PrecisionDifficulty,
                Speed = difficultyAttributes.SpeedDifficulty,
                Stamina = difficultyAttributes.StaminaDifficulty,
                Accuracy = difficultyAttributes.AccuracyDifficulty,
            };
        }

        private static PpPlusPerformance GetPerformanceFromPerformanceAttributes(
            OsuPerformanceAttributes performanceAttributes
        )
        {
            return new PpPlusPerformance()
            {
                Total = performanceAttributes.Total,
                Aim = performanceAttributes.Aim,
                JumpAim = performanceAttributes.JumpAim,
                FlowAim = performanceAttributes.FlowAim,
                Precision = performanceAttributes.Precision,
                Speed = performanceAttributes.Speed,
                Stamina = performanceAttributes.Stamina,
                Accuracy = performanceAttributes.Accuracy,
            };
        }
    }
}
