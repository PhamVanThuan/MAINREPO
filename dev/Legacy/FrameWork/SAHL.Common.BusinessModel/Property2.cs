using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord.Queries;
using SAHL.Common.CacheData;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Property : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Property_DAO>, IProperty
	{
        public IPropertyAccessDetails PropertyAccessDetails
        {
            get
            {
                if (_DAO.PropertyAccessDetails != null && _DAO.PropertyAccessDetails.Count > 0)
                {
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return bmtm.GetMappedType<IPropertyAccessDetails, PropertyAccessDetails_DAO>(_DAO.PropertyAccessDetails[0]);
                }

                return null;
            }
            set
            {
                if (_DAO.PropertyAccessDetails == null)
                    _DAO.PropertyAccessDetails = new List<PropertyAccessDetails_DAO>();

                IList<PropertyAccessDetails_DAO> list = _DAO.PropertyAccessDetails as IList<PropertyAccessDetails_DAO>;
                if (list.Count > 0)
                    list.Clear();

                if (value != null)
                {
                    IDAOObject obj = value as IDAOObject;
                    if (obj != null)
                        list.Add((PropertyAccessDetails_DAO)obj.GetDAOObject());
                    else
                        throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
                }
                else
                    _DAO.PropertyAccessDetails = null;
            }
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("PropertyAdCheckDataProvider");
            // mandatory fields
            Rules.Add("PropertyTypeMandatory");
            Rules.Add("PropertyTitleTypeMandatory");
            Rules.Add("PropertyOccupancyTypeMandatory");
            Rules.Add("PropertyAreaClassificationMandatory");
            Rules.Add("PropertyDeedsPropertyTypeMandatory");
            Rules.Add("PropertyDescription1Mandatory");
            Rules.Add("PropertyDescription2Mandatory");
            Rules.Add("PropertyDescription3Mandatory");           
        }

        public bool CanPerformPropertyAdCheckValuation()
        {
            bool retVal = true;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            for (int i = 0; i < this.Valuations.Count; i++)
            {
                if ((this.Valuations[i] is IValuationDiscriminatedAdCheckDesktop || this.Valuations[i] is IValuationDiscriminatedAdCheckPhysical) &&  this.Valuations[i].ValuationStatus.Key == (int)ValuationStatuses.Pending)
                {
                    spc.DomainMessages.Add(new Error("Can not perform another AdCheck Valuation while one is still Pending", ""));
                    retVal = false;
                }
            }
            return retVal;
        }

        public bool CanPerformPropertyLightStoneValuation()
        {
            bool retVal = true;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            for (int i = 0; i < this.Valuations.Count; i++)
            {
                if (this.Valuations[i] is IValuationDiscriminatedLightstoneAVM && this.Valuations[i].ValuationStatus.Key == (int)ValuationStatuses.Pending)
                {
                    spc.DomainMessages.Add(new Error("Can not perform another LightStone Valuation while one is still Pending", ""));
                    retVal = false;
                }
            }
            return retVal;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="Item"></param>
		protected void OnMortgageLoanProperties_BeforeAdd(ICancelDomainArgs args, object Item)
		{
            bool propertyLinkedToOpenAccount = PropertyAlreadyExistsOnOpenAccount(this.PropertyDescription1, this.PropertyDescription2, this.PropertyDescription3);
            if (propertyLinkedToOpenAccount)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                string errorMessage = "This property already exists on the Database and may not be added.";
                spc.DomainMessages.Add(new Error(errorMessage, errorMessage));
            }

		}

        public bool PropertyAlreadyExistsOnOpenAccount(string desc1, string desc2, string desc3)
        {
            bool propertyLinkedToOpenAccount = false;

            string HQL = "from Property_DAO P where P.PropertyDescription1= ? and P.PropertyDescription2=? and P.PropertyDescription3=? ";

            SimpleQuery<Property_DAO> q = new SimpleQuery<Property_DAO>(HQL, desc1, desc2, desc3);
            Property_DAO[] res = q.Execute();
            IEventList<IProperty> list = new DAOEventList<Property_DAO, IProperty, Property>(res);
            IEventList<IProperty> props = (list); ;
            if (props != null && props.Count > 0)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    for (int x = 0; x < props[i].MortgageLoanProperties.Count; x++)
                    {
                        if (props[i].MortgageLoanProperties[x].Account.AccountStatus.Key == (int)AccountStatuses.Open)
                        {
                            propertyLinkedToOpenAccount = true;
                            break;
                        }
                    }
                }
            }
            return propertyLinkedToOpenAccount;
        }

        /// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnMortgageLoanProperties_BeforeRemove(ICancelDomainArgs args, object Item)
		{
           
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnPropertyAccountDebtSettlements_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnPropertyAccountDebtSettlements_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnPropertyTitleDeeds_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnPropertyTitleDeeds_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnValuations_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnValuations_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnAccountProperties_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnAccountProperties_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnMortgageLoanProperties_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnMortgageLoanProperties_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnPropertyAccountDebtSettlements_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnPropertyAccountDebtSettlements_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnPropertyTitleDeeds_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnPropertyTitleDeeds_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnValuations_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnValuations_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnAccountProperties_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnAccountProperties_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnPropertyDatas_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnPropertyDatas_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnPropertyDatas_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="Item"></param>
        protected void OnPropertyDatas_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }



        /// <summary>
        ///returns the latest complete valuation or null if none found.
        /// </summary>
        public IValuation LatestCompleteValuation
        {
            get
            {
                IValuation latestValuation = null;
                DateTime latestDate = new DateTime(1900, 1, 1);

                foreach (IValuation valuation in this.Valuations)
                {
                    if (valuation.ValuationStatus.Key == (int)ValuationStatuses.Complete)
                    {
                        if (valuation.IsActive)
                            return valuation;
                        else if (valuation.ValuationDate > latestDate)
                        {
                            latestDate = (DateTime)valuation.ValuationDate;
                            latestValuation = valuation;
                        }
                    }
                }
                return latestValuation;
            }
        
        }
    }
}


