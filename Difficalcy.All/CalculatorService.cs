using Difficalcy.All.Services;
using Difficalcy.Services;
using Microsoft.Extensions.Logging;

namespace Difficalcy.All;

public class CalculationService
{
    public CalculationService(ICache cache, IBeatmapProvider beatmapProvider, string osuCommitHash)
    {
        Osu = new OsuCalculatorService(cache, beatmapProvider);
        Taiko = new TaikoCalculatorService(cache, beatmapProvider);
        Catch = new CatchCalculatorService(cache, beatmapProvider);
        Mania = new ManiaCalculatorService(cache, beatmapProvider);
        PpPlus = new PpPlusCalculatorService(cache, beatmapProvider, osuCommitHash);
    }

    public OsuCalculatorService Osu { get; }
    public TaikoCalculatorService Taiko { get; }
    public CatchCalculatorService Catch { get; }
    public ManiaCalculatorService Mania { get; }
    public PpPlusCalculatorService PpPlus { get; }
}

public class WebBeatmapProvider(
        string beatmapDirectory,
        bool downloadMissingBeatmaps,
        ILogger<WebBeatmapProvider> logger
    ) : IBeatmapProvider
{
    private readonly string _beatmapDirectory = beatmapDirectory;
    private readonly bool _downloadMissingBeatmaps = downloadMissingBeatmaps;
    private readonly HttpClient _httpClient = new();

    public async Task EnsureBeatmap(string beatmapId)
    {
        var beatmapPath = GetBeatmapPath(beatmapId);
        if (!File.Exists(beatmapPath))
        {
            if (!_downloadMissingBeatmaps)
            {
                logger.LogWarning(
                    "Beatmap {BeatmapId} not found and downloading is disabled",
                    beatmapId
                );
                throw new BeatmapNotFoundException(beatmapId);
            }

            logger.LogInformation("Downloading beatmap {BeatmapId}", beatmapId);

            using var response = await _httpClient.GetAsync(
                $"https://osu.ppy.sh/osu/{beatmapId}"
            );
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "Failed to download beatmap {BeatmapId}, status code {StatusCode}",
                    beatmapId,
                    response.StatusCode
                );
                throw new BeatmapNotFoundException(beatmapId);
            }

            if (response.Content.Headers.ContentLength == 0)
            {
                logger.LogWarning(
                    "Downloaded beatmap {BeatmapId} response was empty",
                    beatmapId
                );
                throw new BeatmapNotFoundException(beatmapId);
            }

            var dir = Path.GetDirectoryName(beatmapPath);
            if (dir != null) Directory.CreateDirectory(dir);
            using var fs = new FileStream(beatmapPath, FileMode.CreateNew);
            await response.Content.CopyToAsync(fs);
            if (fs.Length == 0)
            {
                logger.LogWarning(
                    "Downloaded beatmap {BeatmapId} was empty, deleting",
                    beatmapId
                );
                fs.Close();
                File.Delete(beatmapPath);
                throw new BeatmapNotFoundException(beatmapId);
            }
        }
    }

    public Stream GetBeatmapStream(string beatmapId)
    {
        var beatmapPath = GetBeatmapPath(beatmapId);
        return File.OpenRead(beatmapPath);
    }

    private string GetBeatmapPath(string beatmapId)
    {
        return Path.Combine(_beatmapDirectory, $"{beatmapId}.osu");
    }
}