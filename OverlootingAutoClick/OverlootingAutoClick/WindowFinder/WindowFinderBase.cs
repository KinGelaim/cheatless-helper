using System.Runtime.InteropServices;

namespace OverlootingAutoClick.WindowFinder;

/// <summary>
/// Базовый класс для поиска окна Windows
/// </summary>
internal abstract partial class WindowFinderBase : IWindowFinder
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    protected static partial bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    protected static partial bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    protected static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int maxLength);

    protected delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    /// <inheritdoc/>
    public abstract IntPtr FindWindow();
}