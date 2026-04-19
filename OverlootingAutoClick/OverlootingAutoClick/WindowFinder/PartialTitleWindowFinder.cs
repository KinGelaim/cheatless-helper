namespace OverlootingAutoClick.WindowFinder;

internal sealed class PartialTitleWindowFinder : WindowFinderBase
{
    private readonly string _titlePart;

    public PartialTitleWindowFinder(string titlePart)
    {
        _titlePart = titlePart;
    }

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