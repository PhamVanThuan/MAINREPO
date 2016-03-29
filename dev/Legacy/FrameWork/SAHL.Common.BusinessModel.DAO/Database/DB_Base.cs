using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;

//using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Interfaces;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.DAO.Database
{
    /// <summary>
    /// Base DAO class for all SAHL DAO objects, the lowest level before activerecord.
    /// </summary>
    /// <typeparam name="T">The DAO object type.</typeparam>
    public class DB_Base<T> : ActiveRecordValidationBase<T>, IBusinessModelContainer, IShallowCloneable, IProxyableDAOObject where T : class
    {
        private SAHLPrincipalCache _spc;

        #region Events

        /// <summary>
        /// Event that will get raised before an entity is persisted to the database.  If any other
        /// validation needs to be done prior to the write occurring, you should perform it using
        /// this event.
        /// </summary>
        public virtual event EventHandler<ValidateEventArgs> Validate;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the domain message collection off the current principal cache.
        /// </summary>
        private IDomainMessageCollection CurrentDomainMessages
        {
            get
            {
                if (_spc == null)
                    _spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                return _spc.DomainMessages;
            }
        }

        #endregion Properties

        #region Private Methods

        ///// <summary>
        ///// Used internally to ensure that BelongsTo properties marked as NotNull have a value when the entity
        ///// is validated.
        ///// </summary>
        ///// <param name="arm"></param>
        //private void CheckBelongsTo(ActiveRecordModel arm)
        //{
        //    foreach (BelongsToModel btm in arm.BelongsTo)
        //    {
        //        string btName = btm.Property.Name;
        //        object val = btm.Property.GetValue(this, null);

        //        if (btm.Property.CanWrite && btm.BelongsToAtt.NotNull && val == null)
        //            CurrentDomainMessages.Add(new Error(String.Format("{0} is a mandatory field.", InsertSpaces(btName)), ""));
        //    }

        //    // we need to check the paren't properties too
        //    if (arm.Parent != null)
        //        CheckBelongsTo(arm.Parent);

        //}

        ///// <summary>
        ///// Used internally to ensure that properties marked as NotNull have a value when the entity
        ///// is validated, and also that string properties described with a Length attribute are not
        ///// set with a value exceeding the maximum length.
        ///// </summary>
        ///// <param name="arm"></param>
        //private void CheckProperties(ActiveRecordModel arm)
        //{
        //    foreach (PropertyModel pm in arm.Properties)
        //    {
        //        string propName = pm.Property.Name;
        //        object val = pm.Property.GetValue(this, null);

        //        if (pm.PropertyAtt.NotNull && val == null)
        //            CurrentDomainMessages.Add(new Error(String.Format("{0} is a mandatory field.", InsertSpaces(propName)), ""));

        //        string sVal = val as String;
        //        if (sVal != null)
        //        {
        //            int maxLength = pm.PropertyAtt.Length;

        //            if (sVal.Length == 0 && pm.PropertyAtt.NotNull)
        //                CurrentDomainMessages.Add(new Error(String.Format("{0} is a mandatory field.", InsertSpaces(propName)), ""));

        //            if (maxLength > 0 && sVal.Length > maxLength)
        //                CurrentDomainMessages.Add(new Error(String.Format("{0} cannot exceed a length of {1}.", InsertSpaces(propName), maxLength), ""));
        //        }
        //    }

        //    // we need to check the paren't properties too
        //    if (arm.Parent != null)
        //        CheckProperties(arm.Parent);

        //}

        ///// <summary>
        ///// Checks to see that domain rules are not violated when ValidateEntity is called.  This does
        ///// things like ensure that NotNull properties have values, Length restrictions are not
        ///// exceeded, etc..
        ///// </summary>
        //private void CheckDomainStructure()
        //{
        //    ActiveRecordModel arm = ActiveRecordModel.GetModel(this.GetType());
        //    if (arm == null)
        //        throw new Exception(String.Format("Unable to retrieve ActiveRecordModel for type {0}", this.GetType().FullName));

        //    CheckBelongsTo(arm);
        //    CheckProperties(arm);
        //}

        ///// <summary>
        ///// Uses a regular expression to insert spaces in front of capital letters in a
        ///// word.  This will only insert spaces into the FIRST capital letter of a
        ///// sequence i.e. TheIdA will return as "The Id A", whereas "TheIDA" will
        ///// return as "The IDA".
        ///// </summary>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //private static string InsertSpaces(string expression)
        //{
        //    return Regex.Replace(expression, "([A-Z]+)", " $1", RegexOptions.Compiled).Trim();
        //}

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Creates a shallow copy of the object.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion Public Methods

        #region ActiveRecordBase Method Overrides

        /// <summary>
        /// Overridden to ensure our business rules are run.
        /// </summary>
        /// <returns>True if there are no domain errors, else false.</returns>
        public override bool IsValid()
        {
            return IsValid(RunWhen.Everytime);
        }

        /// <summary>
        /// Overridden to ensure our business rules are run.
        /// </summary>
        /// <param name="runWhen"></param>
        /// <returns>True if there are no domain errors, else false.</returns>
        public override bool IsValid(RunWhen runWhen)
        {
            // check domain structure to ensure all necessary values have been captured
            // CheckDomainStructure();

            // raise the Validate event - this will allow any subscribers to run their own rules
            if (Validate != null)
            {
                ValidateEventArgs args = new ValidateEventArgs();
                Validate(this, args);
            }

            // call base validation
            bool baseIsValid = base.IsValid(runWhen);
            // move any basic validation messages into the domain message collection and fail
            foreach (string error in this.ValidationErrorMessages)
            {
                CurrentDomainMessages.Add(new Error(error, error));
            }

            // the object is valid if it passes basic validation checks and the principal cache has no domain messages
            return (baseIsValid && CurrentDomainMessages.ErrorMessages.Count == 0);
        }

        /// <summary>
        /// Overidden to ensure that all error messages are moved into the principal cache domain message
        /// collection.  The default is to raise an exception, instead we raise a DomainValidationException.
        /// </summary>
        protected override void OnNotValid()
        {
            // don't throw an exception - if we do, we lose multiple failure messages bubbling up to the UI
            // GaryD: this has been added back to prevent TxnDispose errors.
            // This is a fundamental part of the application. Where multiple validation messages are required,
            // all required validation should be explicitly done before any transaction is created.
            // This should only be done if there are domain validation errors!
            if (CurrentDomainMessages.ErrorMessages.Count > 0)
            {
                foreach (IDomainMessage dm in CurrentDomainMessages.ErrorMessages)
                    throw new DomainValidationException(dm.Message);
            }
        }

        /// <summary>
        /// Create function not supported - use CreateAndFlush() instead.
        /// </summary>
        public override void Create()
        {
            throw new NotSupportedException("SAHL Domain does not support Create() due to validation - use CreateAndFlush() instead.");
        }

        /// <summary>
        /// Delete function not supported - use DeleteAndFlush() instead.
        /// </summary>
        public override void Delete()
        {
            throw new NotSupportedException("SAHL Domain does not support Delete() due to validation - use DeleteAndFlush() instead.");
        }

        /// <summary>
        /// Save function not supported - use SaveAndFlush() instead.
        /// </summary>
        public override void Save()
        {
            throw new NotSupportedException("SAHL Domain does not support Save() due to validation - use SaveAndFlush() instead.");
        }

        /// <summary>
        /// Update function not supported - use UpdateAndFlush() instead.
        /// </summary>
        public override void Update()
        {
            throw new NotSupportedException("SAHL Domain does not support Update() due to validation - use UpdateAndFlush() instead.");
        }

        #endregion ActiveRecordBase Method Overrides

        #region IBusinessModelContainer Members

        private IBusinessModelObject _BusinessModelObject;

        public virtual IBusinessModelObject BusinessModelObject
        {
            get
            {
                return _BusinessModelObject;
            }
            set
            {
                _BusinessModelObject = value;
            }
        }

        #endregion IBusinessModelContainer Members

        public virtual object ActualDAOObject
        {
            get { return this; }
        }

        public virtual U As<U>() where U : class
        {
            return this as U;
        }
    }

    public class ValidateEventArgs : EventArgs
    {
        bool _valid;

        public ValidateEventArgs()
        {
        }

        public ValidateEventArgs(bool valid)
        {
            _valid = valid;
        }

        public bool Valid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
        }
    }
}