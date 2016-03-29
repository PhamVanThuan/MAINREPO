// -----------------------------------------------------------------------
// <copyright file="IActiveRecordConfigurationProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SAHL.Common.BusinessModel.Interfaces.Configuration
{
    public interface IActiveRecordConfigurationProvider
    {
        string ConnectionStringFor2AMDB { get; }

        string ConnectionStringForX2DB { get; }

        string ConnectionStringForSAHLDB { get; }

        string ConnectionStringForWarehouseDB { get; }

        string ConnectionStringForDocumentManagementDB { get; }

        string ConnectionStringForImageIndexDB { get; }
    }
}