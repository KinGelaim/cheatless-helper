using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick.WindowCapture;

internal sealed class WindowsApiCapture : IWindowCapture
{
    [DllImport("user32.dll")]
    private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcB, uint nFlags);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public Bitmap? CaptureWindow(IntPtr hWnd)
    {
        // Получить размер окна
        if (!GetWindowRect(hWnd, out RECT rect))
        {
            return null;
        }

        var width = rect.Right - rect.Left;
        var height = rect.Bottom - rect.Top;

        var bmp = new Bitmap(width, height);
        using (var graphics = Graphics.FromImage(bmp))
        {
            var hdc = graphics.GetHdc();
            var success = PrintWindow(hWnd, hdc, 0);
            graphics.ReleaseHdc(hdc);

            if (!success)
            {
                bmp.Dispose();
                return null;
            }
        }
        return bmp;
    }
}