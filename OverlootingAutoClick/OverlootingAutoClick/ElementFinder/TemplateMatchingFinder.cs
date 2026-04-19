using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing;

namespace OverlootingAutoClick.ElementFinder;

internal sealed class TemplateMatchingFinder : IElementFinder
{
    private string _templatePath;
    private double _threshold;

    public TemplateMatchingFinder(string templatePath, double threshold = 0.94)
    {
        _templatePath = templatePath;
        _threshold = threshold;
    }

    public Point? FindElementBitmap(string windowImagePath)
    {
        // 1. Загружаем изображения
        using var sourceMat = CvInvoke.Imread(windowImagePath, ImreadModes.AnyColor);
        using var templateMat = CvInvoke.Imread(_templatePath, ImreadModes.Unchanged);

        if (sourceMat.IsEmpty || templateMat.IsEmpty)
        {
            Console.WriteLine("Ошибка при загрузке изображений. Проверьте пути к файлам.");
            return null;
        }

        // Получаем альфа-канал
        VectorOfMat channels = new VectorOfMat();
        CvInvoke.Split(templateMat, channels);
        Mat alphaMask = channels[3];

        // Создаем маску: если альфа > 0, то 255, иначе 0
        CvInvoke.Threshold(alphaMask, alphaMask, 0, 255, ThresholdType.Binary);

        // Преобразуем шаблон к 3 каналам, если у бэкграунда нет альфы
        Mat templateBgr = new Mat();
        CvInvoke.CvtColor(templateMat, templateBgr, ColorConversion.Bgra2Bgr);

        // Создаем матрицу для результата
        int resultCols = sourceMat.Cols - templateBgr.Cols + 1;
        int resultRows = sourceMat.Rows - templateBgr.Rows + 1;

        using (Mat result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1))
        {
            // Выполняем сопоставление шаблонов
            CvInvoke.MatchTemplate(sourceMat, templateBgr, result, TemplateMatchingType.CcorrNormed, alphaMask);

            // Находим максимум (лучшее совпадение)
            double minVal = 0, maxVal = 0;
            Point minLoc = new();
            Point maxLoc = new();

            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

            if (maxVal > _threshold)
            {
                // maxLoc - это координаты верхнего левого угла найденного шаблона
                Console.WriteLine($"Лучшее совпадение: {maxVal} в точке {maxLoc}");

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
    }
}