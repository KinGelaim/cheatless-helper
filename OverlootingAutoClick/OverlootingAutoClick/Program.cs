using OverlootingAutoClick.ClickExecutor;
using OverlootingAutoClick.ElementFinder;
using OverlootingAutoClick.Resources;
using OverlootingAutoClick.Utils;
using OverlootingAutoClick.WindowCapture;
using OverlootingAutoClick.WindowFinder;

namespace OverlootingAutoClick;

public sealed class Program
{
    private static volatile bool _shouldStop = false;

    public static void Main()
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            _shouldStop = true;
            e.Cancel = true;
            Console.WriteLine("Получена команда на завершение. Дожидаемся завершения цикла...");
        };

        Console.WriteLine("Для завершения нажмите Ctrl+C");

        var resourceContainer = new ResourceContainer();
        while (!_shouldStop)
        {
            ScanResources(resourceContainer);

            Thread.Sleep(1000);
        }

        Console.WriteLine("Программа завершена");
    }

    private static void ScanResources(ResourceContainer resourceContainer)
    {
        // Найти окно
        var windowFinder = new ExactTitleWindowFinder("Overlooting");
        var hWnd = windowFinder.FindWindow();
        if (hWnd == IntPtr.Zero)
        {
            Console.WriteLine("Окно не найдено");
            return;
        }

        // Захватить изображение
        var capturer = new WindowsApiCapture();
        using var bmp = capturer.CaptureWindow(hWnd);
        if (bmp is null)
        {
            Console.WriteLine("Не удалось захватить изображение");
            return;
        }

        // Сохранить изображение
        var imageStorage = new ImageStorage();
        var filePath = imageStorage.SaveToTempFile(bmp);
        Console.WriteLine($"Изображение окна в фоне сохранено как '{filePath}'");
        bmp.Dispose();

        // Поиск элементов по приоритетам
        using var finder = new TemplateMatchingFinder(filePath);
        foreach (var resource in resourceContainer.GetResources())
        {
            var elementPos = finder.FindElementBitmap(resource);

            if (elementPos.HasValue)
            {
                var clicker = new SendMessageExecutor();
                clicker.ClickAt(elementPos.Value, hWnd);
            }
        }
        finder.Dispose();

        // В конце избавляемся от изображения экрана
        imageStorage.DeleteFile(filePath);
    }
}