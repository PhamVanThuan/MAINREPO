using System;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ICDVRepository
    {
        bool ValidateAccountNo(string p_BranchNo, int p_AccType, String p_AccountNo);

        string ErrorMessage { get; }

        string ExceptionRoutine { get; }
    }
}