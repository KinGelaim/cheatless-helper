using OverlootingAutoClick.Resources;
using System.Drawing;

namespace OverlootingAutoClick.ElementFinder;

/// <summary>
/// Интерфейс для поиска элемента (ресурса)
/// </summary>
internal interface IElementFinder
{
    /// <summary>
    /// Возвращает координаты центра найденного элемента
    /// </summary>
    /// <param name="resource">Информация о ресурсе, который ищем</param>
    /// <returns>Координаты центра найденного элемента или null, если элемент не найден</returns>
    public Point? FindElementBitmap(ResourceInfo resource);
}