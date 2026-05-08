namespace OverlootingAutoClick.WindowFinder;

/// <summary>
/// Поиск окна Windows по частичному совпадению имени процесса
/// </summary>
internal sealed class PartialTitleWindowFinder : WindowFinderBase
{
    private readonly string _titlePart;

    public PartialTitleWindowFinder(string titlePart)
    {
        _titlePart = titlePart;
    }

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

                if (!string.IsNullOrEmpty(windowTitle) && windowTitle.Contains(_titlePart))
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