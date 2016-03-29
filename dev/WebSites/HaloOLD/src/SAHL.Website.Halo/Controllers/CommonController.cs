using SAHL.Core.Web;
using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Halo.Commands;
using System;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace SAHL.Website.Halo.Controllers
{
    public class CommonController : Controller
    {
        private IHaloService haloService;

        public CommonController(IHaloService haloService)
        {
            this.haloService = haloService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ImageResult GetUserImage()
        {
            Image userPhoto = null;
            try
            {
                var command = new GetUserDetailsForUserCommand(this.User.Identity.Name);
                var result = haloService.PerformCommand(command);
                var userDetails = command.Result;

                if (userDetails.UserPhoto == null)
                {
                    // try get a gravatar
                    string hashedEmail = this.HashEmailAddress(userDetails.EmailAddress.ToLower());
                    string url = string.Format("http://www.gravatar.com/avatar/{0}", hashedEmail);

                    WebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

                    WebProxy webProxy = new WebProxy("http://192.168.11.27:8080/", true)
                    {
                        Credentials = new NetworkCredential("HaloUser", "Natal123", "SAHL")
                    };

                    webRequest.Proxy = webProxy;

                    var webResponse = (HttpWebResponse)webRequest.GetResponse();

                    var imageStream = webResponse.GetResponseStream();
                    userDetails.UpdatePhoto(Image.FromStream(imageStream));
                }
                userPhoto = userDetails.UserPhoto;
            }
            catch (Exception ex)
            {
                int a = 0;
            }
            return new ImageResult(userPhoto, userPhoto.RawFormat);
        }

        public ActionResult ChangeUserRole(string organisationArea, string roleName)
        {
            var command = new ChangeUserRoleCommand(this.User.Identity.Name, organisationArea, roleName);
            var result = haloService.PerformCommand(command);

            return RedirectToAction("Index", "Client");
        }

        private string HashEmailAddress(string address)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            var hasedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(address));

            var sb = new StringBuilder();

            for (var i = 0; i < hasedBytes.Length; i++)
            {
                sb.Append(hasedBytes[i].ToString("X2"));
            }

            return sb.ToString().ToLower();
        }
    }
}