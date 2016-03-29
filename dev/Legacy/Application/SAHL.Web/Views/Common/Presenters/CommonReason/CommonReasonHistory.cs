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
    public class CommonReasonHistory : SAHLCommonBasePresenter<INTUReasons>
    {

        protected CBOMenuNode _node;
        protected Int32 _genericKey;
        protected int _genericKeyTypeKey;

        private int _dcKey;
        private List<Int32> _pKeys = new List<Int32>();


        public CommonReasonHistory(INTUReasons view, SAHLCommonBaseController controller)
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
            _genericKeyTypeKey = _node.GenericKeyTypeKey;

            //setup the dependant keys from the generic key type
            switch (_genericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                    _dcKey = _genericKey;
                    foreach (IProposal p in DCRepo.GetDebtCounsellingByKey(_dcKey).Proposals)
                    {
                        _pKeys.Add(p.Key);
                    }
                    break;
                default:
                    break;
            }

            DataTable genericKeysAndTypes = new DataTable();
            genericKeysAndTypes.Columns.Add("genericKey");
            genericKeysAndTypes.Columns.Add("typeKey");


            string[] rtKeys = _view.ViewAttributes["reasontypekey"].Split(',');

            //bool dcDone = false;
            //bool pKeysDone = false;

            for (int i = 0; i < rtKeys.Length; i++)
            {
                IReasonType reasonType = ReasonRepo.GetReasonTypeByKey(Convert.ToInt32(rtKeys[i]));
                _genericKeyTypeKey = reasonType.GenericKeyType.Key;

                switch (_genericKeyTypeKey)
                {
                    case (int)GenericKeyTypes.DebtCounselling2AM:
                        //if (!dcDone)
                        //{
                        genericKeysAndTypes.Rows.Add(_dcKey, _genericKeyTypeKey);
                        //    dcDone = true;
                        //}
                        break;
                    case (int)GenericKeyTypes.Proposal:
                        //if (!pKeysDone)
                        //{
                        foreach (Int32 pKey in _pKeys)
                        {
                            genericKeysAndTypes.Rows.Add(pKey, _genericKeyTypeKey);

                        }
                        //    pKeysDone = true;
                        //}
                        break;
                    default:
                        break;
                }
            }

            _view.PanelHeader = "Debt Counselling Reason History";

            IEventList<IReason> reasons = ReasonRepo.GetReasonsByGenericKeyTypeAndKeys(genericKeysAndTypes);
            _view.BindgrdHistory(PopulateReasosnDS(reasons));
        }

        static DataTable PopulateReasosnDS(IEventList<IReason> reasons)
        {
            DataTable dtReasons = new DataTable();

            // Setup the Table 
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


        private IDebtCounsellingRepository _dcRepo;
        private IDebtCounsellingRepository DCRepo
        {
            get
            {
                if (_dcRepo == null)
                    _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _dcRepo;
            }
        }

        private IReasonRepository _rRepo;
        private IReasonRepository ReasonRepo
        {
            get
            {
                if (_rRepo == null)
                    _rRepo = RepositoryFactory.GetRepository<IReasonRepository>();

                return _rRepo;
            }
        }
    }
}
