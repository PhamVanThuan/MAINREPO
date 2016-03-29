using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using System.Data;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface INoteSummary : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        int GenericKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int GenericKeyTypeKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTime DiaryDate { set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noteDetails"></param>
        void BindNotesGrid(List<INoteDetail> noteDetails);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legalEntities"></param>
        void BindLegalEntityFilter(List<ILegalEntity> legalEntities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniqueDates"></param>
        void BindDateFilters(Dictionary<string, string> uniqueDates);
    }
}
