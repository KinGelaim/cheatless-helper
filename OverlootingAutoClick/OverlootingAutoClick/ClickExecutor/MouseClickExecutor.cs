using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick.ClickExecutor;

/// <summary>
/// Клик мышью через смещение положения курсора и эмуляцию клика
/// </summary>
internal sealed class MouseClickExecutor : ClickExecutorBase
{
    // WinAPI для перемещения мыши
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    // WinAPI для клика мышью
    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    /// <inheritdoc/>
    public override void ClickAt(Point position, IntPtr hWnd)
    {
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
}