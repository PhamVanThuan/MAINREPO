using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.Interfaces.Validation;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Base
{
    public class BusinessModelBase<TDAO> : IEntityValidation, IEntityRuleConsumer, IBusinessModelObject, IDAOObject where TDAO : DB_Base<TDAO>
    {
        protected TDAO _DAO;
        private List<IEntityRuleProvider> _ruleProviders = new List<IEntityRuleProvider>();
        private List<string> _allRules = new List<string>();

        #region Constructor

        /// <summary>
        /// Base Business Model Object constructor.
        /// </summary>
        public BusinessModelBase(TDAO DAOObject)
        {
            _DAO = DAOObject;
            ((IBusinessModelContainer)_DAO).BusinessModelObject = this;
            _DAO.Validate += new EventHandler<ValidateEventArgs>(_DAO_Validate);
            // allow the objects overrides to populate their rules
            ExtendedConstructor();
            OnPopulateRules(_allRules);
        }

        private void _DAO_Validate(object sender, ValidateEventArgs e)
        {
            e.Valid = IsValid(_allRules);
        }

        #endregion Constructor

        #region Members

        [Obsolete("Please use method - SAHL.Common.BusinessModel.Helpers.Extensions.GetPreviousValue")]
        public object GetPreviousValue<InterfaceType, DAOType>(string PropertyName)
        {
            object obj = null;
            if (this._DAO is DB_AuditableBase<TDAO>)
            {
                obj = (this._DAO as DB_AuditableBase<TDAO>).GetPreviousValue(PropertyName);

                if (obj as SAHL.Common.Interfaces.IShallowCloneable != null)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

                    DAOType newobj = (DAOType)obj;

                    return BMTM.GetMappedType<InterfaceType, DAOType>(newobj);
                }
            }

            return obj;
        }

        /// <summary>
        /// Creates a shallow copy of the object.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            object daoClone = _DAO.Clone();
            return Activator.CreateInstance(this.GetType(), daoClone);
        }

        /// <summary>
        /// Refreshes the instance from the database.  Use with caution as this is only applicable
        /// to the DAO!  This should be overridden if necessary.
        /// </summary>
        public virtual void Refresh()
        {
            _DAO.Refresh();
        }

        /// <summary>
        /// Provides the opportunity to add constructor-level code.
        /// </summary>
        public virtual void ExtendedConstructor()
        {
        }

        /// <summary>
        /// Populates the object with the list of rules that apply to the business object.
        /// </summary>
        /// <param name="Rules"></param>
        public virtual void OnPopulateRules(List<string> Rules)
        {
        }

        /// <summary>
        /// Determines if this object is valid.  This must only validate the business model object itself (not the
        /// underlying DAO, as this can be called from the DAO or from the ValidateEntity method.
        /// </summary>
        /// <param name="rules">A list of rules to run.</param>
        /// <returns>True if there are no rules errors, else false.</returns>
        private bool IsValid(List<string> rules)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection dmc = spc.DomainMessages;

            int countBefore = dmc.Count;

            if (spc == null)
                throw new PrincipalNotCachedException();

            if (dmc == null)
                throw new MissingDomainMessageCollectionException();

            // run all database rules against the entity
            if (rules.Count > 0)
            {
                IRuleService rs = ServiceFactory.GetService<IRuleService>();
                rs.ExecuteRuleSet(dmc, rules, this);
            }

            return (dmc.Count == countBefore);
        }

        #endregion Members

        #region IDAOObject Members

        public object GetDAOObject()
        {
            return _DAO;
        }

        #endregion IDAOObject Members

        #region IEntityValidation Members

        /// <summary>
        /// Validates an entity against it's rules.  This can be run to determine if an entity is valid
        /// before attempting to write to the database.
        /// </summary>
        /// <returns>True if the entity validates, otherwise false.</returns>
        public bool ValidateEntity()
        {
            return (_DAO.IsValid() && this.IsValid(_allRules));
        }

        #endregion IEntityValidation Members

        #region IEntityRuleConsumer Members

        public void AddProvider(IEntityRuleProvider Provider)
        {
            _ruleProviders.Add(Provider);
            RefreshRules();
        }

        public void RemoveProvider(IEntityRuleProvider Provider)
        {
            _ruleProviders.Remove(Provider);
            RefreshRules();
        }

        protected void RefreshRules()
        {
            _allRules.Clear();
            OnPopulateRules(_allRules);
            // _allRules.AddRange(_DAO.EntityRules);
            for (int i = 0; i < _ruleProviders.Count; i++)
            {
                _allRules.AddRange(_ruleProviders[i].Rules);
            }
        }

        #endregion IEntityRuleConsumer Members
    }
}