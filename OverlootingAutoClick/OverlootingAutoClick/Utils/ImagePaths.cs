namespace OverlootingAutoClick.Utils;

internal static class ImagePaths
{
    public static readonly OrderedDictionary<ImageResource, string> Resources = new()
    {
        { ImageResource.ArrowNextLevel, "Images/ArrowNextLevel.png" },

        { ImageResource.WoodenChest, "Images/Chests/Wooden.png" },
        { ImageResource.GoldenChest, "Images/Chests/Golden.png" },
        { ImageResource.NecromancerChest, "Images/Chests/Necromancer.png" },

        { ImageResource.BlacksmithEnter, "Images/BlacksmithEnter.png" }, // 0.92
        { ImageResource.BlacksmithExit, "Images/BlacksmithExit.png" },   // А тут повысить скорее всего
        { ImageResource.Heart, "Images/Heart.png" },
        { ImageResource.WaterWellEnter, "Images/WaterWellEnter.png" },
        { ImageResource.WaterWellExit, "Images/WaterWellExit.png" },

        { ImageResource.Slime, "Images/Enemies/Slime.png" },
        { ImageResource.Rabbit, "Images/Enemies/Rabbit.png" },
        { ImageResource.ForestKiller, "Images/Enemies/ForestKiller.png" },
        { ImageResource.Snake, "Images/Enemies/Snake.png" },
        { ImageResource.ForestWarrior, "Images/Enemies/ForestWarrior.png" },
        { ImageResource.Bee, "Images/Enemies/Bee.png" },
        { ImageResource.ForestDruid, "Images/Enemies/ForestDruid.png" },
        { ImageResource.ForestBully, "Images/Enemies/ForestBully.png" },
        { ImageResource.Amalgam, "Images/Enemies/Amalgam.png" },
        { ImageResource.Cobra, "Images/Enemies/Cobra.png" },
        { ImageResource.Shark, "Images/Enemies/Shark.png" },

        { ImageResource.SlimeMother, "Images/Bosses/SlimeMother.png" },
        { ImageResource.FilthProphet, "Images/Bosses/FilthProphet.png" }
    };

    public static string GetPath(ImageResource resource)
    {
        if (Resources.TryGetValue(resource, out var path))
        {
            return path;
        }

        throw new ArgumentException($"Путь для {resource} не найден");
    }
}

internal enum ImageResource
{
    ArrowNextLevel,

    WoodenChest,
    GoldenChest,
    NecromancerChest,

    BlacksmithEnter,
    BlacksmithExit,
    Heart,
    WaterWellEnter,
    WaterWellExit,

    Slime,
    Rabbit,
    ForestKiller,
    Snake,
    ForestWarrior,
    Bee,
    ForestDruid,
    ForestBully,
    Amalgam,
    Cobra,
    Shark,

    SlimeMother,
    FilthProphet
}