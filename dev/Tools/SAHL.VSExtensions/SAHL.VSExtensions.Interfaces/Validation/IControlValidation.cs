using System;

namespace SAHL.VSExtensions.Interfaces.Validation
{
    public interface IControlValidation
    {
        bool Result { get; }

        string ResultMessage { get; }

        void ValidateControl(string message, Func<bool> isValid);
    }
}