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
	/// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO
	/// </summary>
	public partial class QuestionnaireEmail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO>, IQuestionnaireEmail
	{
				public QuestionnaireEmail(SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO QuestionnaireEmail) : base(QuestionnaireEmail)
		{
			this._DAO = QuestionnaireEmail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.EmailSubject
		/// </summary>
		public String EmailSubject 
		{
			get { return _DAO.EmailSubject; }
			set { _DAO.EmailSubject = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.EmailBody
		/// </summary>
		public String EmailBody 
		{
			get { return _DAO.EmailBody; }
			set { _DAO.EmailBody = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.ContentType
		/// </summary>
		public IContentType ContentType 
		{
			get
			{
				if (null == _DAO.ContentType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IContentType, ContentType_DAO>(_DAO.ContentType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ContentType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ContentType = (ContentType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.QuestionnaireQuestionnaireEmails
		/// </summary>
		private DAOEventList<QuestionnaireQuestionnaireEmail_DAO, IQuestionnaireQuestionnaireEmail, QuestionnaireQuestionnaireEmail> _QuestionnaireQuestionnaireEmails;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO.QuestionnaireQuestionnaireEmails
		/// </summary>
		public IEventList<IQuestionnaireQuestionnaireEmail> QuestionnaireQuestionnaireEmails
		{
			get
			{
				if (null == _QuestionnaireQuestionnaireEmails) 
				{
					if(null == _DAO.QuestionnaireQuestionnaireEmails)
						_DAO.QuestionnaireQuestionnaireEmails = new List<QuestionnaireQuestionnaireEmail_DAO>();
					_QuestionnaireQuestionnaireEmails = new DAOEventList<QuestionnaireQuestionnaireEmail_DAO, IQuestionnaireQuestionnaireEmail, QuestionnaireQuestionnaireEmail>(_DAO.QuestionnaireQuestionnaireEmails);
					_QuestionnaireQuestionnaireEmails.BeforeAdd += new EventListHandler(OnQuestionnaireQuestionnaireEmails_BeforeAdd);					
					_QuestionnaireQuestionnaireEmails.BeforeRemove += new EventListHandler(OnQuestionnaireQuestionnaireEmails_BeforeRemove);					
					_QuestionnaireQuestionnaireEmails.AfterAdd += new EventListHandler(OnQuestionnaireQuestionnaireEmails_AfterAdd);					
					_QuestionnaireQuestionnaireEmails.AfterRemove += new EventListHandler(OnQuestionnaireQuestionnaireEmails_AfterRemove);					
				}
				return _QuestionnaireQuestionnaireEmails;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_QuestionnaireQuestionnaireEmails = null;
			
		}
	}
}


