using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel
{
    public abstract class ApplicationProduct : IApplicationProduct, IEntityRuleProvider
    {
        List<string> _rules = new List<string>();
        IApplication _application;
        protected Application_DAO _appDAO;
        protected ApplicationInformation_DAO _appInfoDAOPrevious;
        protected ApplicationInformation_DAO _appInfoDAO;
        protected bool clone;

        public ApplicationProduct(IApplication Application, bool CreateNew)
        {
            _application = Application;

            IDAOObject AppDAOObj = _application as IDAOObject;
            if (null != AppDAOObj)
                _appDAO = AppDAOObj.GetDAOObject() as Application_DAO;

            if (CreateNew)
            {
                _appInfoDAOPrevious = GetLatestApplicationInformation();
                _appInfoDAO = new ApplicationInformation_DAO();
                ApplicationInformationType_DAO appType = null;
                if (null == _appDAO.ApplicationInformations || _appDAO.ApplicationInformations.Count == 0)
                {
                    ApplicationInformationType_DAO[] arr = ApplicationInformationType_DAO.FindAllByProperty("Description", "Original Offer");
                    appType = arr[0];
                    clone = false;
                }
                else
                {
                    ApplicationInformationType_DAO[] arr = ApplicationInformationType_DAO.FindAllByProperty("Description", "Revised Offer");
                    appType = arr[0];
                    clone = true;
                }
                _appInfoDAO.ApplicationInformationType = appType;
                _appInfoDAO.ApplicationInsertDate = DateTime.Now;
                _appInfoDAO.Application = _appDAO;
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                IDomainMessageCollection Messages = spc.DomainMessages;
                _application.ApplicationInformations.Add(Messages, new ApplicationInformation(_appInfoDAO));

                // check if there is data
                if (clone && (null != _appInfoDAOPrevious))
                {
                    // copy the appinfo from the previous applicaiton info
                    _appInfoDAO.ApplicationInsertDate = _appInfoDAOPrevious.ApplicationInsertDate;
                    _appInfoDAO.Product = _appInfoDAOPrevious.Product;
                }
            }
            else
            {
                // go find the latest applicaitoninformation (of this type)
                GetLatestApplicationInformation();
            }

            OnPopulateRules(_rules);

            IEntityRuleConsumer ERC = _application as IEntityRuleConsumer;
            ERC.AddProvider(this);
        }

        public ApplicationProduct(IApplicationInformation ApplicationInformation)
        {
            _application = ApplicationInformation.Application as Application;

            IDAOObject AppDAOObj = _application as IDAOObject;
            if (null != AppDAOObj)
                _appDAO = AppDAOObj.GetDAOObject() as Application_DAO;

            // go find the latest applicaitoninformation (of this type)
            //GetLatestApplicationInformation();
            IDAOObject AppInfoDAOObj = ApplicationInformation as IDAOObject;
            if (AppInfoDAOObj != null)
            {
                _appInfoDAO = AppInfoDAOObj.GetDAOObject() as ApplicationInformation_DAO;
            }

            OnPopulateRules(_rules);

            IEntityRuleConsumer ERC = _application as IEntityRuleConsumer;
            ERC.AddProvider(this);
        }

        protected virtual void OnPopulateRules(List<string> Rules)
        {
        }

        #region IApplicationProduct Members

        public IApplication Application
        {
            get
            {
                return _application;
            }
        }

        public virtual void DisposeProduct()
        {
            IEntityRuleConsumer ERC = _application as IEntityRuleConsumer;
            ERC.RemoveProvider(this);

            _rules.Clear();
            _rules = null;
            _appDAO = null;
            _appInfoDAO = null;
            _application = null;
        }

        public SAHL.Common.Globals.Products ProductType
        {
            get
            {
                string s = Enum.GetName(typeof(SAHL.Common.Globals.Products), _appInfoDAO.Product.Key);
                object o = Enum.Parse(typeof(SAHL.Common.Globals.Products), s);
                //object o = Enum.Parse(typeof(SAHL.Common.Globals.Products), _appInfoDAO.Product.Description);
                return (SAHL.Common.Globals.Products)o;
            }
        }

        #endregion IApplicationProduct Members

        protected ApplicationInformation_DAO GetLatestApplicationInformation()
        {
            IApplicationInformation AppInfo = Application.GetLatestApplicationInformation();
            if (null == AppInfo)
            {
                //throw new Exception(string.Format("Product cannot be initialised, missing data. Application No.: {0}", Application.Key));
                return null;
            }
            IDAOObject AppInfoDAOObj = AppInfo as IDAOObject;
            if (AppInfoDAOObj != null)
            {
                _appInfoDAO = AppInfoDAOObj.GetDAOObject() as ApplicationInformation_DAO;
            }
            return _appInfoDAO;
        }

        #region IEntityRuleProvider Members

        public List<string> Rules
        {
            get { return _rules; }
        }

        #endregion IEntityRuleProvider Members
    }
}