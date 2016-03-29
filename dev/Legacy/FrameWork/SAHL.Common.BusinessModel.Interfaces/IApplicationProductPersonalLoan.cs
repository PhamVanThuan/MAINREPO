namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IApplicationProductPersonalLoan : IApplicationProduct
    {
        IApplicationInformationPersonalLoan ApplicationInformationPersonalLoan { get; }
        IExternalLifePolicy ExternalLifePolicy { get; set; }
    }
}