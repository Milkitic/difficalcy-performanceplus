using System.Reflection;
using Difficalcy.All.Models.Osu;
using Difficalcy.All.Models.PpPlus;
using Difficalcy.Models;
using Difficalcy.Services;
using Microsoft.Extensions.Logging;

namespace Difficalcy.All.ConsoleTest;

internal class Program
{
    static async Task Main(string[] args)
    {
        var factory = LoggerFactory.Create(builder => builder.AddSimpleConsole());
        var cache = new InMemoryCache();
        var beatmapProvider = new WebBeatmapProvider("Songs", true, factory.CreateLogger<WebBeatmapProvider>());
        //var beatmapProvider = new TestBeatmapProvider(Assembly.GetExecutingAssembly().GetName().Name);
        var service = new CalculationService(cache, beatmapProvider, "test");
        var result2 = await service.Osu.GetCalculation(new OsuScore()
        {
            BeatmapId = "951053",
            //BeatmapId = "diffcalc-test",
            //Combo = 3448,
            //Mehs = 0,
            //Oks = 0,
            //Misses = 0,
            Mods = [ new Mod()
            {
                Acronym = "HR"
            }, new Mod()
            {
                Acronym = "CL"
            }]
        }); 
        
        var result = await service.PpPlus.GetCalculation(new PpPlusScore()
        {
            BeatmapId = "951053",
            //BeatmapId = "diffcalc-test",
            //Combo = 3448,
            //Mehs = 0,
            //Oks = 0,
            //Misses = 0,
            Mods = [ new Mod()
            {
                Acronym = "HR"
            }, new Mod()
            {
                Acronym = "CL"
            }]
        });

        Console.Read();
    }
}