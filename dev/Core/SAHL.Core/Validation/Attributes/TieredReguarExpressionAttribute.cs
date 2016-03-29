using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Validation.Attributes
{
    /// <summary>
    /// Provides a mechanism to provide a second regular expression pattern to match against, instead of the first
    /// </summary>
    public class TieredReguarExpressionAttribute : RegularExpressionAttribute
    {
        private RegularExpressionAttribute regularExpressionAttribute;

        private RegularExpressionAttribute RegularExpressionAttribute
        {
            get
            {
                if (regularExpressionAttribute == null)
                {
                    regularExpressionAttribute = new RegularExpressionAttribute(ServerSidePattern);
                }
                return regularExpressionAttribute;
            }
        }

        private string ServerSidePattern { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="clientSidePattern">The pattern that is to be used for client-side validation (e.g. jquery validation)</param>
        /// <param name="serverSidePattern">The pattern that is to be used for server-side validation (e.g. by the System.ComponentModel.DataAnnotations.Validator class)</param>
        public TieredReguarExpressionAttribute(string clientSidePattern, string serverSidePattern)
            : base(clientSidePattern)
        {
            ServerSidePattern = serverSidePattern;
        }

        public override bool IsValid(object value)
        {
            return RegularExpressionAttribute.IsValid(value);
        }
    }
}