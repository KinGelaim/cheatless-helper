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
        { ImageResource.Mannequin, "Images/Mannequin.png" },

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

        { ImageResource.DogTrapVenus, "Images/Enemies/DogTrapVenus.png" },
        { ImageResource.StrangeRaven, "Images/Enemies/StrangeRaven.png" },
        { ImageResource.Mandragora, "Images/Enemies/Mandragora.png" },
        { ImageResource.EvilMandragora, "Images/Enemies/EvilMandragora.png" },
        { ImageResource.CreepingMandragora, "Images/Enemies/CreepingMandragora.png" },
        { ImageResource.StrangeReptile, "Images/Enemies/StrangeReptile.png" },
        { ImageResource.Mushroom, "Images/Enemies/Mushroom.png" },
        { ImageResource.MushroomFamily, "Images/Enemies/MushroomFamily.png" },
        { ImageResource.WolfTrapVenus, "Images/Enemies/WolfTrapVenus.png" },
        { ImageResource.WolfTrapVenus, "Images/Enemies/StrangeRhino.png" },
        { ImageResource.SaurianMandragora, "Images/Enemies/SaurianMandragora.png" },
        { ImageResource.VargoTrapVenus, "Images/Enemies/VargoTrapVenus.png" },

        { ImageResource.SlimeMother, "Images/Bosses/SlimeMother.png" },
        { ImageResource.RedDragon, "Images/Bosses/RedDragon.png" },
        { ImageResource.GreenDragon, "Images/Bosses/GreenDragon.png" },
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
    Mannequin,

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

    DogTrapVenus,
    StrangeRaven,
    Mandragora,
    EvilMandragora,
    CreepingMandragora,
    StrangeReptile,
    Mushroom,
    MushroomFamily,
    WolfTrapVenus,
    StrangeRhino,
    SaurianMandragora,
    VargoTrapVenus,

    SlimeMother,
    RedDragon,
    GreenDragon,
    FilthProphet
}