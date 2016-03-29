using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent an Unsecured Lending Application.
    /// DiscriminatorValue = "11"
    /// </summary>
    [ActiveRecord("Offer", Schema = "dbo", DiscriminatorValue = "11", Lazy = true)]
    [ConstructorInjector]
    public class ApplicationUnsecuredLending_DAO : Application_DAO
    {
    }
}