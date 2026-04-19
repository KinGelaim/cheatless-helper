namespace OverlootingAutoClick.Utils;

internal static class ImagePaths
{
    private static readonly Dictionary<ImageResource, string> paths = new()
    {
        { ImageResource.ArrowNextLevel, "Images/ArrowNextLevel.png" },
        { ImageResource.Chest, "Images/Chest.png" }
    };

    public static string GetPath(ImageResource resource)
    {
        if (paths.TryGetValue(resource, out var path))
        {
            return path;
        }

        throw new ArgumentException($"Путь для {resource} не найден");
    }
}

internal enum ImageResource
{
    ArrowNextLevel,
    Chest
}