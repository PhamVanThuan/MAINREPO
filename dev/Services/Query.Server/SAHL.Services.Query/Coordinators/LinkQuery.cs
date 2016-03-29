using SAHL.Core.Extensions;

namespace SAHL.Services.Query.Coordinators
{
    public class LinkQuery
    {
        public string Relationship { get; private set; }
        public string RelativeUrl { get; private set; }
        public string AbsoluteUrl { get; private set; }
        public string AbsolutePath { get; private set; }

        public string JsonResult { get; set; }

        public LinkQuery(string relationship, string relativeUrl, string absoluteUrl, string absolutePath)
        {
            this.Relationship = relationship;
            this.RelativeUrl = relativeUrl;
            this.AbsoluteUrl = absoluteUrl;
            this.AbsolutePath = absolutePath;
        }

        public bool IsTemplatedUrl()
        {
            var indexOfOpenBrace = this.RelativeUrl.IndexOf("{");
            if (indexOfOpenBrace < 0)
            {
                return false;
            }
            var indexOfCloseBrace = this.RelativeUrl.IndexOf("}");
            if (indexOfCloseBrace < 0)
            {
                return false;
            }

            var indexOfQuestionMark = this.RelativeUrl.IndexOf("?");
            if (indexOfOpenBrace > indexOfQuestionMark && indexOfQuestionMark > 0)
            {
                return false;
            }
            
            //opening brace should occur before the closing brace, then we can assume url is templated
            return indexOfOpenBrace < (indexOfCloseBrace - 1);
        }
    }
}