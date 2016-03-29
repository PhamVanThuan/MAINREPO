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
	/// SAHL.Common.BusinessModel.DAO.Image_DAO
	/// </summary>
	public partial class Image : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Image_DAO>, IImage
	{
				public Image(SAHL.Common.BusinessModel.DAO.Image_DAO Image) : base(Image)
		{
			this._DAO = Image;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Image_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Image_DAO.URL
		/// </summary>
		public String URL 
		{
			get { return _DAO.URL; }
			set { _DAO.URL = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Image_DAO.AnswerImages
		/// </summary>
		private DAOEventList<AnswerImage_DAO, IAnswerImage, AnswerImage> _AnswerImages;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Image_DAO.AnswerImages
		/// </summary>
		public IEventList<IAnswerImage> AnswerImages
		{
			get
			{
				if (null == _AnswerImages) 
				{
					if(null == _DAO.AnswerImages)
						_DAO.AnswerImages = new List<AnswerImage_DAO>();
					_AnswerImages = new DAOEventList<AnswerImage_DAO, IAnswerImage, AnswerImage>(_DAO.AnswerImages);
					_AnswerImages.BeforeAdd += new EventListHandler(OnAnswerImages_BeforeAdd);					
					_AnswerImages.BeforeRemove += new EventListHandler(OnAnswerImages_BeforeRemove);					
					_AnswerImages.AfterAdd += new EventListHandler(OnAnswerImages_AfterAdd);					
					_AnswerImages.AfterRemove += new EventListHandler(OnAnswerImages_AfterRemove);					
				}
				return _AnswerImages;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_AnswerImages = null;
			
		}
	}
}


