namespace OverlootingAutoClick.WindowFinder;

/// <summary>
/// Поиск окна Windows по полному совпадению имени процесса
/// </summary>
internal sealed class ExactTitleWindowFinder(string title) : WindowFinderBase
{
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
                _ = GetWindowText(hWnd, buffer, nChars);
                if (buffer.ToString() == title)
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