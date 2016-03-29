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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITest : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnLegalEntityGridSelectedIndexChanged;
        event KeyChangedEventHandler OnLegalEntityGridNewSelectedIndexChanged;

        /// <summary>
        /// Binds the Legal Entitity Grid data
        /// </summary>
        /// <param name="lstNaturalPersons"></param>
        void BindLegalEntityGrid(IReadOnlyEventList<ILegalEntityNaturalPerson> lstNaturalPersons);

        ///// <summary>
        ///// Binds the Legal Entitity Grid data to the New Grid
        ///// </summary>
        ///// <param name="legalEntityGridItems"></param>
        //void BindLegalEntityGridNew(IList<SAHL.Web.Controls.LegalEntityGridItem> legalEntityGridItems);

        /// <summary>
        /// Binds the Legal Entitity Grid data to the New Grid
        /// </summary>
        /// <param name="legalEntities"></param>
        void BindLegalEntityGridNew(IReadOnlyEventList<ILegalEntity> legalEntities);
    }
}
