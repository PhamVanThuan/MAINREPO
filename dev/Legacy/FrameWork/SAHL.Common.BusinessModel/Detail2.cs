using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Detail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Detail_DAO>, IDetail
	{
        /// <summary>
        /// 
        /// </summary>
        public DateTime DetailDate
        {
            get { return _DAO.DetailDate; }
            set 
            { 
                if (CanUpdate)
                    _DAO.DetailDate = value; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Double? Amount
        {
            get { return _DAO.Amount; }
            set 
            { 
                if (CanUpdate)
                    _DAO.Amount = value; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set 
            { 
                if (CanUpdate)
                    _DAO.Description = value; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? LinkID
        {
            get { return _DAO.LinkID; }
            set 
            { 
                if (CanUpdate)
                    _DAO.LinkID = value; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set 
            { 
                if (CanUpdate)
                    _DAO.UserID = value; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set 
            { 
                if (CanUpdate)
                    _DAO.ChangeDate = value; 
            }
        }		/// <summary>
        /// 
        /// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Account) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
                }
            }

            set
            {
                if (CanUpdate)
                {
                    if (value == null)
                    {
                        _DAO.Account = null;
                        return;
                    }
                    IDAOObject obj = value as IDAOObject;

                    if (obj != null)
                        _DAO.Account = (Account_DAO)obj.GetDAOObject();
                    else
                        throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public IDetailType DetailType
        {
            get
            {
                if (null == _DAO.DetailType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDetailType, DetailType_DAO>(_DAO.DetailType);
                }
            }

            set
            {
                // updates to this value can only occur for adding
                if (Key <= 0)
                {
                    if (value == null)
                    {
                        _DAO.DetailType = null;
                        return;
                    }
                    IDAOObject obj = value as IDAOObject;

                    if (obj != null)
                        _DAO.DetailType = (DetailType_DAO)obj.GetDAOObject();
                    else
                        throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
                }
                else
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    IDomainMessageCollection dmc = spc.DomainMessages;
                    dmc.Add(new Error("Detail type cannot be updated on existing detail records.", ""));
                }

            }
        }

        /// <summary>
        /// Determines if the detail can be updated or not.
        /// </summary>
        private bool CanUpdate
        {
            get
            {
                // if it's a new object, exit immediately
                if (this.Key <= 0)
                    return true;

                bool canUpdate = true;

                if (this.DetailType != null)
                {
                    // if the detail type is not active, updates not allowed
                    if (this.DetailType.GeneralStatus != null && DetailType.GeneralStatus.Key != (int)GeneralStatuses.Active)
                        canUpdate = false;
                    else if (!this.DetailType.AllowUpdate)
                        canUpdate = false;
                }

                if (!canUpdate)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    IDomainMessageCollection dmc = spc.DomainMessages;
                    dmc.Add(new Error("Details cannot be updated when the detail type is not active or the detail type does not allow updates.", ""));
                }

                return canUpdate;
            }

        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("DetailsMandatoryDate");
            Rules.Add("DetailsPositiveLoanBalanceHOC");
        }
	}
}


