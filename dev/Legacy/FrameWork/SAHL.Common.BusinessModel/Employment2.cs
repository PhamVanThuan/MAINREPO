using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.Security;
using SAHL.Common.DomainMessages;
namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Employment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Employment_DAO>, IEmployment
    {

        private IExtendedEmployment _extendedEmployment;
        private double _previousConfirmedBasicIncome;
        private string _previousContactPerson;
        private string _previousPhoneNumber;
        private string _previousPhoneCode;
        private string _previousDepartment;


        public override void ExtendedConstructor()
        {
            base.ExtendedConstructor();
            _previousConfirmedBasicIncome = (this.ConfirmedBasicIncome ?? 0D);
            _previousContactPerson = (this.ContactPerson);
            _previousPhoneNumber = (this.ContactPhoneNumber);
            _previousPhoneCode = (this.ContactPhoneCode);
            _previousDepartment = (this.Department);

        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("EmploymentCurrentStatusEndDateInvalid");
            Rules.Add("EmploymentStatusAddCurrent");
            Rules.Add("EmploymentMonthlyIncomeMinimum");
            //Rules.Add("EmploymentConfirmedIncomeMinimum");
            Rules.Add("EmploymentEmployerMandatory");
            Rules.Add("EmploymentStartDateMinimum");
            Rules.Add("EmploymentStartDateMaximum");
            Rules.Add("EmploymentLegalEntityCompanyRemunerationTypes");
            Rules.Add("EmploymentSupportedRemunerationTypes");
            Rules.Add("EmploymentEndDateMinimum");
            Rules.Add("EmploymentEndDateMaximum");
            Rules.Add("EmploymentPreviousStatusEndDateMandatory");
            Rules.Add("EmploymentPreviousPTICheck");
            Rules.Add("EmploymentRemunerationCommissionMandatory");
            //Rules.Add("EmploymentSalaryPayDayMandatory");

            // NOTE: these rules can't be run as old employment records don't necessarily have this data and there's no 
            // way of working out when they apply
            //Rules.Add("EmploymentMandatoryConfirmedBy");
            //Rules.Add("EmploymentMandatoryConfirmedDate");
        }

        #region Properties

        /// <summary>
        /// Used to determine if the confirmed income amount has changed.
        /// </summary>
        public bool ConfirmedIncomeChanged
        {
            get
            {
                return ((ConfirmedBasicIncome ?? 0D) != _previousConfirmedBasicIncome);
            }
        }

        public bool ContactPersonChanged
        {
            get
            {
                return ((ContactPerson) != _previousContactPerson);
            }
        }

        public bool ContactPhoneNumberChanged
        {
            get
            {
                return ((ContactPhoneNumber) != _previousPhoneNumber);
            }
        }



        public bool ContactPhoneCodeChanged
        {
            get
            {
                return ((ContactPhoneCode) != _previousPhoneCode);
            }
        }



        public bool DepartmentChanged
        {
            get
            {
                return ((Department) != _previousDepartment);
            }
        }



        /// <summary>
        /// Gets/sets the confirmed basic income amount.  If extended employment details are required, this will 
        /// get/set the <see cref="IEmployment.ConfirmedBasicIncome"/> value.
        /// </summary>
        public virtual double? ConfirmedBasicIncome
        {
            get
            {
                return _DAO.ConfirmedBasicIncome;
            }
            set
            {
                _DAO.ConfirmedBasicIncome = value;
            }
        }

        /// <summary>
        /// Declared as virtul so this can be overridden.
        /// </summary>
        public virtual IEmploymentStatus EmploymentStatus
        {
            get
            {
                if (null == _DAO.EmploymentStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IEmploymentStatus, EmploymentStatus_DAO>(_DAO.EmploymentStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.EmploymentStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.EmploymentStatus = (EmploymentStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Implements <see cref="IEmployment.EmploymentType"/>.
        /// </summary>
        public virtual IEmploymentType EmploymentType
        {
            get
            {
                // for new objects, the DAO value will be null, so just return a value depending on the 
                // discriminated type
                if (null == _DAO.EmploymentType)
                {
                    ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                    if (this is IEmploymentSalaried)
                        return lookups.EmploymentTypes.ObjectDictionary[((int)EmploymentTypes.Salaried).ToString()];
                    else if (this is IEmploymentSelfEmployed)
                        return lookups.EmploymentTypes.ObjectDictionary[((int)EmploymentTypes.SelfEmployed).ToString()];
                    else if (this is IEmploymentSubsidised)
                        return lookups.EmploymentTypes.ObjectDictionary[((int)EmploymentTypes.SalariedwithDeduction).ToString()];
                    else if (this is IEmploymentUnemployed)
                        return lookups.EmploymentTypes.ObjectDictionary[((int)EmploymentTypes.Unemployed).ToString()];
                    else
                        throw new NotSupportedException("Unsupported employment type");
                }

                // item is loaded, return type from DAO
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IEmploymentType, EmploymentType_DAO>(_DAO.EmploymentType);
            }
        }

        public virtual IExtendedEmployment ExtendedEmployment
        {
            get
            {
                if (_extendedEmployment == null)
                    RefreshExtendedEmployment();

                return _extendedEmployment;
            }
        }

        /// <summary>
        /// Implements <see cref="IEmployment.IsConfirmed"/>.
        /// </summary>
        public virtual bool IsConfirmed
        {
            get
            {
                return ConfirmedBasicIncome.HasValue;
            }
        }

        /// <summary>
        /// Gets/sets the basic income.  If there are extended employment details, this will get/set the <see cref="IEmployment.BasicIncome"/> 
        /// value.
        /// </summary>
        public virtual double? BasicIncome
        {
            get
            {
                return _DAO.BasicIncome;
            }
            set
            {
                _DAO.BasicIncome = value;
            }
        }

        /// <summary>
        /// Gets/sets the remuneration type internally for employment objects.  
        /// </summary>
        public virtual IRemunerationType RemunerationType
        {
            get
            {
                if (null == _DAO.RemunerationType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IRemunerationType, RemunerationType_DAO>(_DAO.RemunerationType);
                }
            }
            set
            {
                if (value == null)
                {
                    _DAO.RemunerationType = null;
                    return;
                }

                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.RemunerationType = (RemunerationType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");

                RefreshExtendedEmployment();
            }

        }

        /// <summary>
        /// Implements <see cref="IEmployment.RequiresExtended"/>.
        /// </summary>
        public virtual bool RequiresExtended
        {
            get
            {
                return (ExtendedEmployment != null);
            }
        }

        /// <summary>
        /// Determines which remuneration types are supported by the employment type.  
        /// </summary>
        public virtual IReadOnlyEventList<RemunerationTypes> SupportedRemunerationTypes
        {
            get
            {
                return new ReadOnlyEventList<RemunerationTypes>();
            }
        }

        /// <summary>
        /// Gets the total unconfirmed monthly income for the employment object (this will take extended employment 
        /// information into account).
        /// </summary>
        public double MonthlyIncome
        {
            get
            {
                if (RequiresExtended)
                    return (BasicIncome ?? 0D) + ExtendedEmployment.VariableIncome;
                else
                    return (BasicIncome ?? 0D);
            }
        }

        /// <summary>
        /// Gets the total confirmed income for the employment object (this will take extended employment 
        /// information into account).
        /// </summary>
        public double ConfirmedIncome
        {
            get
            {
                if (RequiresExtended)
                    return (ConfirmedBasicIncome ?? 0D) + ExtendedEmployment.ConfirmedVariableIncome;
                else
                    return (ConfirmedBasicIncome ?? 0D);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Helper method for subclasses to build their list of supported remuneration types.
        /// </summary>
        /// <param name="enumRemunerationTypes">The type of an enumeration containing remuneration types e.g. <c>typeof(EmploymentSalariedRemunerationTypes)</c></param>
        /// <returns></returns>
        protected IReadOnlyEventList<RemunerationTypes> GetSupportedRemunerationTypes(Type enumRemunerationTypes)
        {
            List<RemunerationTypes> lst = new List<RemunerationTypes>();
            foreach (string val in Enum.GetNames(enumRemunerationTypes))
                lst.Add((RemunerationTypes)Enum.Parse(enumRemunerationTypes, val));

            return new ReadOnlyEventList<RemunerationTypes>(lst);

        }

        /// <summary>
        /// Refreshes the value in the extended employment variable depending on the remuneration type
        /// </summary>
        private void RefreshExtendedEmployment()
        {
            if (RemunerationType == null)
            {
                _extendedEmployment = null;
                return;
            }

            // only salaried and subsidised - use EmploymentType property rather than typed "is" checks as we 
            // may just be a standard Employment object from an HQL query
            if (this.EmploymentType.Key != (int)EmploymentTypes.Salaried && this.EmploymentType.Key != (int)EmploymentTypes.SalariedwithDeduction)
            {
                _extendedEmployment = null;
                return;
            }

            // only salaried and BasicAndCommission
            if (RemunerationType.Key != (int)RemunerationTypes.Salaried && RemunerationType.Key != (int)RemunerationTypes.BasicAndCommission)
            {
                _extendedEmployment = null;
                return;
            }

            if (_extendedEmployment == null)
                _extendedEmployment = new ExtendedEmployment(this);

        }

        /// <summary>
        /// Declared as virtul so this can be overridden.
        /// </summary>
        public virtual IEmploymentConfirmationSource EmploymentConfirmationSource
        {
            get
            {
                if (null == _DAO.EmploymentConfirmationSource) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IEmploymentConfirmationSource, EmploymentConfirmationSource_DAO>(_DAO.EmploymentConfirmationSource);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.EmploymentConfirmationSource = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.EmploymentConfirmationSource = (EmploymentConfirmationSource_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        #endregion

        protected void OnSubsidies_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnSubsidies_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }


        protected void OnEmploymentVerificationProcesses_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnEmploymentVerificationProcesses_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnEmploymentVerificationProcesses_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnEmploymentVerificationProcesses_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

    }
}


