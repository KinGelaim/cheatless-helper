using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace OverlootingAutoClick.ElementFinder;

internal sealed class TemplateMatchingFinder : IElementFinder
{
    private string _templatePath;
    private double _threshold;

    public TemplateMatchingFinder(string templatePath, double threshold = 0.9)
    {
        _templatePath = templatePath;
        _threshold = threshold;
    }

    public Point? FindElementBitmap(string windowImagePath)
    {
        using var sourceMat = CvInvoke.Imread(windowImagePath);
        using var templateMat = CvInvoke.Imread(_templatePath);
        using var resultMat = new Mat();
        CvInvoke.MatchTemplate(sourceMat, templateMat, resultMat, TemplateMatchingType.CcorrNormed);

        double minVal = 0, maxVal = 0;
        Point minLoc = new(), maxLoc = new();
        CvInvoke.MinMaxLoc(resultMat, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

        Console.WriteLine($"Максимальное совпадение: {maxVal}");

        if (maxVal > _threshold)
        {
            var clickX = maxLoc.X + templateMat.Width / 2;
            var clickY = maxLoc.Y + templateMat.Height / 2;
            return new Point(clickX, clickY);
        }
        return null;
    }
}