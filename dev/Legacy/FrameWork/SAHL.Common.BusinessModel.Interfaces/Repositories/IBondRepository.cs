using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// The Bond repository
    /// </summary>
    public interface IBondRepository
    {
        IBond GetBondByKey(int Key);

        void SaveBond(IBond Bond);

        IReadOnlyEventList<IBond> GetBondByRegistrationNumber(string bondRegistrationNumber);

        //IBond GetBondByRegistrationNumberAndDeedsOfficeKey(string bondRegistrationNumber, int deedsOfficeKey);
        IBond GetBondByApplicationKey(int ApplicationKey);
    }
}