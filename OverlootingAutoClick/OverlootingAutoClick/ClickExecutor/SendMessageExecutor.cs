using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick.ClickExecutor;

internal sealed class SendMessageExecutor : ClickExecutorBase
{
    // WinAPI для отправки сообщения в окно
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    private const uint WM_LBUTTONDOWN = 0x0201;
    private const uint WM_LBUTTONUP = 0x0202;

    /// <inheritdoc/>
    public override void ClickAt(Point position, IntPtr hWnd)
    {
        // Перед отправкой клика желательно установить окно в активное или фокус
        //SetForegroundWindow(hWnd);

        Console.WriteLine($"Отправляю клик по ({position.X}, {position.Y})...");
        SendMessage(hWnd, WM_LBUTTONDOWN, (IntPtr)1, MakeLParam(position.X, position.Y));
        Thread.Sleep(50);
        SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, MakeLParam(position.X, position.Y));
    }

    private static IntPtr MakeLParam(int x, int y)
    {
        return (IntPtr)((y << 16) | (x & 0xFFFF));
    }
}