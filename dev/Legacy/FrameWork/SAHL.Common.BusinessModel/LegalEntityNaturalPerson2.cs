using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel
{
    public partial class LegalEntityNaturalPerson : LegalEntity, ILegalEntityNaturalPerson
    {
        #region Properties

        public override string DisplayName
        {
            get
            {
                return GetLegalName(LegalNameFormat.Full);
            }
        }

        public int? AgeNextBirthday
        {
            get
            {
                if (this.DateOfBirth.HasValue)
                    return DateUtils.CalculateAgeNextBirthday(this.DateOfBirth.Value);
                else
                    return null;
            }
        }

        public int? CurrentAge
        {
            get
            {
                if (this.AgeNextBirthday.HasValue)
                    return this.AgeNextBirthday.Value - 1;
                else
                    return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public String IDNumber
        {
            get
            {
                return _DAO.IDNumber;
            }
            set
            {
                // for out comparison logic, convert both values to 'null' if they are ''
                string idNumberOnDB = string.IsNullOrEmpty(_DAO.IDNumber) ? null : _DAO.IDNumber.Trim();
                string idNumberNew = string.IsNullOrEmpty(value) ? null : value.Trim();

                if (!this.IsUpdatable && idNumberNew != idNumberOnDB)
                {
                    string msg = "ID Number: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    PrintErrorMessage(msg);
                    return;
                }
                this._DAO.IDNumber = idNumberNew;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public String PassportNumber
        {
            get { return _DAO.PassportNumber; }
            set
            {
                this._DAO.PassportNumber = value;
            }
        }

        /// <summary>
        /// Domain Rule: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account
        /// and the Legal Entity does not have the Exception Status of InvalidIDNumber.
        /// </summary>
        public IMaritalStatus MaritalStatus
        {
            get
            {
                if (null == _DAO.MaritalStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IMaritalStatus, MaritalStatus_DAO>(_DAO.MaritalStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.MaritalStatus = null;
                    return;
                }

                //if (!CheckUpdateOpenAccount() && value.Key != _DAO.MaritalStatus.Key)
                if (!this.IsUpdatable && value.Key != _DAO.MaritalStatus.Key)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string msg = "MaritalStatus: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    spc.DomainMessages.Add(new Error(msg, msg));

                    return;
                }

                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.MaritalStatus = (MaritalStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Domain Rule: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account
        /// and the Legal Entity does not have the Exception Status of InvalidIDNumber.
        /// </summary>
        public IGender Gender
        {
            get
            {
                if (null == _DAO.Gender) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGender, Gender_DAO>(_DAO.Gender);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Gender = null;
                    return;
                }

                //if (!CheckUpdateOpenAccount() && value.Key != _DAO.Gender.Key)
                if (!this.IsUpdatable && value.Key != _DAO.Gender.Key)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string msg = "Gender: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    spc.DomainMessages.Add(new Error(msg, msg));

                    return;
                }

                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Gender = (Gender_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ISalutation Salutation
        {
            get
            {
                if (null == _DAO.Salutation) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ISalutation, Salutation_DAO>(_DAO.Salutation);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Salutation = null;
                    return;
                }

                // We should always be allowed to change the salutation
                //if (!CheckUpdateOpenAccount() && value.Key != _DAO.Salutation.Key)
                //{
                //    SAHLPrincipalCache SPC = SAHLPrincipal.GetSAHLPrincipalCache();
                //    string msg = "Salutation: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                //    spc.DomainMessages.Add(new Error(msg, msg));

                //    return;
                //}

                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Salutation = (Salutation_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Domain Rule: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account
        /// and the Legal Entity does not have the Exception Status of InvalidIDNumber.        /// </summary>
        public String FirstNames
        {
            get { return _DAO.FirstNames; }
            set
            {
                //if (!CheckUpdateOpenAccount() && value != _DAO.FirstNames)
                if (!this.IsUpdatable && value != _DAO.FirstNames)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string msg = "FirstNames: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    spc.DomainMessages.Add(new Error(msg, msg));

                    return;
                }

                _DAO.FirstNames = value;
            }
        }

        /// <summary>
        /// Domain Rule: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account
        /// and the Legal Entity does not have the Exception Status of InvalidIDNumber.        /// </summary>
        public String Initials
        {
            get { return _DAO.Initials; }
            set
            {
                //if (!CheckUpdateOpenAccount() && value != _DAO.Initials)
                if (!this.IsUpdatable && value != _DAO.Initials)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string msg = "Initials: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    spc.DomainMessages.Add(new Error(msg, msg));

                    return;
                }

                _DAO.Initials = value;
            }
        }

        /// <summary>
        /// Domain Rule: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account
        /// and the Legal Entity does not have the Exception Status of InvalidIDNumber.        /// </summary>
        public String Surname
        {
            get { return _DAO.Surname; }
            set
            {
                //if (!CheckUpdateOpenAccount() && value != _DAO.Surname)
                if (!this.IsUpdatable && value != _DAO.Surname)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string msg = "Surname: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    spc.DomainMessages.Add(new Error(msg, msg));

                    return;
                }

                _DAO.Surname = value;
            }
        }

        /// <summary>
        /// Domain Rule: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account
        /// and the Legal Entity does not have the Exception Status of InvalidIDNumber.        /// </summary>
        public ICitizenType CitizenType
        {
            get
            {
                if (null == _DAO.CitizenType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICitizenType, CitizenType_DAO>(_DAO.CitizenType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CitizenType = null;
                    return;
                }

                //if (!CheckUpdateOpenAccount() && value.Key != _DAO.CitizenType.Key)
                if (!this.IsUpdatable && value.Key != _DAO.CitizenType.Key)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string msg = "CitizenType: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    spc.DomainMessages.Add(new Error(msg, msg));

                    return;
                }

                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CitizenType = (CitizenType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        #endregion Properties

        /// <summary>
        /// Overridden to return the ID number of the natural person.  If this does not exist, the passport number is returned.
        /// </summary>
        /// <remarks>This is for display purposes only.</remarks>
        public override string LegalNumber
        {
            get
            {
                if (String.IsNullOrEmpty(IDNumber))
                    return PassportNumber;
                else
                    return IDNumber;
            }
        }

        public override string GetLegalName(LegalNameFormat Format)
        {
            string Name = "";
            if (Format != LegalNameFormat.FullNoSalutation && Format != LegalNameFormat.InitialsOnlyNoSalutation)
            {
                if (Salutation != null && Salutation.Description != null && Salutation.Description.Length > 0)
                    Name += Salutation.Description.Trim();
            }

            switch (Format)
            {
                case LegalNameFormat.Full:
                case LegalNameFormat.FullNoSalutation:
                    if (FirstNames != null)
                    {
                        if (Name.Length > 0)
                            Name += " ";
                        Name += FirstNames.Trim();
                    }
                    if (Surname != null)
                    {
                        if (Name.Length > 0)
                            Name += " ";
                        Name += Surname.Trim();
                    }
                    return Name;
                case LegalNameFormat.InitialsOnly:
                case LegalNameFormat.InitialsOnlyNoSalutation:
                    if (Initials != null && Initials.Length > 0)
                    {
                        if (Name.Length > 0)
                            Name += " ";
                        Name += Initials.Trim();
                    }
                    else
                        if (FirstNames != null && FirstNames.Length > 0)
                        {
                            if (Name.Length > 0)
                                Name += " ";
                            Name += FirstNames.Trim();
                        }
                        else
                        {
                        }
                    if (Surname != null)
                    {
                        if (Name.Length > 0)
                            Name += " ";
                        Name += Surname.Trim();
                    }
                    return Name;
                case LegalNameFormat.SurnamesOnly:
                    if (Surname != null)
                    {
                        if (Name.Length > 0)
                            Name += " ";
                        Name += Surname.Trim();
                    }
                    return Name;
                default:
                    return Name;
            }
        }

        #region Public Methods

        // Methods have been removed by Nazir J 2008-07-14
        /*
                /// <summary>
        /// Rules:
        /// The passport number may not be diplicated accross Legal Entities.
        /// Ensure that the passport number is not captured as a SA ID Number.
        /// </summary>
        /// <param name="PassportNumber"></param>
        /// <param name="CitizenType"></param>

		public void SetPassportNumber(string PassportNumber, CitizenTypes CitizenType)
        {
            bool error = false;
            string errorMessage = null;

            // If the Passport Number passes the IDNumber Validation, its probably an IDNumber.
            if (ValidationUtils.ValidateID(PassportNumber))
            {
                error = true;
                errorMessage = "The Passport Number appears to be an ID Number.";
            }

            // Check for uniqueness
            if (!PassportNumber.Equals(_DAO.PassportNumber))
            {
                LegalEntityNaturalPerson_DAO[] matches = LegalEntityNaturalPerson_DAO.FindAllByProperty("PassportNumber", PassportNumber);

                foreach (LegalEntityNaturalPerson_DAO leNP_DAO in matches)
                {
                    // Belongs to another LE.
                    if (leNP_DAO.Key != this.Key)
                    {
                        error = true;
                        errorMessage = "The Passport Number must be unique.";

                        break;
                    }
                }
            }

            // Check for Citizenship
            if (CitizenType == CitizenTypes.SACitizen)
            {
                error = true;
                errorMessage = "The citizenship may not be South African, please use the ID Number for South African Citizens.";
            }

            if (error)
            {
                SAHLPrincipal currentPrincipal = new SAHLPrincipal(WindowsIdentity.GetCurrent());
                SAHLPrincipalCache sahlPrincipalCache = SAHLPrincipalCache.GetPrincipalCache(currentPrincipal);

                sahlPrincipalCache.DomainMessages.Add(new Error(errorMessage, String.Empty));
            }
            else
                this._DAO.PassportNumber = PassportNumber;
        }

		        /// <summary>
        /// Sets the ID Number for South African citizens. Will automatically set the citizen type to South African.
        /// </summary>
        /// <param name="IDNumber"></param>
        public void SetIDNumber(string IDNumber)
        {
            bool error = false;
            string errorMessage = null;

            //
            if (!CheckUpdateOpenAccount() && IDNumber != _DAO.IDNumber)
            {
                SAHLPrincipalCache SPC = SAHLPrincipal.GetSAHLPrincipalCache();
                string msg = "ID Number: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                spc.DomainMessages.Add(new Error(msg, msg));

                return;
            }

            // Is the ID Number Valid?
            if (String.IsNullOrEmpty(IDNumber) || !ValidationUtils.ValidateID(IDNumber))
            {
                error = true;
                errorMessage = String.Format("The ID Number is invalid: {0}.", GetLegalName(LegalNameFormat.Full));
            }
            else
            {
                // Is the ID Number Unique?
                if (!IDNumber.Equals(_DAO.IDNumber))
                {
                    LegalEntityNaturalPerson_DAO[] matches = LegalEntityNaturalPerson_DAO.FindAllByProperty("IDNumber", IDNumber);

                    if (matches.Length > 0)
                    {
                        string person = (matches[0].Salutation != null ? matches[0].Salutation.Description + " " : "" ) + matches[0].FirstNames + " " + matches[0].Surname;
                        error = true;
                        errorMessage = "A Legal Entity (" + person + ") already exists with this ID Number.";
                    }
                }

                // Automatically change the CitizenType to RSA.
                this._DAO.IDNumber = IDNumber;

                ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                this.CitizenType = lookupRepository.CitizenTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.CitizenTypes.SACitizen)];
            }

            //// Is the ID Number Unique?
            //if (!IDNumber.Equals(_DAO.IDNumber))
            //{
            //    LegalEntityNaturalPerson_DAO[] matches = LegalEntityNaturalPerson_DAO.FindAllByProperty("IDNumber", IDNumber);

            //    if (matches.Length > 0)
            //    {
            //        error = true;
            //        errorMessage = "The ID Number must be unique.";
            //    }
            //}

            if (error)
            {
                SAHLPrincipal currentPrincipal = new SAHLPrincipal(WindowsIdentity.GetCurrent());
                SAHLPrincipalCache sahlPrincipalCache = SAHLPrincipalCache.GetPrincipalCache(currentPrincipal);

                sahlPrincipalCache.DomainMessages.Add(new Error(errorMessage, String.Empty));
            }
        }
        */

        #endregion Public Methods

        #region Private methods

        #endregion Private methods

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("LegalEntityMandatoryName");
            Rules.Add("LegalEntityContactDetailsMandatory");
            Rules.Add("LegalEntityNaturalPersonMandatorySaluation");
            Rules.Add("LegalEntityNaturalPersonMandatoryInitials");
            // Preferred Name is optional ticket #9693
            //Rules.Add("LegalEntityNaturalPersonMandatoryPreferredName");
            Rules.Add("LegalEntityNaturalPersonMandatoryGender");
            Rules.Add("LegalEntityNaturalPersonMandatoryMaritalStatus");
            Rules.Add("LegalEntityNaturalPersonMandatoryPopulationGroup");
            Rules.Add("LegalEntityNaturalPersonMandatoryEducation");
            Rules.Add("LegalEntityNaturalPersonMandatoryCitizenType");
            Rules.Add("LegalEntityNaturalPersonMandatoryIDNumber");
            Rules.Add("LegalEntityNaturalPersonMandatoryPassportNumber");
            Rules.Add("LegalEntityNaturalPersonMandatoryDateOfBirth");
            Rules.Add("LegalEntityNaturalPersonMandatoryHomeLanguage");
            Rules.Add("LegalEntityNaturalPersonMandatoryDocumentLanguage");
            Rules.Add("LegalEntityNaturalPersonMandatoryLegalEntityStatus");
            Rules.Add("LegalEntityNaturalPersonValidateIDNumber");
            Rules.Add("LegalEntityNaturalPersonValidatePassportNumber");
            Rules.Add("LegalEntityNaturalPersonIsPassportNumberUnique");
            Rules.Add("LegalEntityNaturalPersonIsIDNumberUnique");
            Rules.Add("LegalEntityNaturalPersonItsAForeigner");
            Rules.Add("LegalEntityNaturalPersonUpdateToForeigner");
        }
    }
}