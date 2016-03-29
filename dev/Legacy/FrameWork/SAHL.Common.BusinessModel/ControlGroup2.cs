using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// The Control Group DAO Object specifies what type of control is being used.
    /// </summary>
    public partial class ControlGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ControlGroup_DAO>, IControlGroup
    {
    }
}