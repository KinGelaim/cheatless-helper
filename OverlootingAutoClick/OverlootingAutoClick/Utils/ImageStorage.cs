using System.Drawing;
using System.Drawing.Imaging;

namespace OverlootingAutoClick.Utils;

internal sealed class ImageStorage
{
    public string SaveToTempFile(Bitmap bitmap, ImageFormat? format = null)
    {
        // По умолчанию сохраняем PNG
        format ??= ImageFormat.Png;

        var tempDir = Path.GetTempPath();
        var fileName = $"capture_{Guid.NewGuid()}.{format.ToString().ToLower()}";
        var filePath = Path.Combine(tempDir, fileName);

        bitmap.Save(filePath, format);
        return filePath;
    }

    public void DeleteFile(string pathToFile)
    {
        File.Delete(pathToFile);
    }
}