using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Interfaces
{
    /// <summary>
    /// Batch Progress Interface
    /// </summary>
    public interface IBatchProgress : IViewBase
    {

        #region Properties

        int? BulkBatchTypeKey { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Bind BatchGrid
        /// </summary>
        /// <param name="bulkTransactions"></param>
        void BindBatchGrid(IList<IBulkBatch> bulkTransactions);
        #endregion

        #region EventHandlers
        /// <summary>
        /// Event Handler for BatchGrid Selected Index Change
        /// </summary>
        event KeyChangedEventHandler OnBatchGridSelectedIndexChanged;
        /// <summary>
        /// Event Handler for Batch Type Selected Index Change
        /// </summary>
        event KeyChangedEventHandler OnBatchTypeListSelectedIndexChange;
        /// <summary>
        /// Event Handler for Message Type Selected Index Change
        /// </summary>
        event KeyChangedEventHandler OnMessageTypeListSelectedIndexChange;


        #endregion

        /// <summary>
        /// Bind Look Ups
        /// </summary>
        /// <param name="bulkBatchTypes"></param>
        /// <param name="messageType"></param>
        void BindLookUps(IEventList<IBulkBatchType> bulkBatchTypes, IEventList<IMessageType> messageType);
        /// <summary>
        /// Bind Message Grid
        /// </summary>
        /// <param name="bulkBatchLog"></param>
        void BindMessageGrid(IList<IBulkBatchLog> bulkBatchLog);
        /// <summary>
        /// Bind BatchGridFields
        /// </summary>
        /// <param name="bulkBatch"></param>
        void BindBatchGridFields(IBulkBatch bulkBatch);
        /// <summary>
        /// Selected Index on Grid
        /// </summary>
        int GetSelectedIndexOnGrid { get; }
        /// <summary>
        /// Message Type
        /// </summary>
        int GetSelectedMessageType { get;}
    }
}
