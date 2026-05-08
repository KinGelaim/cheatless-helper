using System.Runtime.InteropServices;

namespace OverlootingAutoClick.WindowFinder;

/// <summary>
/// Базовый класс для поиска окна Windows
/// </summary>
internal abstract class WindowFinderBase : IWindowFinder
{
    [DllImport("user32.dll")]
    protected static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    protected static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    protected static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int maxLength);

    protected delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    /// <inheritdoc/>
    public abstract IntPtr FindWindow();
}