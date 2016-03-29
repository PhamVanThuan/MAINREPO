namespace SAHL.Services.DocumentManager.Utils.PdfConverter
{
    public interface IPdfConverter
    {
        byte[] ConvertImageToPdf(byte[] imageToConvert, int pdfVersion = 0);
    }
}