namespace OverlootingAutoClick.WindowFinder;

/// <summary>
/// Поиск окна
/// </summary>
internal interface IWindowFinder
{
    /// <summary>
    /// Поиск окна среди процессов
    /// </summary>
    /// <returns>Окно</returns>
    public IntPtr FindWindow();
}