using System.Drawing;

namespace OverlootingAutoClick.ClickExecutor;

/// <summary>
/// Интерфейс для выполнения кликов
/// </summary>
internal interface IClickExecutor
{
    /// <summary>
    /// Клик по заданным координатам в рамках заданного окна
    /// </summary>
    /// <param name="position">Координаты клика</param>
    /// <param name="hWnd">Окно</param>
    public void ClickAt(Point position, IntPtr hWnd);
}