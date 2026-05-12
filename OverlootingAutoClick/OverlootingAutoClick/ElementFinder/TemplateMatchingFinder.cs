using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using OverlootingAutoClick.Resources;
using OverlootingAutoClick.Utils;
using System.Drawing;

namespace OverlootingAutoClick.ElementFinder;

/// <summary>
/// Поиск элемента (ресурса) на заданном изображении
/// </summary>
/// <param name="windowImage">Исходное (заданное) изображение</param>
internal sealed class TemplateMatchingFinder(Bitmap windowImage) : IElementFinder, IDisposable
{
    private readonly Mat _sourceMat = BitmapConverter.BitmapToMatNoAlpha(windowImage);

    /// <inheritdoc/>
    public Point? FindElementBitmap(ResourceInfo resourceInfo)
    {
        // 1. Загружаем изображения
        var templateMat = resourceInfo.ImageMat;

        if (_sourceMat.IsEmpty || templateMat is null || templateMat.IsEmpty)
        {
            Console.WriteLine("Ошибка при загрузке изображений. Проверьте пути к файлам.");
            return null;
        }

        // Получаем альфа-канал
        var channels = new VectorOfMat();
        CvInvoke.Split(templateMat, channels);
        Mat alphaMask = channels[3];

        // Создаем маску: если альфа > 0, то 255, иначе 0
        CvInvoke.Threshold(alphaMask, alphaMask, 0, 255, ThresholdType.Binary);

        // Преобразуем шаблон к 3 каналам, если у бэкграунда нет альфы
        var templateBgr = new Mat();
        CvInvoke.CvtColor(templateMat, templateBgr, ColorConversion.Bgra2Bgr);

        // Создаем матрицу для результата
        int resultCols = _sourceMat.Cols - templateBgr.Cols + 1;
        int resultRows = _sourceMat.Rows - templateBgr.Rows + 1;

        using var result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1);
        // Выполняем сопоставление шаблонов
        CvInvoke.MatchTemplate(_sourceMat, templateBgr, result, TemplateMatchingType.CcorrNormed, alphaMask);

        // Находим максимум (лучшее совпадение)
        double minVal = 0, maxVal = 0;
        Point minLoc = new(), maxLoc = new();

        CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

        //Console.WriteLine($"Лучшее совпадение: {resource.Name} {maxVal} в точке {maxLoc}");

        if (maxVal > resourceInfo.Threshold)
        {
            // maxLoc - это координаты верхнего левого угла найденного шаблона
            Console.WriteLine($"Лучшее совпадение: {resourceInfo.Name} {maxVal} в точке {maxLoc}");

            // Отображение (опционально)
            //CvInvoke.Imwrite("template_color.png", templateBgr);
            //CvInvoke.Rectangle(sourceMat, new Rectangle(maxLoc, new Size(templateBgr.Width, templateBgr.Height)), new MCvScalar(0, 255, 0), 2);
            //CvInvoke.Imshow("Result", sourceMat);
            //CvInvoke.WaitKey(0);

            var clickX = maxLoc.X + templateMat.Width / 2;
            var clickY = maxLoc.Y + templateMat.Height / 2;
            return new Point(clickX, clickY);
        }
        return null;
    }

    public void Dispose() => _sourceMat.Dispose();
}