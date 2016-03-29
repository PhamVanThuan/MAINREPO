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
	/// SAHL.Common.BusinessModel.DAO.TranslatableItem_DAO
	/// </summary>
	public partial class TranslatableItem : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TranslatableItem_DAO>, ITranslatableItem
	{
				public TranslatableItem(SAHL.Common.BusinessModel.DAO.TranslatableItem_DAO TranslatableItem) : base(TranslatableItem)
		{
			this._DAO = TranslatableItem;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TranslatableItem_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TranslatableItem_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TranslatableItem_DAO.TranslatedTexts
		/// </summary>
		private DAOEventList<TranslatedText_DAO, ITranslatedText, TranslatedText> _TranslatedTexts;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TranslatableItem_DAO.TranslatedTexts
		/// </summary>
		public IEventList<ITranslatedText> TranslatedTexts
		{
			get
			{
				if (null == _TranslatedTexts) 
				{
					if(null == _DAO.TranslatedTexts)
						_DAO.TranslatedTexts = new List<TranslatedText_DAO>();
					_TranslatedTexts = new DAOEventList<TranslatedText_DAO, ITranslatedText, TranslatedText>(_DAO.TranslatedTexts);
					_TranslatedTexts.BeforeAdd += new EventListHandler(OnTranslatedTexts_BeforeAdd);					
					_TranslatedTexts.BeforeRemove += new EventListHandler(OnTranslatedTexts_BeforeRemove);					
					_TranslatedTexts.AfterAdd += new EventListHandler(OnTranslatedTexts_AfterAdd);					
					_TranslatedTexts.AfterRemove += new EventListHandler(OnTranslatedTexts_AfterRemove);					
				}
				return _TranslatedTexts;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_TranslatedTexts = null;
			
		}
	}
}


