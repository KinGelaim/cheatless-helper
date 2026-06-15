using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Tesseract;

namespace TimberRushAutoClick;

internal sealed partial class Program
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

        var stopwatch = new Stopwatch();
        while (!_shouldStop)
        {
            stopwatch.Restart();
            ScanResources();
            stopwatch.Stop();
            Console.WriteLine($"Время сканирования ресурсов: {stopwatch.ElapsedMilliseconds}");

            Thread.Sleep(60_000);
        }

        Console.WriteLine("Программа завершена");
    }

    private static void ScanResources()
    {
        // Найти окно
        var hWnd = FindWindow("Timber Rush");
        if (hWnd == IntPtr.Zero)
        {
            Console.WriteLine("Окно не найдено");
            return;
        }

        // Захватить изображение
        using var windowBmp = CaptureWindow(hWnd);
        if (windowBmp is null)
        {
            Console.WriteLine("Не удалось захватить изображение");
            return;
        }

        // Преобразование изображения
        using Bitmap binarizedWindowBmp = BinarizeBitmap(windowBmp);

        // Распознание текста
        var targetText = "овый";
        var results = RecognizeTextAndBoundingBoxes(
            binarizedWindowBmp,
            targetText);

        foreach (var result in results)
        {
            Console.WriteLine($"Текст '{targetText}' найден в области: {result.BoundingBox}");

            var clickX = result.BoundingBox.Left + result.BoundingBox.Width / 2;
            var clickY = result.BoundingBox.Top + result.BoundingBox.Height / 2;
            var point = new Point(clickX, clickY);
            ClickAt(point, hWnd);
            Thread.SpinWait(100);

            Console.WriteLine("Произведён клик по найденному элементу.");
        }
    }


    #region FindWindow

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int maxLength);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    private static IntPtr FindWindow(string title)
    {
        var foundHWnd = IntPtr.Zero;

        bool Callback(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                const int nChars = 256;
                var buffer = new System.Text.StringBuilder(nChars);
                _ = GetWindowText(hWnd, buffer, nChars);
                if (buffer.ToString() == title)
                {
                    foundHWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

        EnumWindows(Callback, IntPtr.Zero);
        return foundHWnd;
    }

    #endregion FindWindow


    #region CaptureWindow

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetWindowRect(IntPtr hWnd, out RECT rect);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    // WinAPI для получения окна на переднем плане
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    // WinAPI для установки окна на передний план
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(IntPtr hWnd);

    private static Bitmap? CaptureWindow(IntPtr hWnd)
    {
        // Получить размер окна
        if (!GetWindowRect(hWnd, out RECT rect))
        {
            return null;
        }

        var width = rect.Right - rect.Left;
        var height = rect.Bottom - rect.Top;

        // Сделать нужное окно активным
        SetForegroundWindow(hWnd);
        Thread.SpinWait(1000);
        Console.WriteLine("Окно на переднем плане");

        // Захватываем весь экран
        var screenShot = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(screenShot))
        {
            g.CopyFromScreen(0, 0, 0, 0, new Size(width, height));
        }

        // Вырезаем нужный прямоугольник
        var windowRect = new Rectangle(rect.Left, rect.Top, width, height);
        var windowImage = screenShot.Clone(windowRect, screenShot.PixelFormat);

        return windowImage;
    }

    #endregion CaptureWindow


    #region BinarizeImage

    private static Bitmap BinarizeBitmap(Bitmap originalBitmap)
    {
        // Создаем новое изображение такой же размерности
        Bitmap binaryBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);

        int threshold = 128; // уровень порога

        for (int y = 0; y < originalBitmap.Height; y++)
        {
            for (int x = 0; x < originalBitmap.Width; x++)
            {
                Color color = originalBitmap.GetPixel(x, y);
                int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);

                if (gray < threshold)
                {
                    binaryBitmap.SetPixel(x, y, Color.Black);
                }
                else
                {
                    binaryBitmap.SetPixel(x, y, Color.White);
                }
            }
        }
        return binaryBitmap;
    }

    #endregion BinarizeImage


    #region RecognizeText

    private class TextBoundingBox
    {
        public string Text { get; set; }
        public Rectangle BoundingBox { get; set; }
    }

    private static List<TextBoundingBox> RecognizeTextAndBoundingBoxes(Bitmap img, string searchText)
    {
        var result = new List<TextBoundingBox>();
        var tessDataPath = @"./tessdata";

        using var engine = new TesseractEngine(tessDataPath, "rus", EngineMode.Default);
        using var pix = PixConverter.ToPix(img);
        using var page = engine.Process(pix);

        // Итератор для прохождения по результатам
        var iterator = page.GetIterator();
        iterator.Begin();

        do
        {
            // Получение слова и области вокруг
            var text = iterator.GetText(PageIteratorLevel.TextLine);
            var normalizedText = text.Replace(" ", "");
            if (!string.IsNullOrEmpty(normalizedText) && normalizedText.Contains(searchText))
            {
                // Получение bounding box слова
                if (iterator.TryGetBoundingBox(PageIteratorLevel.TextLine, out Rect rect))
                {
                    result.Add(new TextBoundingBox
                    {
                        Text = text,
                        BoundingBox = new Rectangle(
                            rect.X1,
                            rect.Y1,
                            rect.X2 - rect.X1,
                            rect.Y2 - rect.Y1)
                    });
                }
            }
        } while (iterator.Next(PageIteratorLevel.TextLine));

        return result;
    }

    #endregion RecognizeText


    #region ClickExecutor

    // WinAPI для отправки сообщения в окно
    [LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
    private static partial IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    private const uint WM_LBUTTONDOWN = 0x0201;
    private const uint WM_LBUTTONUP = 0x0202;



    // WinAPI для перемещения мыши
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetCursorPos(int X, int Y);

    // WinAPI для клика мышью
    [LibraryImport("user32.dll")]
    private static partial void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    private static void ClickAt(Point position, IntPtr hWnd)
    {
        //Console.WriteLine($"Отправляю клик по ({position.X}, {position.Y})...");
        //SendMessage(hWnd, WM_LBUTTONDOWN, (IntPtr)1, MakeLParam(position.X, position.Y));
        //Thread.SpinWait(50);
        //SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, MakeLParam(position.X, position.Y));

        // Перед кликом необходимо установить окно в активное или фокус
        SetForegroundWindow(hWnd);

        Console.WriteLine($"Перемещаю курсор на ({position.X}, {position.Y})...");
        SetCursorPos(position.X, position.Y);
        Thread.Sleep(100);

        Console.WriteLine($"Кликаю ({position.X}, {position.Y})...");
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
        Thread.Sleep(50);

        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
    }

    private static IntPtr MakeLParam(int x, int y)
    {
        return (IntPtr)((y << 16) | (x & 0xFFFF));
    }

    #endregion ClickExecutor
}