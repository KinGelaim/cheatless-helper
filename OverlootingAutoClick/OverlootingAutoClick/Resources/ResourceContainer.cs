using Emgu.CV;
using Emgu.CV.CvEnum;

namespace OverlootingAutoClick.Resources;

/// <summary>
/// Класс-хранилище для информации о ресурсах
/// </summary>
internal sealed class ResourceContainer : IDisposable
{
    private readonly ResourceInfo[] _resources =
    [
        new("Стрелка", "Images/ArrowNextLevel.png"),

        new("Деревянный сундук", "Images/Chests/Wooden.png"),
        new("Золотой сундук", "Images/Chests/Golden.png"),
        new("Некроманский сундук", "Images/Chests/Necromancer.png"),

        new("Кузнец (вход) 1", "Images/BlacksmithEnterFirst.png", 0.90),
        new("Кузнец (вход) 2", "Images/BlacksmithEnterSecond.png", 0.90),
        new("Кузнец (вход) 3", "Images/BlacksmithEnterThird.png", 0.90),
        new("Кузнец (выход)", "Images/BlacksmithExit.png"),
        new("Сердце (святилище)", "Images/Heart.png"),
        new("Колодец (вход)", "Images/WaterWellEnter.png"),
        new("Колодец (выход)", "Images/WaterWellExit.png"),
        new("Маникен (вход)", "Images/MannequinEnter.png", 0.98),
        new("Маникен (выход)", "Images/MannequinExit.png", 0.97),
        new("Перекрёсток (вход)", "Images/CrossroadEnter.png"),
        new("Перекрёсток (выход)", "Images/CrossroadExit.png"),
        new("Остатки героя", "Images/HeroRemains.png"),
        new("Конец уровня", "Images/LevelEnd.png", 0.97),

        new("Слизь", "Images/Enemies/Slime.png"),
        new("Кролик", "Images/Enemies/Rabbit.png"),
        new("Лесной убийца", "Images/Enemies/ForestKiller.png"),
        new("Змея", "Images/Enemies/Snake.png"),
        new("Лесной воин", "Images/Enemies/ForestWarrior.png"),
        new("Пчела", "Images/Enemies/Bee.png"),
        new("Лесной друид", "Images/Enemies/ForestDruid.png"),
        new("Лесной громила", "Images/Enemies/ForestBully.png"),
        new("Амальгама", "Images/Enemies/Amalgam.png"),
        new("Кобра", "Images/Enemies/Cobra.png"),
        new("Волк-акула", "Images/Enemies/Shark.png"),

        new("Венерена собаколовка", "Images/Enemies/DogTrapVenus.png"),
        new("Странный ворон", "Images/Enemies/StrangeRaven.png"),
        new("Мандрагора", "Images/Enemies/Mandragora.png"),
        new("Злая мандрагора", "Images/Enemies/EvilMandragora.png"),
        new("Ползучая мандрагора", "Images/Enemies/CreepingMandragora.png"),
        new("Странная рептилия", "Images/Enemies/StrangeReptile.png"),
        new("Грибочек", "Images/Enemies/Mushroom.png"),
        new("Семья грибочков", "Images/Enemies/MushroomFamily.png"),
        new("Венерена волколовка", "Images/Enemies/WolfTrapVenus.png"),
        new("Странный носорог", "Images/Enemies/StrangeRhino.png"),
        new("Саурианская мандрагора", "Images/Enemies/SaurianMandragora.png"),
        new("Венерена варголовка", "Images/Enemies/VargoTrapVenus.png"),

        new("Фрагмент ядра", "Images/Enemies/CoreFragment.png"),
        new("Зверь ядра", "Images/Enemies/CoreBeast.png"),
        new("Осквернённый берсерк", "Images/Enemies/DefiledBerserker.png"),
        new("Осквернённый священник", "Images/Enemies/DefiledPriest.png"),
        new("Осквернённый страж", "Images/Enemies/DefiledGuardian.png"),
        new("Гомункул ядра", "Images/Enemies/CoreHomunculus.png"),
        new("Осквернённый отшельник", "Images/Enemies/DefiledHermit.png"),
        new("Чародей ядра", "Images/Enemies/CoreEnchanter.png"),
        new("Защитник ядра", "Images/Enemies/CoreDefender.png"),
        new("Копьё ядра", "Images/Enemies/CoreSpear.png"),
        new("Сфера ядра", "Images/Enemies/CoreSphere.png"),

        new("Мать слизней", "Images/Bosses/SlimeMother.png"),
        new("Химера", "Images/Bosses/Chimera.png"),
        new("Мантикора", "Images/Bosses/Manticore.png"),
        new("Красный дракон", "Images/Bosses/RedDragon.png"),
        new("Зелёный дракон", "Images/Bosses/GreenDragon.png"),
        new("Владыка леса", "Images/Bosses/ForestLord.png"),
        new("Саженец", "Images/Bosses/Seedling.png"),
        new("Венерина гигантоловка", "Images/Bosses/GiantTrapVenus.png"),
        new("Венерина троллеловка", "Images/Bosses/TrollTrapVenus.png"),
        new("Венерина драконоловка", "Images/Bosses/DragonTrapVenus.png"),
        new("Осквернённый цветок", "Images/Bosses/DefiledFlower.png"),
        new("Семя скверны", "Images/Bosses/DefilementSeed.png"),
        new("Гнилой посланник ядра", "Images/Bosses/RottenCoreMessenger.png"),
        new("Раненый посланник ядра", "Images/Bosses/WoundedCoreMessenger.png"),
        new("Мерзкий пророк", "Images/Bosses/FilthProphet.png"),
        new("Аватар скверны", "Images/Bosses/CorruptionAvatar.png"),
        new("Величие", "Images/Bosses/Greatness.png")
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