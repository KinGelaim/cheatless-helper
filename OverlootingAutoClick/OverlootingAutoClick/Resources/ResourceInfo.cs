using Emgu.CV;

namespace OverlootingAutoClick.Resources;

internal sealed class ResourceInfo(
    ResourceType type,
    string name,
    string path) : IDisposable
{
    public ResourceType Type { get; init; } = type;
    public string Name { get; init; } = name;
    public string Path { get; init; } = path;
    public Mat? ImageMat { get; set; }

    public void Dispose()
    {
        ImageMat?.Dispose();
    }
}