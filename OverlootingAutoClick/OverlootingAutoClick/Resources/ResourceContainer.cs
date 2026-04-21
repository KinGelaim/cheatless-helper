using Emgu.CV;
using Emgu.CV.CvEnum;

namespace OverlootingAutoClick.Resources;

internal sealed class ResourceContainer : IDisposable
{
    private readonly ResourceInfo[] _resources =
    [
        new(ResourceType.ArrowNextLevel, "Стрелка", "Images/ArrowNextLevel.png"),

        new(ResourceType.WoodenChest, "Деревянный сундук", "Images/Chests/Wooden.png"),
        new(ResourceType.GoldenChest, "Золотой сундук", "Images/Chests/Golden.png"),
        new(ResourceType.NecromancerChest, "Некроманский сундук", "Images/Chests/Necromancer.png"),

        new(ResourceType.BlacksmithEnter, "Кузнец (вход)", "Images/BlacksmithEnter.png"),
        new(ResourceType.BlacksmithExit, "Кузнец (выход)", "Images/BlacksmithExit.png"),
        new(ResourceType.Heart, "Сердце (святилище)", "Images/Heart.png"),
        new(ResourceType.WaterWellEnter, "Колодец (вход)", "Images/WaterWellEnter.png"),
        new(ResourceType.WaterWellExit, "Колодец (выход)", "Images/WaterWellExit.png"),
        new(ResourceType.MannequinEnter, "Маникен (вход)", "Images/MannequinEnter.png"),
        new(ResourceType.MannequinExit, "Маникен (выход)", "Images/MannequinExit.png"),
        new(ResourceType.HeroRemains, "Остатки героя", "Images/HeroRemains.png"),

        new(ResourceType.Slime, "Слизь", "Images/Enemies/Slime.png"),
        new(ResourceType.Rabbit, "Кролик", "Images/Enemies/Rabbit.png"),
        new(ResourceType.ForestKiller, "Лесной убийца", "Images/Enemies/ForestKiller.png"),
        new(ResourceType.Snake, "Змея", "Images/Enemies/Snake.png"),
        new(ResourceType.ForestWarrior, "Лесной воин", "Images/Enemies/ForestWarrior.png"),
        new(ResourceType.Bee, "Пчела", "Images/Enemies/Bee.png"),
        new(ResourceType.ForestDruid, "Лесной друид", "Images/Enemies/ForestDruid.png"),
        new(ResourceType.ForestBully, "Лесной громила", "Images/Enemies/ForestBully.png"),
        new(ResourceType.Amalgam, "Амальгама", "Images/Enemies/Amalgam.png"),
        new(ResourceType.Cobra, "Кобра", "Images/Enemies/Cobra.png"),
        new(ResourceType.Shark, "Волк-акула", "Images/Enemies/Shark.png"),

        new(ResourceType.DogTrapVenus, "Венерена собаколовка", "Images/Enemies/DogTrapVenus.png"),
        new(ResourceType.StrangeRaven, "Странный ворон", "Images/Enemies/StrangeRaven.png"),
        new(ResourceType.Mandragora, "Мандрагора", "Images/Enemies/Mandragora.png"),
        new(ResourceType.EvilMandragora, "Злая мандрагора", "Images/Enemies/EvilMandragora.png"),
        new(ResourceType.CreepingMandragora, "Ползучая мандрагора", "Images/Enemies/CreepingMandragora.png"),
        new(ResourceType.StrangeReptile, "Странная рептилия", "Images/Enemies/StrangeReptile.png"),
        new(ResourceType.Mushroom, "Грибочек", "Images/Enemies/Mushroom.png"),
        new(ResourceType.MushroomFamily, "Семья грибочков", "Images/Enemies/MushroomFamily.png"),
        new(ResourceType.WolfTrapVenus, "Венерена волколовка", "Images/Enemies/WolfTrapVenus.png"),
        new(ResourceType.StrangeRhino, "Странный носорог", "Images/Enemies/StrangeRhino.png"),
        new(ResourceType.SaurianMandragora, "Сауриан мандрагора", "Images/Enemies/SaurianMandragora.png"),
        new(ResourceType.VargoTrapVenus, "Венерена варголовка", "Images/Enemies/VargoTrapVenus.png"),

        new(ResourceType.CoreFragment, "Фрагмент ядра", "Images/Enemies/CoreFragment.png"),
        new(ResourceType.CoreBeast, "Зверь ядра", "Images/Enemies/CoreBeast.png"),
        new(ResourceType.DefiledBerserker, "Осквернённый берсерк", "Images/Enemies/DefiledBerserker.png"),
        new(ResourceType.DefiledPriest, "Осквернённый священник", "Images/Enemies/DefiledPriest.png"),
        new(ResourceType.DefiledGuardian, "Осквернённый страж", "Images/Enemies/DefiledGuardian.png"),
        new(ResourceType.CoreHomunculus, "Гомункул ядра", "Images/Enemies/CoreHomunculus.png"),
        new(ResourceType.DefiledHermit, "Осквернённый отшельник", "Images/Enemies/DefiledHermit.png"),
        new(ResourceType.CoreEnchanter, "Чародей ядра", "Images/Enemies/CoreEnchanter.png"),
        new(ResourceType.CoreDefender, "Защитник ядра", "Images/Enemies/CoreDefender.png"),
        new(ResourceType.CoreSpear, "Копьё ядра", "Images/Enemies/CoreSpear.png"),
        new(ResourceType.CoreSphere, "Сфера ядра", "Images/Enemies/CoreSphere.png"),

        new(ResourceType.SlimeMother, "Мать слизней", "Images/Bosses/SlimeMother.png"),
        new(ResourceType.RedDragon, "Красный дракон", "Images/Bosses/RedDragon.png"),
        new(ResourceType.GreenDragon, "Зелёный дракон", "Images/Bosses/GreenDragon.png"),
        new(ResourceType.RottenCoreMessenger, "Гнилой посланник ядра", "Images/Bosses/RottenCoreMessenger.png"),
        new(ResourceType.WoundedCoreMessenger, "Раненый посланник ядра", "Images/Bosses/WoundedCoreMessenger.png"),
        new(ResourceType.FilthProphet, "Мерзкий пророк", "Images/Bosses/FilthProphet.png")
    ];

    public IEnumerable<ResourceInfo> GetResources()
    {
        foreach (var resource in _resources)
        {
            resource.ImageMat ??= CvInvoke.Imread(resource.Path, ImreadModes.Unchanged);

            yield return resource;
        }
    }

    public void Dispose()
    {
        foreach (var resource in _resources)
        {
            resource.Dispose();
        }
    }
}