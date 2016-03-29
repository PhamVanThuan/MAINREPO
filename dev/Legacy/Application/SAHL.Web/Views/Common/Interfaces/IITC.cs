using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IITC : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listITC"></param>
        void BindITCGrid(List<BindableITC> listITC);

        #region BindOtherAccountITCGrid
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="listITC"></param>
        //void BindOtherAccountITCGrid(List<BindableITC> listITC);
        #endregion


        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnDoEnquiryButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnViewHistoryButtonClicked;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        List<Int32> ListITCDoEnquiry
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 LegalEntityKeyForHistory
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool ViewHistoryColumnVisible
        {
            set;
        }

        /// <summary>
        /// Gets/sets whether the Do Enquiry column is visible on the grid.
        /// </summary>
        bool DoEnquiryColumnVisible
        {
            set;
        }

        /// <summary>
        /// Gets/sets whether the AccountKey column is visible on the grid.
        /// </summary>
        bool AccountColumnVisible
        {
            set;
        }

        /// <summary>
        /// Gets/sets whether the Status column is visible on the grid.
        /// </summary>
        bool StatusColumnVisible
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool DoEnquiryButtonVisible
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool BackButtonVisible
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string HeaderCaption
        {
            get;
            set;
        }

        #endregion
    }


    /// <summary>
    /// Class to make binding to the Grid easier because it is a mix of LegalEntity and ITC data
    /// </summary>
    public class BindableITC
    {
        internal int _key;
        public int Key { get { return _key; } }
        internal Int32 _LegalEntityKey;
        public Int32 LegalEntityKey { get { return _LegalEntityKey; } }
        internal Int32 _AccountKey;
        public Int32 AccountKey { get { return _AccountKey; } }
        internal string _DisplayName;
        public string DisplayName { get { return _DisplayName; } }
        internal string _IDNumber;
        public string IDNumber { get { return _IDNumber; } }
        internal string _ResponseStatus;
        public string ResponseStatus { get { return _ResponseStatus; } }
        internal DateTime _ChangeDate;
        public DateTime ChangeDate { get { return _ChangeDate; } }
        internal string _UserID;
        public string UserID { get { return _UserID; } }
        internal int _ArchiveCount;
        public int ArchiveCount { get { return _ArchiveCount; } }
        internal bool _IsHistory;
        public bool IsHistory { get { return _IsHistory; } }
        internal bool _RedoITC;
        public bool RedoITC { get { return _RedoITC; } }
        internal bool _performITC;
        public bool PerformITC { get {return _performITC;} }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public BindableITC() { }

        /// <summary>
        /// Constructor for existing enquiries
        /// </summary>
        /// <param name="itc"></param>
        /// <param name="le"></param>
        /// <param name="ArchivedITCCount"></param>
        /// <param name="redoITC"></param>
        /// <param name="performITC"></param>
        public BindableITC(SAHL.Common.BusinessModel.Interfaces.IITC itc, ILegalEntityNaturalPerson le, int ArchivedITCCount, bool redoITC, bool performITC)
        {
            this._key = itc.Key;
            this._LegalEntityKey = le.Key;
            if (itc.ReservedAccount != null)
                this._AccountKey = itc.ReservedAccount.Key;
            this._ResponseStatus = itc.ResponseStatus;
            this._ChangeDate = itc.ChangeDate;
            this._UserID = itc.UserID;
            this._DisplayName = le.DisplayName;
            this._IDNumber = le.IDNumber;
            this._ArchiveCount = ArchivedITCCount;
            //this._IsHistory = false;
            this._RedoITC = redoITC;
            this._performITC = performITC;
        }

        #region old
        ///// <summary>
        ///// Constructor for Archived enquiries
        ///// </summary>
        ///// <param name="itc"></param>
        ///// <param name="le"></param>
        ///// <param name="AccountKey"></param>
        //public BindableITC(IITCArchive itc, ILegalEntityNaturalPerson le)
        //{
        //    this._key = itc.Key;
        //    this._LegalEntityKey = itc.LegalEntityKey;
        //    this._AccountKey = itc.AccountKey;
        //    this._ResponseStatus = itc.ResponseStatus;
        //    this._ChangeDate = itc.ChangeDate;
        //    this._UserID = itc.UserID;
        //    this._DisplayName = le.DisplayName;
        //    this._IDNumber = le.IDNumber;
        //    //this._ArchiveCount = 0;
        //    this._IsHistory = true;
        //}
        #endregion


        /// <summary>
        /// Constructor for History of enquiries
        /// </summary>
        /// <param name="itc"></param>
        /// <param name="le"></param>
        public BindableITC(System.Data.DataRow itc, ILegalEntityNaturalPerson le)
        {
            this._key = Convert.ToInt32(itc[0]);
            this._LegalEntityKey = Convert.ToInt32(itc[1]);
            this._AccountKey = Convert.ToInt32(itc[2]);
            this._ResponseStatus = itc[5].ToString();
            this._ChangeDate = Convert.ToDateTime(itc[3]);
            this._UserID = itc[6].ToString();
            this._DisplayName = le.DisplayName;
            this._IDNumber = le.IDNumber;
            //this._ArchiveCount = 0;
            this._IsHistory = !(String.IsNullOrEmpty(itc[7].ToString()));
        }

    }
}
