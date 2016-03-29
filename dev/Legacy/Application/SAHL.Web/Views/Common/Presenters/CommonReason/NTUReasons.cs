using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    public class NTUReasonsPresenter : SAHLCommonBasePresenter<INTUReasons>
    {
        int _genericKey;
        int _genericKeyTypeKey;
        protected CBOMenuNode _node;


        public NTUReasonsPresenter(INTUReasons view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// OnViewInitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // Get the CBO Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKey = _node.GenericKey;

            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            IReasonRepository reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

            int reasonTypeKey = Convert.ToInt32(_view.ViewAttributes["reasontypekey"]);

            IReasonType reasonType = reasonRepo.GetReasonTypeByKey(reasonTypeKey);
            _genericKeyTypeKey = reasonType.GenericKeyType.Key;

            switch (_genericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.OfferInformation:
                    IApplicationInformation latestApplicationInformation = RepositoryFactory.GetRepository<IApplicationRepository>().GetApplicationByKey(_genericKey).GetLatestApplicationInformation();
                    _genericKey = latestApplicationInformation.Key;
                    break;
                default:
                    break;
            }

            DataTable genericKeysAndTypes = new DataTable();
            genericKeysAndTypes.Columns.Add("genericKey");
            genericKeysAndTypes.Columns.Add("typeKey");

            genericKeysAndTypes.Rows.Add(_genericKey, _genericKeyTypeKey);

            if (_genericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
            {
                IEventList<IApplicationInformation> informations = applicationRepository.GetApplicationRevisionHistory(_genericKey);
                foreach (IApplicationInformation appInfo in informations)
                {
                    genericKeysAndTypes.Rows.Add(appInfo.Key, (int)SAHL.Common.Globals.GenericKeyTypes.OfferInformation);
                }
            }

            _view.PanelHeader = "Application Reason History";
            IEventList<IReason> reasons = reasonRepo.GetReasonsByGenericKeyTypeAndKeys(genericKeysAndTypes);
            _view.BindgrdHistory(PopulateReasosnDS(reasons));
        }


        static DataTable PopulateReasosnDS(IEventList<IReason> reasons)
        {

            DataTable dtReasons = new DataTable();


            // Setup the Valuations Table 
            dtReasons.Reset();
            dtReasons.Columns.Add("BusinessArea");
            dtReasons.Columns.Add("Reason");
            dtReasons.Columns.Add("Comment");
            dtReasons.Columns.Add("Date");
            dtReasons.Columns.Add("User");
            dtReasons.TableName = "Reasons";


            if (reasons.Count > 0)
                for (int i = 0; i < reasons.Count; i++)
                {
                    DataRow valRow = dtReasons.NewRow();
                    valRow["BusinessArea"] = reasons[i].ReasonDefinition.ReasonType.Description;
                    valRow["Reason"] = reasons[i].ReasonDefinition.ReasonDescription.Description;
                    valRow["Comment"] = reasons[i].Comment;
                    if (reasons[i].StageTransition != null) valRow["Date"] = reasons[i].StageTransition.TransitionDate.ToString(SAHL.Common.Constants.DateFormat);
                    if (reasons[i].StageTransition != null) valRow["User"] = reasons[i].StageTransition.ADUser.ADUserName;
                    dtReasons.Rows.Add(valRow);
                }


            return dtReasons;

        }



    }
}
