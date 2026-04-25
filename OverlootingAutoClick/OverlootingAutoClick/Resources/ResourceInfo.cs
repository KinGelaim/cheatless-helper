using Emgu.CV;

namespace OverlootingAutoClick.Resources;

internal sealed class ResourceInfo(
    string name,
    string path,
    double threshold = 0.96) : IDisposable
{
    public string Name { get; init; } = name;

    public string Path { get; init; } = path;

    public double Threshold { get; init; } = threshold;

    public Mat? ImageMat { get; set; }

    public void Dispose()
    {
        ImageMat?.Dispose();
    }
}