using System.Drawing;

namespace OverlootingAutoClick.WindowCapture;

internal interface IWindowCapture
{
    /// <summary>
    /// Захват изображения экрана
    /// </summary>
    /// <param name="hWnd">Окно</param>
    /// <returns>Изображение экрана</returns>
    public Bitmap? CaptureWindow(IntPtr hWnd);
}