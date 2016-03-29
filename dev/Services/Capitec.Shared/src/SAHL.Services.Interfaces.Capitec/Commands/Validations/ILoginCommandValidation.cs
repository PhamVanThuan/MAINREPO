using SAHL.Core.Services.Extensions;

namespace SAHL.Services.Interfaces.Capitec.Commands.Validations
{
    public interface ICalculateNewPurchaseQueryValidation : ICommandValidation<LoginCommand, UsernamePassword>
    {
    }
}
