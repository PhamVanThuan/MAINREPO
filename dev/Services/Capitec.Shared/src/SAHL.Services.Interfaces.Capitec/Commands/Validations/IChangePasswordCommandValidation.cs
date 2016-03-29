using SAHL.Core.Services.Extensions;

namespace SAHL.Services.Interfaces.Capitec.Commands.Validations
{
    public interface IChangePasswordCommandValidation : ICommandValidation<ChangePasswordCommand, PasswordOnly>
    {
    }
}
