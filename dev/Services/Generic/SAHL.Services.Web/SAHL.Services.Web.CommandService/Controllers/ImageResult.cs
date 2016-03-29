using System;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace SAHL.Services.Web.CommandService.Controllers
{
    public class ImageResult : ActionResult
    {
        private static Dictionary<ImageFormat, string> FormatMap { get; set; }

        static ImageResult()
        {
            CreateContentTypeMap();
        }

        public ImageResult(Image image, ImageFormat imageFormat)
        {
            this.Image       = image;
            this.ImageFormat = imageFormat;
        }

        public Image Image { get; protected set; }

        public ImageFormat ImageFormat { get; protected set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (this.Image == null) { throw new ArgumentNullException("Image"); }
            if (this.ImageFormat == null) { throw new ArgumentNullException("ImageFormat"); }

            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = FormatMap[ImageFormat];

            this.Image.Save(context.HttpContext.Response.OutputStream, ImageFormat);
        }

        private static void CreateContentTypeMap()
        {
            FormatMap = new Dictionary<ImageFormat, string>
                {
                    { ImageFormat.Bmp, "image/bmp" },
                    { ImageFormat.Gif, "image/gif" },
                    { ImageFormat.Icon, "image/vnd.microsoft.icon" },
                    { ImageFormat.Jpeg, "image/Jpeg" },
                    { ImageFormat.Png, "image/png" },
                    { ImageFormat.Tiff, "image/tiff" },
                    { ImageFormat.Wmf, "image/wmf" }
                };
        }
    }
}
