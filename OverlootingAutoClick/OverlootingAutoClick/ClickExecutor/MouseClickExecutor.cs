using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick.ClickExecutor;

/// <summary>
/// Клик мышью через смещение положения курсора и эмуляцию клика
/// </summary>
internal sealed partial class MouseClickExecutor : ClickExecutorBase
{
    // WinAPI для перемещения мыши
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetCursorPos(int X, int Y);

    // WinAPI для клика мышью
    [LibraryImport("user32.dll")]
    private static partial void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

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