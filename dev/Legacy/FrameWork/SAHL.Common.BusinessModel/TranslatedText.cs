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
	/// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO
	/// </summary>
	public partial class TranslatedText : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TranslatedText_DAO>, ITranslatedText
	{
				public TranslatedText(SAHL.Common.BusinessModel.DAO.TranslatedText_DAO TranslatedText) : base(TranslatedText)
		{
			this._DAO = TranslatedText;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO.Text
		/// </summary>
		public String Text 
		{
			get { return _DAO.Text; }
			set { _DAO.Text = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO.TranslatableItem
		/// </summary>
		public ITranslatableItem TranslatableItem 
		{
			get
			{
				if (null == _DAO.TranslatableItem) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITranslatableItem, TranslatableItem_DAO>(_DAO.TranslatableItem);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TranslatableItem = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TranslatableItem = (TranslatableItem_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TranslatedText_DAO.Language
		/// </summary>
		public ILanguage Language 
		{
			get
			{
				if (null == _DAO.Language) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILanguage, Language_DAO>(_DAO.Language);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Language = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Language = (Language_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


