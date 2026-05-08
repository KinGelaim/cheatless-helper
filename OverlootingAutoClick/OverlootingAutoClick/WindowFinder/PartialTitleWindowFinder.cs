namespace OverlootingAutoClick.WindowFinder;

/// <summary>
/// Поиск окна Windows по частичному совпадению имени процесса
/// </summary>
internal sealed class PartialTitleWindowFinder(string titlePart) : WindowFinderBase
{
    /// <inheritdoc/>
    public override IntPtr FindWindow()
    {
        var foundHWnd = IntPtr.Zero;

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
}