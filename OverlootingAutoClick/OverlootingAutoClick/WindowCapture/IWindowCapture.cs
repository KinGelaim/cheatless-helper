using System.Drawing;

namespace OverlootingAutoClick.WindowCapture;

/// <summary>
/// Интерфейс для захвата изображения окна
/// </summary>
internal interface IWindowCapture
{
    /// <summary>
    /// Захват изображения экрана
    /// </summary>
    /// <param name="hWnd">Окно</param>
    /// <returns>Изображение экрана</returns>
    public Bitmap? CaptureWindow(IntPtr hWnd);
}