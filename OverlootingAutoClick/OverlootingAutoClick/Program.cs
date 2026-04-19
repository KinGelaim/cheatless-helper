using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick;

public sealed class Program
{
    // WinAPI функции для захвата окна
    [DllImport("user32.dll")]
    private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcB, uint nFlags);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int maxLength);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

    // Выделение делегата для EnumWindows
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    // WinAPI для перемещения мыши
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    // WinAPI для клика мышью
    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    // WinAPI для установки окна на передний план
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    // WinAPI для отправки сообщения в окно
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    private const uint WM_LBUTTONDOWN = 0x0201;
    private const uint WM_LBUTTONUP = 0x0202;

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    public static void Main()
    {
        // Название окна, который хотим захватить
        //var windowTitlePartial = "Overlooting";
        var windowTitle = "Overlooting";

        // Найти окно по части заголовка
        //var hWnd = FindWindowByPartialTitle(windowTitlePartial);
        var hWnd = FindWindowByExactTitle(windowTitle);
        if (hWnd == IntPtr.Zero)
        {
            Console.WriteLine($"Окно с заголовком '{windowTitle}' не найдено");
            return;
        }
        Console.WriteLine($"Окно найдено, HWND: {hWnd}");

        // Выполнить активизацию окна
        // SetForegroundWindow(hWnd);

        // Захват изображения окна в фоне
        var bmp = CaptureWindow(hWnd);
        if (bmp is not null)
        {
            bmp.Save("temp_background_screenshot.png");
            Console.WriteLine("Изображение окна в фоне сохранено как 'temp_background_screenshot.png'");
            bmp.Dispose();
        }
        else
        {
            Console.WriteLine("Не удалось захватить изображение окна");
            return;
        }

        // Теперь можем продолжить обработку, например, поиск шаблона в изображении
        var templatePath = "Images/ArrowNextLevel.png";
        var threshold = 0.9;

        // Обработка изображения
        using Mat source = CvInvoke.Imread("temp_background_screenshot.png");
        using Mat template = CvInvoke.Imread(templatePath);
        using Mat result = new();
        CvInvoke.MatchTemplate(source, template, result, TemplateMatchingType.CcorrNormed);

        double minVal = 0, maxVal = 0;
        Point minLoc = new(), maxLoc = new();
        CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

        Console.WriteLine($"Максимальное совпадение: {maxVal}");

        if (maxVal > threshold)
        {
            var clickX = maxLoc.X + template.Width / 2;
            var clickY = maxLoc.Y + template.Height / 2;

            Console.WriteLine($"Обнаружена стрелка. Отправляю клик по ({clickX}, {clickY})...");
            //SetCursorPos(clickX, clickY);
            Thread.Sleep(100);
            //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            SendMessage(hWnd, WM_LBUTTONDOWN, (IntPtr)1, MakeLParam(clickX, clickY));
            Thread.Sleep(50);
            //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
            SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, MakeLParam(clickX, clickY));
        }
        else
        {
            Console.WriteLine("Шаблон не найден или совпадение слишком низкое");
        }
    }

    // Метод поиска окна по части заголовка
    private static IntPtr FindWindowByPartialTitle(string titlePart)
    {
        IntPtr foundHWnd = IntPtr.Zero;

        bool EnumProc(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                const int nChars = 256;
                var Buff = new System.Text.StringBuilder(nChars);
                GetWindowText(hWnd, Buff, nChars);
                var windowTitle = Buff.ToString();

                if (!string.IsNullOrEmpty(windowTitle) && windowTitle.Contains(titlePart))
                {
                    foundHWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

        EnumWindows(EnumProc, IntPtr.Zero);
        return foundHWnd;
    }

    // Метод поиска окна по полному совпадению
    static IntPtr FindWindowByExactTitle(string exactTitle)
    {
        IntPtr foundHWnd = IntPtr.Zero;

        bool EnumProc(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                const int nChars = 256;
                var Buff = new System.Text.StringBuilder(nChars);
                GetWindowText(hWnd, Buff, nChars);
                var windowTitle = Buff.ToString();

                if (!string.IsNullOrEmpty(windowTitle) && windowTitle == exactTitle)
                {
                    foundHWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

        EnumWindows(EnumProc, IntPtr.Zero);
        return foundHWnd;
    }

    // Метод захвата окна без его активизации
    private static Bitmap? CaptureWindow(IntPtr hWnd)
    {
        try
        {
            if (!GetWindowRect(hWnd, out RECT rect))
            {
                Console.WriteLine("Не удалось получить размеры окна");
                return null;
            }

            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;

            var bmp = new Bitmap(width, height);
            var gfx = Graphics.FromImage(bmp);
            var hDC = gfx.GetHdc();

            var success = PrintWindow(hWnd, hDC, 0);
            gfx.ReleaseHdc(hDC);
            gfx.Dispose();

            if (!success)
            {
                Console.WriteLine("PrintWindow() не удалось");
                bmp.Dispose();
                return null;
            }

            return bmp;
        }
        catch
        {
            return null;
        }
    }

    public static IntPtr MakeLParam(int x, int y)
    {
        return (IntPtr)((y << 16) | (x & 0xFFFF));
    }

    // Структура RECT
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}