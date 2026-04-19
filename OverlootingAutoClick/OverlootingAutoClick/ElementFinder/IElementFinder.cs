using System.Drawing;

namespace OverlootingAutoClick.ElementFinder;

internal interface IElementFinder
{
    /// <summary>
    /// Возвращает координаты центра найденного элемента
    /// </summary>
    /// <param name="windowImagePath">Путь к изображению в котором будем искать</param>
    /// <returns>Координаты центра найденного элемента или null, если элемент не найден</returns>
    public Point? FindElementBitmap(string windowImagePath);
}