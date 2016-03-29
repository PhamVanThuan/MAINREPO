using System;
using System.Text;

namespace SAHL.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string BuildExceptionErrorMessage(this Exception exception)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine(exception.Message);

            if (exception.InnerException != null)
            {
                errorMessage.AppendLine(exception.InnerException.BuildExceptionErrorMessage());
            }

            return errorMessage.ToString();
        }
    }
}
