namespace OverlootingAutoClick.WindowFinder;

/// <summary>
/// Поиск окна Windows по полному совпадению имени процесса
/// </summary>
internal sealed class ExactTitleWindowFinder : WindowFinderBase
{
    private readonly string _title;

    public ExactTitleWindowFinder(string title)
    {
        _title = title;
    }

    /// <inheritdoc/>
    public override IntPtr FindWindow()
    {
        var foundHWnd = IntPtr.Zero;

        bool Callback(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                const int nChars = 256;
                var buffer = new System.Text.StringBuilder(nChars);
                GetWindowText(hWnd, buffer, nChars);
                if (buffer.ToString() == _title)
                {
                    foundHWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

        EnumWindows(Callback, IntPtr.Zero);
        return foundHWnd;
    }
}