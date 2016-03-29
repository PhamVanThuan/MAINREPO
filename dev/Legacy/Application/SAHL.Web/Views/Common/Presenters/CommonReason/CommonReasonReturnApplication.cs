using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    //public class CommonReasonReturnApplication : CommonReasonBase
    //{

    //    public CommonReasonReturnApplication(ICommonReason view, SAHLCommonBaseController controller)
    //        : base(view, controller)
    //    {
    //    }

    //    protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
    //    {
    //       _selectedReasons = (List<SelectedReason>)e.Key;
    //       IReasonRepository reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
    //       IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
    //       IApplication app = appRepo.GetApplicationByKey(base.GenericKey);
    //       //IApplicationFurtherLoan appFurther = app as IApplicationFurtherLoan;

    //        if (_selectedReasons.Count > 0) // Only call Save if Reasons have been selected
    //        {
    //            SAHL.Common.BusinessModel.Interfaces.IMemo memo = null;
    //            IMemoRepository _memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
    //            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
    //            IOrganisationStructureRepository OSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
    //            IADUser adUser = OSR.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);

    //            for (int x = 0; x < _selectedReasons.Count; x++)
    //            {
    //                if (_selectedReasons[x].Comment.Length > 0)
    //                {
    //                        memo = _memoRepository.CreateMemo();
    //                        memo.GenericKey = base.GenericKey; 
    //                        if (_node != null)
    //                            memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[((int)_node.GenericKeyTypeKey).ToString()];
    //                        else
    //                            memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Offer).ToString()];
    //                        memo.Description = _selectedReasons[x].Comment;
    //                        memo.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
    //                        memo.InsertedDate = DateTime.Now.Date;
    //                        memo.ADUser = adUser;
    //                  }

    //                TransactionScope txn = new TransactionScope();

    //                try
    //                {
    //                    if (_selectedReasons[x].Comment.Length > 0)
    //                        _memoRepository.SaveMemo(memo);

    //                    IReason res = reasonRepo.CreateEmptyReason();

    //                    res.Comment = _selectedReasons[x].Comment;
    //                    res.GenericKey = base.GenericKey;
    //                    res.ReasonDefinition = reasonRepo.GetReasonDefinitionByKey(_selectedReasons[x].ReasonDefinitionKey);
    //                    reasonRepo.SaveReason(res);
    //                    if (app != null)
    //                    {
    //                        //BADNESS!! this should not be called ever.

    //                        //IDisbursementRepository disbRepo = RepositoryFactory.GetRepository<IDisbursementRepository>();
    //                        //disbRepo.RollbackDisbursedLoan(app.Account.Key);
    //                    }
                    
    //                }

    //                catch (Exception)
    //                {
    //                    txn.VoteRollBack();
    //                    if (_view.IsValid)
    //                        throw;
    //                }

    //                finally
    //                {
    //                    txn.Dispose();
    //                }

    //                if (_view.IsValid)
    //                    CompleteActivityAndNavigate();
    //            }
    //        }
    //    }

    //    public override void CancelActivity()
    //    {
    //        base.X2Service.CancelActivity(_view.CurrentPrincipal);
    //        base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
    //    }

    //    public override void CompleteActivityAndNavigate()
    //    {
    //        X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
    //        if (base.sdsdgKeys.Count > 0)
    //        {
    //            UpdateReasonsWithStageTransitionKey();
    //        } 
    //        base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
    //    }
    //}
}
