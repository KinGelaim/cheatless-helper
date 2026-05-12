using Emgu.CV;

namespace OverlootingAutoClick.Resources;

/// <summary>
/// Информация о ресурсе
/// </summary>
/// <param name="name">Наименование ресурса</param>
/// <param name="path">Путь к изображению ресурса</param>
/// <param name="threshold">Пороговое значение совпадения при поиске ресурса</param>
internal sealed class ResourceInfo(
    string name,
    string path,
    double threshold = 0.96) : IDisposable
{
    /// <summary>
    /// Наименование ресурса
    /// </summary>
    public string Name { get; init; } = name;

    /// <summary>
    /// Путь к изображению ресурса
    /// </summary>
    public string Path { get; init; } = path;

    /// <summary>
    /// Пороговое значение совпадения при поиске ресурса
    /// </summary>
    public double Threshold { get; init; } = threshold;

    /// <summary>
    /// Информация об изображении ресурса
    /// </summary>
    public Mat? ImageMat { get; set; }

    public void Dispose()
    {
        ImageMat?.Dispose();
    }
}