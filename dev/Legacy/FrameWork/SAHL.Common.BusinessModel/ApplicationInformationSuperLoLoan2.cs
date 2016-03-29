using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationInformationSuperLoLoan : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationSuperLoLoan_DAO>, IApplicationInformationSuperLoLoan
    {
        public void Clone(IApplicationInformationSuperLoLoan obj)
        {
            obj.ElectionDate = this.ElectionDate;
            obj.PPThresholdYr1 = this.PPThresholdYr1;
            obj.PPThresholdYr2 = this.PPThresholdYr2;
            obj.PPThresholdYr3 = this.PPThresholdYr3;
            obj.PPThresholdYr4 = this.PPThresholdYr4;
            obj.PPThresholdYr5 = this.PPThresholdYr5;
        }
    }
}