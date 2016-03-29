using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SAHL.Services.DocumentManager.Utils.PdfConverter
{
    public class PdfConverter : IPdfConverter
    {
        public byte[] ConvertImageToPdf(byte[] imageToConvert, int pdfVersion = 0)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageToConvert))
            {
                memoryStream.Position = 0;
                memoryStream.Write(imageToConvert, 0, imageToConvert.Length);
                Image image = Image.FromStream(memoryStream, true, true);

                PdfDocument doc = new PdfDocument();
                if(pdfVersion > 0)
                {
                    doc.Version = pdfVersion;
                }
                int count = image.GetFrameCount(FrameDimension.Page);
                for (int pageNumber = 0; pageNumber < count; pageNumber++)
                {
                    image.SelectActiveFrame(FrameDimension.Page, pageNumber);
                    PdfPage page = new PdfPage();
                    doc.Pages.Add(page);
                    XGraphics graphic = XGraphics.FromPdfPage(page);
                    XImage xImage = XImage.FromGdiPlusImage(image);
                    graphic.DrawImage(xImage, 0, 0, page.Width, page.Height);
                }
                using (MemoryStream pdfStream = new MemoryStream())
                {
                    pdfStream.Position = 0;
                    doc.Save(pdfStream, false);
                    var result = pdfStream.ToArray();
                    return result;
                }
            }
        }
    }
}