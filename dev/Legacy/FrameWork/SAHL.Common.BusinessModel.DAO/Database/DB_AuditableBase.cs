//using SAHL.Common.Service.Interfaces;
using System.Collections;
using SAHL.Common.Factories;
using SAHL.Common.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.DAO.Database
{
    /// <summary>
    /// Base class for any entity that needs to be audited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DB_AuditableBase<T> : DB_Base<T> where T : class
    {
        private IDictionary _previousState;
        private IDictionary _currentState;
        private IAuditsAndMetricsService _auditService;

        // _saved keeps track of whether an item has been written to the database - this is done by
        // overriding the validation routine as this is only ever called once, and then doing the actual
        // write after a flush
        private bool _auditRequired = false;

        // keeps track of whether an item has been deleted and an audit record written - this is necessary
        // as the Delete overrides are run differently depending on whether we're cascading or not - and in
        // some instances (but not all) more than one of the Delete overrides is called by ActiveRecord
        private bool _auditDelete = false;

        private IAuditsAndMetricsService AuditService
        {
            get
            {
                if (_auditService == null)
                    _auditService = ServiceFactory.GetService<IAuditsAndMetricsService>();
                return _auditService;
            }
        }

        protected override bool BeforeLoad(object id, IDictionary adapter)
        {
            _previousState = adapter;
            return base.BeforeLoad(id, adapter);
        }

        protected override int[] FindDirty(object id, IDictionary previousState, IDictionary currentState, NHibernate.Type.IType[] types)
        {
            _currentState = currentState;
            int[] result = base.FindDirty(id, previousState, currentState, types);

            //// this is the last stage at which an event occurs when deleting from a child object, so do the auditting
            //// now
            //if (_deleted)
            //{
            //    IAuditsAndMetricsService AMS = ServiceFactory.GetService<IAuditsAndMetricsService>();
            //    AMS.StoreAudit(this, _previousState, null);
            //    _deleted = false;
            //}

            return result;
        }

        protected override bool OnFlushDirty(object id, IDictionary previousState, IDictionary currentState, NHibernate.Type.IType[] types)
        {
            _currentState = currentState;
            return base.OnFlushDirty(id, previousState, currentState, types);
        }

        protected override void PostFlush()
        {
            base.PostFlush();

            // save the audit record - this is done as late as possible otherwise for new objects there is no
            // key to work with (object still has a key of 0)
            if (_auditRequired)
            {
                //Removed for now so that we can run load save loads
                AuditService.StoreAudit(this, _previousState, _currentState);
                _auditRequired = false;
            }
        }

        public virtual object GetPreviousValue(string PropertyName)
        {
            if (_previousState == null)
                return null;

            object prevval = _previousState[PropertyName];

            if (prevval == null)
                return prevval;

            //return prevval;
            if (prevval as IShallowCloneable == null)
            {
                return prevval;
            }

            return ((IShallowCloneable)prevval).Clone();
        }

        public override bool IsValid(Castle.Components.Validator.RunWhen runWhen)
        {
            bool isValid = base.IsValid(runWhen);
            _auditRequired = isValid;
            return isValid;
        }

        public override bool IsValid()
        {
            bool isValid = base.IsValid();
            _auditRequired = isValid;
            return isValid;
        }

        protected override void OnDelete()
        {
            base.OnDelete();
            if (!_auditDelete)
            {
                AuditService.StoreAudit(this, _previousState, null);
                _auditDelete = true;
            }
        }

        public override void Delete()
        {
            base.DeleteAndFlush();  // this is deliberate!  we want to force the flush for validation of cascade objects
            if (!_auditDelete)
            {
                AuditService.StoreAudit(this, _previousState, null);
                _auditDelete = true;
            }
        }

        public override void DeleteAndFlush()
        {
            base.DeleteAndFlush();
            if (!_auditDelete)
            {
                AuditService.StoreAudit(this, _previousState, null);
                _auditDelete = true;
            }
        }
    }
}