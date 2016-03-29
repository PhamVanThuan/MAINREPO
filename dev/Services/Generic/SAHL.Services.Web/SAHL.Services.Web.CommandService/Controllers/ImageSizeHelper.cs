using System;

namespace SAHL.Services.Web.CommandService.Controllers
{
    public class ImageSizeHelper
    {
        private readonly double percentWidth;
        private readonly double percentHeight;

        public ImageSizeHelper(ResizeMode resizeMode, AnchorPosition anchorPosition, bool upscale, int width, int height,
                               int imageWidth, int imageHeight, int defaultMaxWidth, int defaultMaxHeight)
        {
            this.ResizeMode       = resizeMode;
            this.AnchorPosition   = anchorPosition;
            this.Upscale          = upscale;
            this.Width            = width;
            this.Height           = height;
            this.ImageWidth       = imageWidth;
            this.ImageHeight      = imageHeight;
            this.CalculatedX      = 0;
            this.CalculatedY      = 0;
            this.CalculatedWidth  = width;
            this.CalculatedHeight = height;
            this.MaxWidth         = defaultMaxWidth > 0 ? defaultMaxWidth : int.MaxValue;
            this.MaxHeight        = defaultMaxHeight > 0 ? defaultMaxHeight : int.MaxValue;
            this.percentWidth     = Math.Abs(width / (double)this.ImageWidth);
            this.percentHeight    = Math.Abs(height / (double)this.ImageHeight);

            // If height or width is not passed we assume that the standard ratio is to be kept.
            if (height == 0)
            {
                this.CalculatedHeight = (int)Math.Ceiling(this.ImageHeight * percentWidth);
                this.Height           = this.CalculatedHeight;
            }

            if (width == 0)
            {
                this.CalculatedWidth = (int)Math.Ceiling(this.ImageWidth * percentHeight);
                this.Width           = this.CalculatedWidth;
            }
        }

        public ResizeMode ResizeMode { get; set; }
        public bool Upscale { get; set; }
        public AnchorPosition AnchorPosition { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }

        public int CalculatedX { get; private set; }
        public int CalculatedY { get; private set; }
        public int CalculatedWidth { get; private set; }
        public int CalculatedHeight { get; private set; }

        public bool IsScalingNotAllowed
        {
            get { return (this.Width > this.ImageWidth || this.Height > this.ImageHeight) && !this.Upscale && this.ResizeMode != ResizeMode.Stretch; }
        }

        public bool IsPaddingRequired
        {
            get { return this.ResizeMode == ResizeMode.Pad && this.Width > 0 && this.Height > 0; }
        }

        public bool IsCroppingRequired
        {
            get { return this.ResizeMode == ResizeMode.Crop && this.Width > 0 && this.Height > 0; }
        }

        public bool IsMaxResize
        {
            get { return this.ResizeMode == ResizeMode.Max; }
        }

        public bool IsEnlarging
        {
            get { return this.Width < this.CalculatedWidth && this.Height < this.CalculatedHeight; }
        }

        public bool IsResizeRequired
        {
            get
            {
                return this.Width > 0 &&
                       this.Height > 0 &&
                       this.Width <= this.MaxWidth &&
                       this.Height <= this.MaxHeight;
            }
        }

        public void CalculatePadding()
        {
            double ratio;

            if (percentHeight < percentWidth)
            {
                ratio                = percentHeight;
                this.CalculatedX     = (int)((this.Width - (this.ImageWidth * ratio)) / 2);
                this.CalculatedWidth = (int)Math.Ceiling(this.ImageWidth * percentHeight);
            }
            else
            {
                ratio                 = percentWidth;
                this.CalculatedY      = (int)((this.Height - (this.ImageHeight * ratio)) / 2);
                this.CalculatedHeight = (int)Math.Ceiling(this.ImageHeight * percentWidth);
            }
        }

        public void CalculateCropping()
        {
            double ratio;

            if (percentHeight < percentWidth)
            {
                ratio = percentWidth;

                switch (this.AnchorPosition)
                {
                    case AnchorPosition.Top:
                        this.CalculatedY = 0;
                        break;

                    case AnchorPosition.Bottom:
                        this.CalculatedY = (int)(this.Height - (this.ImageHeight * ratio));
                        break;

                    default:
                        this.CalculatedY = (int)((this.Height - (this.ImageHeight * ratio)) / 2);
                        break;
                }

                this.CalculatedHeight = (int)Math.Ceiling(this.ImageHeight * percentWidth);
            }
            else
            {
                ratio = percentHeight;

                switch (this.AnchorPosition)
                {
                    case AnchorPosition.Left:
                        this.CalculatedX = 0;
                        break;

                    case AnchorPosition.Right:
                        this.CalculatedX = (int)(this.Width - (this.ImageWidth * ratio));
                        break;

                    default:
                        this.CalculatedX = (int)((this.Width - (this.ImageWidth * ratio)) / 2);
                        break;
                }

                this.CalculatedWidth = (int)Math.Ceiling(this.ImageWidth * percentHeight);
            }
        }

        public void CalculateMaxResize()
        {
            if (this.ImageWidth <= this.Width && this.ImageHeight <= this.Height) { return; }

            double ratio       = Math.Abs(this.Height / this.Width);
            double sourceRatio = Math.Abs(this.ImageHeight / this.ImageWidth);

            if (sourceRatio < ratio)
            {
                this.Height = 0;
            }
            else
            {
                this.Width = 0;
            }
        }
    }
}
