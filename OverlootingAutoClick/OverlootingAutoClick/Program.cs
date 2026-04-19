using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick;

public sealed class Program
{
    // Импортируем необходимые функции WinAPI
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    public static void Main()
    {
        // Параметры поиска
        string templatePath = "Images/ArrowNextLevel.png"; // путь к изображению шаблона
        double threshold = 0.8; // порог совпадения

        // Координаты и размеры монитора (заполняйте актуальными данными)
        Rectangle monitorBounds = new Rectangle(0, 0, 1920, 1080);

        // Захват изображения монитора
        Bitmap bmp = new Bitmap(monitorBounds.Width, monitorBounds.Height);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(monitorBounds.Location, Point.Empty, monitorBounds.Size);
        }

        // Сохраняем скриншот для обработки
        string screenshotPath = "temp_screenshot.png";
        bmp.Save(screenshotPath);

        // Загружаем изображение скриншота и шаблон
        using (Mat source = CvInvoke.Imread(screenshotPath))
        using (Mat template = CvInvoke.Imread(templatePath))
        {
            using (Mat result = new Mat())
            {
                // Выполняем сопоставление шаблонов
                CvInvoke.MatchTemplate(source, template, result, TemplateMatchingType.CcorrNormed);

                // Ищем максимум
                double minVal = 0, maxVal = 0;
                Point minLoc = new Point(), maxLoc = new Point();
                CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                Console.WriteLine($"Сравнение: максимум = {maxVal}");

                if (maxVal > threshold)
                {
                    // Находим центро шаблона (для клика)
                    int clickX = maxLoc.X + template.Width / 2 + monitorBounds.X;
                    int clickY = maxLoc.Y + template.Height / 2 + monitorBounds.Y;

                    Console.WriteLine($"Обнаружена стрелка. Кликаю по ({clickX}, {clickY})...");

                    // Перемещаем курсор и кликаем
                    SetCursorPos(clickX, clickY);
                    Thread.Sleep(100);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                    Thread.Sleep(50);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                }
                else
                {
                    Console.WriteLine("Шаблон не найден или совпадение недостаточное.");
                }
            }
        }
    }
}