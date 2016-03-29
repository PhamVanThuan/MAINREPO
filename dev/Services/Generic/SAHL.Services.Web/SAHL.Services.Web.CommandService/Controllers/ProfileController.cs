using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using System.Net.Http;
using System.Web.Http;
using System.Reflection;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Drawing.Drawing2D;
using System.DirectoryServices;
using System.Security.Cryptography;

namespace SAHL.Services.Web.CommandService.Controllers
{
    public enum AnchorPosition
    {
        /// <summary>
        /// Anchors the position of the image to the top of it's bounding container.
        /// </summary>
        Top,

        /// <summary>
        /// Anchors the position of the image to the center of it's bounding container.
        /// </summary>
        Center,

        /// <summary>
        /// Anchors the position of the image to the bottom of it's bounding container.
        /// </summary>
        Bottom,

        /// <summary>
        /// Anchors the position of the image to the left of it's bounding container.
        /// </summary>
        Left,

        /// <summary>
        /// Anchors the position of the image to the right of it's bounding container.
        /// </summary>
        Right
    }

    public enum ResizeMode
    {
        /// <summary>
        /// Pads the resized image to fit the bounds of its container.
        /// </summary>
        Pad,

        /// <summary>
        /// Stretches the resized image to fit the bounds of its container.
        /// </summary>
        Stretch,

        /// <summary>
        /// Crops the resized image to fit the bounds of its container.
        /// </summary>
        Crop,

        /// <summary>
        /// Constrains the resized image to fit the bounds of its container.
        /// </summary>
        Max
    }

    public class ProfileController : ApiController
    {
        private const string LdapAdBaseQuery = "LDAP://DC=SAHL,DC=com";
        private const string LdapUserFilter  = "(&(SAMAccountName={0}))";

        [HttpGet]
        public HttpResponseMessage GetImage(string userName, int size = 24)
        {
            Image userImage  = null;

            try
            {
                var userData = this.RetrieveUserDataFromLdap(userName);
                if (userData == null) { throw new Exception("User not found in LDAP"); }

                var emailAddress = this.RetrieveUserEmailAddress(userData);
                userImage        = this.RetrieveUserImage(userData) ?? this.RetrieveUserGravatar(emailAddress);
            }
            catch (Exception)
            {
                // Do nothing ... Default image will be returned
            }
            finally
            {
                if (userImage == null) { userImage = this.GetDefaultImage(); }
            }

            var finalImage      = this.ResizeImage(userImage, size, size, 48, 48, Color.Transparent);
            var responseMessage = this.BuildHttpResponseMessage(finalImage);
            return responseMessage;
        }

        private HttpResponseMessage BuildHttpResponseMessage(Image finalImage)
        {
            var httpResponseMessage = new HttpResponseMessage
                {
                    Content    = new ByteArrayContent(this.ConvertImageToJpegByteArray(finalImage)),
                    StatusCode = HttpStatusCode.OK
                };

            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            httpResponseMessage.Headers.CacheControl        = new CacheControlHeaderValue
                {
                    MaxAge = new TimeSpan(0, 10, 0), 
                    Public = true
                };

            return httpResponseMessage;
        }

        private SearchResult RetrieveUserDataFromLdap(string username)
        {
            var directoryEntry    = new DirectoryEntry(LdapAdBaseQuery, "sahl\\Halouser", "Natal123");
            var directorySearcher = new DirectorySearcher(directoryEntry);

            directorySearcher.PropertiesToLoad.Add("mail");
            directorySearcher.PropertiesToLoad.Add("thumbnailPhoto");
            directorySearcher.Filter = string.Format(LdapUserFilter, username);

            var userData = directorySearcher.FindOne();
            return userData;
        }

        private string RetrieveUserEmailAddress(SearchResult userData)
        {
            if (!userData.Properties.Contains("mail")) { return string.Empty; }
            if (userData.Properties["mail"].Count == 0) { return string.Empty; }

            var emailAddress = userData.Properties["mail"][0].ToString();
            return emailAddress;
        }

        private Image RetrieveUserImage(SearchResult userData)
        {
            if (!userData.Properties.Contains("thumbnailPhoto")) { return null; }

            var bytes = userData.Properties["thumbnailPhoto"][0] as byte[];
            if (bytes == null) { return null; }

            using (var memoryStream = new MemoryStream(bytes))
            {
                var userImage = Image.FromStream(memoryStream);
                return userImage;
            }
        }

        private Image RetrieveUserGravatar(string emailAddress)
        {
            var hashedEmail = this.HashEmailAddress(emailAddress.ToLower());
            var url         = string.Format("http://www.gravatar.com/avatar/{0}?d=mm", hashedEmail);

            WebRequest webRequest = (HttpWebRequest) WebRequest.Create(url);
            var webProxy = new WebProxy("http://192.168.11.27:8080/", true)
                {
                    Credentials = new NetworkCredential("HaloUser", "Natal123", "SAHL")
                };
            webRequest.Proxy = webProxy;

            var webResponse = (HttpWebResponse) webRequest.GetResponse();
            var imageStream = webResponse.GetResponseStream();
            var userImage   = Image.FromStream(imageStream);
            return userImage;
        }

        private string HashEmailAddress(string address)
        {
            MD5 cryptoServiceProvider = new MD5CryptoServiceProvider();
            var hasedBytes            = cryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(address));
            var hashedEmail           = new StringBuilder();

            foreach (var currentByte in hasedBytes)
            {
                hashedEmail.Append(currentByte.ToString("X2"));
            }

            return hashedEmail.ToString().ToLower();
        }

        private Image GetDefaultImage()
        {
            var entryAssembly = Assembly.GetAssembly(typeof(ProfileController));

            var userImage     = entryAssembly.GetManifestResourceStream("SAHL.Services.Web.CommandService.Images.user.png");
            if (userImage == null) { return null; }

            var userBitmap = new Bitmap(userImage);
            return userBitmap;
        }

        private byte[] ConvertImageToJpegByteArray(Image finalImage)
        {
            var memoryStream = new MemoryStream();
            finalImage.Save(memoryStream, ImageFormat.Jpeg);
            return memoryStream.ToArray();
        }

        private Image ResizeImage(Image image, int width, int height, int defaultMaxWidth, int defaultMaxHeight, Color backgroundColor,
                                  ResizeMode resizeMode = ResizeMode.Pad, AnchorPosition anchorPosition = AnchorPosition.Center, bool upscale = true)
        {
            var imageSizeHelper = new ImageSizeHelper(resizeMode, anchorPosition, upscale, width, height, image.Width, image.Height, 
                                                        defaultMaxWidth, defaultMaxHeight);
            if (imageSizeHelper.IsScalingNotAllowed) { return image; }

            // Change the destination rectangle coordinates if padding and
            // there has been a set width and height.
            if (imageSizeHelper.IsPaddingRequired) { imageSizeHelper.CalculatePadding(); }

            // Change the destination rectangle coordinates if cropping and
            // there has been a set width and height.
            if (imageSizeHelper.IsCroppingRequired) { imageSizeHelper.CalculateCropping(); }

            // Constrain the image to fit the maximum possible height or width.
            if (imageSizeHelper.IsMaxResize) { imageSizeHelper.CalculateMaxResize(); }

            return imageSizeHelper.IsResizeRequired 
                        ? this.CreateResizedImage(image, backgroundColor, imageSizeHelper) 
                        : image;
        }

        private Bitmap CreateResizedImage(Image image, Color backgroundColor, ImageSizeHelper imageSizeHelper)
        {
            var newImage = new Bitmap(imageSizeHelper.Width, imageSizeHelper.Height, PixelFormat.Format32bppPArgb);

            using (var graphics = Graphics.FromImage(newImage))
            {
                // We want to use two different blending algorithms for enlargement/shrinking.
                // Bicubic is better enlarging for whilst Bilinear is better for shrinking.
                // http://www.codinghorror.com/blog/2007/07/better-image-resizing.html
                if (imageSizeHelper.Width < imageSizeHelper.CalculatedWidth && imageSizeHelper.Height < imageSizeHelper.CalculatedHeight)
                {
                    // We are making it larger.
                    graphics.SmoothingMode      = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode  = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode    = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                }
                else
                {
                    // We are making it smaller.
                    graphics.SmoothingMode      = SmoothingMode.None;
                    graphics.InterpolationMode  = InterpolationMode.HighQualityBilinear;
                    graphics.PixelOffsetMode    = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                }

                // An unwanted border appears when using InterpolationMode.HighQualityBicubic to resize the image
                // as the algorithm appears to be pulling averaging detail from surCeilinging pixels beyond the edge
                // of the image. Using the ImageAttributes class to specify that the pixels beyond are simply mirror
                // images of the pixels within solves this problem.
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.Clear(backgroundColor);

                    var destRect = new Rectangle(imageSizeHelper.CalculatedX, imageSizeHelper.CalculatedY, 
                                                 imageSizeHelper.CalculatedWidth, imageSizeHelper.CalculatedHeight);
                    graphics.DrawImage(image, destRect, 0, 0, imageSizeHelper.ImageWidth, imageSizeHelper.ImageHeight, GraphicsUnit.Pixel, wrapMode);
                }

                // Reassign the image.
                image.Dispose();
                return newImage;
            }
        }
    }
}
