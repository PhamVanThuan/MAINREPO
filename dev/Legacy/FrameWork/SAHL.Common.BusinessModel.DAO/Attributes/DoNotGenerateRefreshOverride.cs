using System;

namespace SAHL.Common.BusinessModel.DAO.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DoNotGenerateRefreshOverride : System.Attribute
    {
    }
}