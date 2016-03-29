using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Administration.Interfaces
{
	/// <summary>
	/// Attorney Interface
	/// </summary>
	public interface IAttorney : IViewBase
	{

		#region View Properties

		/// <summary>
		/// 
		/// </summary>
		int GetSet_ddlDeedsOffice { get; set;}

		/// <summary>
		/// 
		/// </summary>
		int GetSet_ddlAttorney { get; set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_tdAttorneyVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_ddlAttorneyVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_pnlAttorneyDetailsVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_tdAttorneyNumberVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblAttorneyNumberVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_tdStatusVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblStatusVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_ddStatusVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblAttorneyNameVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_txtAttorneyNameVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblWorkflowEnabledVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_ddlWorkflowEnabledVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblAttorneyContactVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_txtAttorneyContactVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblAttorneyMandateVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_txtAttorneyMandateVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblPhoneNumberVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_txtPhoneNumberVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblRegistrationAttorneyVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_ddlRegistrationAttorneyVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblEmailAddressVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_txtEmailAddressVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblLitigationAttorneyVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_ddlLitigationAttorneyVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_tdDeedsOfficeChangeVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_ddlDeedsOfficeChangeVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_pnlAddressVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblAddressTypeVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_ddlAddressTypeVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblAddressFormatVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_ddlAddressFormatVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblEffectiveDateVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_dtEffectiveDateVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_lblAddressVisibility { set;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_addressDetailsVisibility { set;}

        /// <summary>
        /// 
        /// </summary>
        DateTime Set_EffectiveDateDefault { set;}

		/// <summary>
		/// 
		/// </summary>
		int Get_ddlStatus { get;}

		/// <summary>
		/// 
		/// </summary>
		string Get_txtAttorneyName { get;}

		/// <summary>
		/// 
		/// </summary>
		int Get_ddlWorkflowEnabled { get;}

		/// <summary>
		/// 
		/// </summary>
		string Get_txtAttorneyContact { get;}

		/// <summary>
		/// 
		/// </summary>
		double Get_txtAttorneyMandate { get;}

		/// <summary>
		/// 
		/// </summary>
		string Get_txtPhoneNumberCode { get;}

		/// <summary>
		/// 
		/// </summary>
		string Get_txtPhoneNumber { get;}

		/// <summary>
		/// 
		/// </summary>
		bool Get_ddlRegistrationAttorney { get;}

		/// <summary>
		/// 
		/// </summary>
		string Get_txtEmailAddress { get;}

		/// <summary>
		/// 
		/// </summary>
		bool Get_ddlLitigationAttorney { get;}

		/// <summary>
		/// 
		/// </summary>
		int Get_ddlDeedsOfficeChange { get;}

		/// <summary>
		/// 
		/// </summary>
		int Get_ddlAddressTypeSelectedValue { get;}
		
		/// <summary>
		/// 
		/// </summary>
		int Get_ddlAddressFormatSelectedValue { get;}

		/// <summary>
		/// 
		/// </summary>
		DateTime GetSet_dtEffectiveDate { get; set;}

		/// <summary>
		/// 
		/// </summary>
		AddressFormats GetSet_AddressFormat { get; set;}

		/// <summary>
		/// 
		/// </summary>
		IAddress Get_CapturedAddress { get;}

		/// <summary>
		/// 
		/// </summary>
		bool Set_CancelButtonVisibility { set;}

        /// <summary>
        /// 
        /// </summary>
        bool Set_SubmitButtonVisibility { set; }

        /// <summary>
        /// 
        /// </summary>
        bool Set_ContactsButtonVisibility { set; }

        /// <summary>
        /// 
        /// </summary>
        bool LitigationAttorneyAuthenticated { get; }

		#endregion

		#region EventHandlers

		/// <summary>
		/// submit button clicked
		/// </summary>
		event EventHandler OnSubmitButtonClicked;

		/// <summary>
		/// cancel button clicked
		/// </summary>
		event EventHandler OnCancelButtonClicked;

        /// <summary>
		/// Contacts button clicked
		/// </summary>
        event EventHandler OnContactsButton_Clicked;

		/// <summary>
		/// Deeds Office ddl Changed
		/// </summary>
		event KeyChangedEventHandler OnDeedsOfficeSelectedIndexChanged;

		/// <summary>
		/// Attorney ddl Changed
		/// </summary>
		event KeyChangedEventHandler OnAttorneySelectedIndexChanged;

		/// <summary>
		/// AddressType ddl changed Event Handler
		/// </summary>
		event KeyChangedEventHandler OnAddressTypeSelectedIndexChanged;

		/// <summary>
		/// AddressFormat ddl changed Event Handler
		/// </summary>
		event KeyChangedEventHandler OnAddressFormatSelectedIndexChanged;

		/// <summary>
		/// add address button clicked when user selects address on ajax
		/// </summary>
		event KeyChangedEventHandler SelectAddressButtonClicked;

		#endregion

		#region  View Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="deedsOfficeList"></param>
		void BindDeedsOfficeDropDown(IEventList<IDeedsOffice> deedsOfficeList);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attorneyList"></param>
		void BindAttorneyDropDown(IDictionary<int, string> attorneyList);

		/// <summary>
		/// 
		/// </summary>
		void ClearAttorneyControls();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attorney"></param>
		void PopulateAttorneyControls(SAHL.Common.BusinessModel.Interfaces.IAttorney attorney);

		/// <summary>
		/// 
		/// </summary>
		void BindAddressFormatDropDown(ISAHLDictionary<int, string> addressFormatList);

		/// <summary>
		/// 
		/// </summary>
		void BindAddressTypeDropDown(IDictionary<int, string> addressTypes);

		/// <summary>
		/// 
		/// </summary>
		void PopulateStatusDropDown(IList<IGeneralStatus> generalStatus);

		/// <summary>
		/// 
		/// </summary>
		void PopulateWorkflowEnabledDropDown();

		/// <summary>
		/// 
		/// </summary>
		void PopulateRegistrationAttorneyDropDown();

		/// <summary>
		/// 
		/// </summary>
		void PopulateLitigationAttorneyDropDown();

		/// <summary>
		/// 
		/// </summary>
		void PopulateRegistrationLitigationControls(bool? registrationInd, bool? litigationInd);

		/// <summary>
		/// 
		/// </summary>
		void BindAttorneyAddress(IAddress address);

		/// <summary>
		/// 
		/// </summary>
		void ClearAddress();

		#endregion

	}
}
