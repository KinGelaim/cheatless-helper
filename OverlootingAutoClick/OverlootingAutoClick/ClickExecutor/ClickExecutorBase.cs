using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick.ClickExecutor;

/// <summary>
/// Базовый класс для выполнения клика
/// </summary>
internal abstract partial class ClickExecutorBase : IClickExecutor
{
    // WinAPI для установки окна на передний план
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    protected static partial bool SetForegroundWindow(IntPtr hWnd);

    /// <inheritdoc/>
    public abstract void ClickAt(Point position, nint hWnd);
}