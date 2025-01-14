using System.Drawing;
using System.Drawing.Imaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace TagsCloudVisualization.BitmapProcessors;

public class PdfBitmapProcessor : IBitmapProcessor
{
    public void SaveImage(Bitmap bitmap, string imageDirectory, string imageName)
    {
        using var memoryStream = new MemoryStream();
        bitmap.Save(memoryStream, ImageFormat.Png);
        memoryStream.Position = 0;
        
        var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);

        var image = XImage.FromStream(memoryStream);
        gfx.DrawImage(image, 0, 0, page.Width, page.Height);

        var pdfPath = Path.Combine(imageDirectory, $"{imageName}.pdf");
        document.Save(pdfPath);
    }
}