using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Property_DAO
    /// </summary>
    public partial interface IProperty : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDescription1
        /// </summary>
        System.String PropertyDescription1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDescription2
        /// </summary>
        System.String PropertyDescription2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDescription3
        /// </summary>
        System.String PropertyDescription3
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.DeedsOfficeValue
        /// </summary>
        Double? DeedsOfficeValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.CurrentBondDate
        /// </summary>
        DateTime? CurrentBondDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.ErfNumber
        /// </summary>
        System.String ErfNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.ErfPortionNumber
        /// </summary>
        System.String ErfPortionNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.SectionalSchemeName
        /// </summary>
        System.String SectionalSchemeName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.SectionalUnitNumber
        /// </summary>
        System.String SectionalUnitNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.ErfSuburbDescription
        /// </summary>
        System.String ErfSuburbDescription
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.ErfMetroDescription
        /// </summary>
        System.String ErfMetroDescription
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.MortgageLoanProperties
        /// </summary>
        IEventList<IFinancialService> MortgageLoanProperties
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyTitleDeeds
        /// </summary>
        IEventList<IPropertyTitleDeed> PropertyTitleDeeds
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.Valuations
        /// </summary>
        IEventList<IValuation> Valuations
        {
            get;
        }

        /// <summary>
        /// The address of the Property
        /// </summary>
        IAddress Address
        {
            get;
            set;
        }

        /// <summary>
        /// The Area Classification of property
        /// </summary>
        IAreaClassification AreaClassification
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.DeedsPropertyType
        /// </summary>
        IDeedsPropertyType DeedsPropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.OccupancyType
        /// </summary>
        IOccupancyType OccupancyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyType
        /// </summary>
        IPropertyType PropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.TitleType
        /// </summary>
        ITitleType TitleType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.DataProvider
        /// </summary>
        IDataProvider DataProvider
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDatas
        /// </summary>
        IEventList<IPropertyData> PropertyDatas
        {
            get;
        }
    }
}