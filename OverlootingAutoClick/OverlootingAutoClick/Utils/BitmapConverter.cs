using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Drawing.Imaging;

namespace OverlootingAutoClick.Utils;

/// <summary>
/// Преобразователь изображений
/// </summary>
internal static class BitmapConverter
{
    /// <summary>
    /// Конвертирует Bitmap изображение с Alpha каналом в Mat без Alpha канала
    /// </summary>
    /// <param name="bitmap">Исходное изображение</param>
    /// <returns>Mat без Alpha канала</returns>
    public static Mat BitmapToMatNoAlpha(this Bitmap bitmap)
    {
        // Блокировка битов в формате 24bpp (BGR), чтобы игнорировать альфа канал
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        // Создание Mat из указателя
        var mat = new Mat(bmpData.Height, bmpData.Width, DepthType.Cv8U, 3, bmpData.Scan0, bmpData.Stride);

        // Важно: Клонируем Mat, чтобы Mat владел своими данными (независимо от Bitmap), 
        // после разблокироваки Bits,
        // иначе он будет указывать на освобождённую память
        var finalMat = mat.Clone();

        bitmap.UnlockBits(bmpData);
        return finalMat;
    }
}