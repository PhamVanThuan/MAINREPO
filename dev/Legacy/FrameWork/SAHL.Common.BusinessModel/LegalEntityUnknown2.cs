using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class LegalEntityUnknown : LegalEntity, ILegalEntityUnknown
    {
        #region Properties

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
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Salutation = (Salutation_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public String FirstNames
        {
            get { return _DAO.FirstNames; }
            set { _DAO.FirstNames = value; }
        }

        public String Initials
        {
            get { return _DAO.Initials; }
            set { _DAO.Initials = value; }
        }

        public String Surname
        {
            get { return _DAO.Surname; }
            set { _DAO.Surname = value; }
        }

        #endregion Properties

        #region Events

        #endregion Events

        #region Methods

        public override string GetLegalName(LegalNameFormat Format)
        {
            string Name = "";
            if (Salutation != null && Salutation.Description != null && Salutation.Description.Length > 0)
                Name += Salutation.Description.Trim();

            switch (Format)
            {
                case LegalNameFormat.Full:
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

        #endregion Methods
    }
}