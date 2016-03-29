using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO
	/// </summary>
	public partial class QuestionnaireQuestionnaireEmail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO>, IQuestionnaireQuestionnaireEmail
	{
				public QuestionnaireQuestionnaireEmail(SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO QuestionnaireQuestionnaireEmail) : base(QuestionnaireQuestionnaireEmail)
		{
			this._DAO = QuestionnaireQuestionnaireEmail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO.InternalEmail
		/// </summary>
		public Int32 InternalEmail 
		{
			get { return _DAO.InternalEmail; }
			set { _DAO.InternalEmail = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO.Questionnaire
		/// </summary>
		public IQuestionnaire Questionnaire 
		{
			get
			{
				if (null == _DAO.Questionnaire) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IQuestionnaire, Questionnaire_DAO>(_DAO.Questionnaire);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Questionnaire = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Questionnaire = (Questionnaire_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionnaireEmail_DAO.QuestionnaireEmail
		/// </summary>
		public IQuestionnaireEmail QuestionnaireEmail 
		{
			get
			{
				if (null == _DAO.QuestionnaireEmail) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IQuestionnaireEmail, QuestionnaireEmail_DAO>(_DAO.QuestionnaireEmail);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.QuestionnaireEmail = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.QuestionnaireEmail = (QuestionnaireEmail_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


