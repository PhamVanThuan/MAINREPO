using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Data_DAO
    /// </summary>
    public partial interface IData : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.SecurityGroup
        /// </summary>
        System.String SecurityGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.ArchiveDate
        /// </summary>
        System.String ArchiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.DataContainer
        /// </summary>
        System.Decimal DataContainer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.BackupVolume
        /// </summary>
        System.Decimal BackupVolume
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Overlay
        /// </summary>
        System.String Overlay
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.STOR
        /// </summary>
        System.Decimal STOR
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.GUID
        /// </summary>
        System.String GUID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Extension
        /// </summary>
        System.String Extension
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key1
        /// </summary>
        System.String Key1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key2
        /// </summary>
        System.String Key2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key3
        /// </summary>
        System.String Key3
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key4
        /// </summary>
        System.String Key4
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key5
        /// </summary>
        System.String Key5
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key6
        /// </summary>
        System.String Key6
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key7
        /// </summary>
        System.String Key7
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key8
        /// </summary>
        System.String Key8
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgTo
        /// </summary>
        System.String MsgTo
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgFrom
        /// </summary>
        System.String MsgFrom
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgSubject
        /// </summary>
        System.String MsgSubject
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgReceived
        /// </summary>
        DateTime? MsgReceived
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.MsgSent
        /// </summary>
        DateTime? MsgSent
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key9
        /// </summary>
        System.String Key9
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key10
        /// </summary>
        System.String Key10
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key11
        /// </summary>
        System.String Key11
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key12
        /// </summary>
        System.String Key12
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key13
        /// </summary>
        System.String Key13
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key14
        /// </summary>
        System.String Key14
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key15
        /// </summary>
        System.String Key15
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key16
        /// </summary>
        System.String Key16
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Title
        /// </summary>
        System.String Title
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.OriginalFilename
        /// </summary>
        System.String OriginalFilename
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Data_DAO.Key
        /// </summary>
        System.Decimal Key
        {
            get;
            set;
        }
    }
}