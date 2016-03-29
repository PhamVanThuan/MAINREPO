using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO
	/// </summary>
	public partial interface IClientSurvey : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.BusinessEventQuestionnaire
		/// </summary>
		IBusinessEventQuestionnaire BusinessEventQuestionnaire
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.DatePresented
		/// </summary>
		System.DateTime DatePresented
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.ADUser
		/// </summary>
		IADUser ADUser
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.GenericKey
		/// </summary>
		System.Int32 GenericKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.GenericKeyType
		/// </summary>
		IGenericKeyType GenericKeyType
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.DateReceived
		/// </summary>
		System.DateTime DateReceived
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.LegalEntity
		/// </summary>
		ILegalEntity LegalEntity
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ClientSurvey_DAO.ClientAnswers
		/// </summary>
		IEventList<IClientAnswer> ClientAnswers
		{
			get;
		}
	}
}


