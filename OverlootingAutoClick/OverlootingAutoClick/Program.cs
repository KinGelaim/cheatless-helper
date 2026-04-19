using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick;

public sealed class Program
{
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    public static void Main()
    {
        // Настройки: координаты точки для проверки и клика
        var checkX = 1350;
        var checkY = 1000;
        var targetColor = Color.FromArgb(255, 0, 0);

        // Получение информации о мониторе
        var monitorBounds = new Rectangle(0, 0, 1920, 1080);

        // Захват изображения с монитора
        var bmp = new Bitmap(monitorBounds.Width, monitorBounds.Height);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(monitorBounds.Location, Point.Empty, monitorBounds.Size);
        }

        // Проверка пикселя
        Color pixelColor = bmp.GetPixel(checkX - monitorBounds.X, checkY - monitorBounds.Y);

        Console.WriteLine($"Пиксель по координате ({checkX},{checkY}): {pixelColor}");

        if (pixelColor.R == targetColor.R && pixelColor.G == targetColor.G && pixelColor.B == targetColor.B)
        {
            Console.WriteLine("Цвет совпал! Выполняю клик...");

            // Перемещаем курсор
            SetCursorPos(checkX, checkY);

            // Эмулируем клик
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(50); // Мелкая задержка
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
            Console.WriteLine("Клик выполнен");
        }
        else
        {
            SetCursorPos(checkX, checkY);
            Console.WriteLine("Цвет не совпал, клик не выполняется");
        }
    }
}