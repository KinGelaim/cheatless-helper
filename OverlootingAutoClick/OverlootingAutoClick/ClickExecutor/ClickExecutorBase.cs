using System.Drawing;
using System.Runtime.InteropServices;

namespace OverlootingAutoClick.ClickExecutor;

/// <summary>
/// Базовый класс для выполнения клика
/// </summary>
internal abstract class ClickExecutorBase : IClickExecutor
{
    // WinAPI для установки окна на передний план
    [DllImport("user32.dll")]
    protected static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <inheritdoc/>
    public abstract void ClickAt(Point position, nint hWnd);
}